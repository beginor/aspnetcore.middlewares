# NuGet 发布（Trusted Publishing / OIDC）

本仓库通过 nuget.org 的 Trusted Publishing 发布，不再保存长期 API key。
GitHub Actions 用 OIDC 换取一个有效期 1 小时的临时 key，用完即失效。

## 1. 发布元数据：release.env

仓库根目录的 `release.env` 是包元数据的唯一来源，四个变量：

| 变量 | 对应 nuspec 字段 | 说明 |
| --- | --- | --- |
| `PACKAGE_VERSION` | `version` | 必须大于 nuget.org 上已有版本；大版本与 .NET 运行时一致，每次递增一个小版本 |
| `PACKAGE_RELEASE_NOTES` | `releaseNotes` | 本次发布说明 |
| `PACKAGE_TAGS` | `tags` | 逗号分隔的标签 |
| `PACKAGE_PROJECT_URL` | `projectUrl` | 项目主页 |

```bash
export PACKAGE_VERSION="10.1.0"
export PACKAGE_RELEASE_NOTES="Publish via nuget.org Trusted Publishing (OIDC)"
export PACKAGE_TAGS="AspNetCore,Middlewares"
export PACKAGE_PROJECT_URL="https://github.com/beginor/aspnetcore.middlewares"
```

## 2. props 引用方式

`src/package-version.props` 把环境变量映射为 MSBuild 属性：

```xml
<Project>
  <PropertyGroup>
    <PackageVersion>$(PACKAGE_VERSION)</PackageVersion>
    <PackageReleaseNotes>$(PACKAGE_RELEASE_NOTES)</PackageReleaseNotes>
    <PackageTags>$(PACKAGE_TAGS)</PackageTags>
    <PackageProjectUrl>$(PACKAGE_PROJECT_URL)</PackageProjectUrl>
  </PropertyGroup>
</Project>
```

每个 `src/*/*.csproj` 通过 `<Import>` 引用它：

```xml
<Import Project="$(MSBuildThisFileDirectory)../package-version.props" />
```

因此**必须先 `source release.env` 再 pack**，否则变量为空，版本会退化成 `1.0.0`。

## 3. nuget.org Trusted Publishing 策略

nuget.org → 用户菜单 → **Trusted Publishing** → **Create**，字段如下：

| 字段 | 值 |
| --- | --- |
| Package owner | 你的 nuget.org 用户名（即 `beginor`） |
| Repository Owner | `beginor` |
| Repository | `aspnetcore.middlewares` |
| Workflow File | **只填文件名**：`publish-nuget.yml`（不要带 `.github/workflows/` 路径） |
| Environment | **留空**（工作流未使用 environment） |
| Scopes | `Push new packages and package versions` |
| Glob | 包 ID 前缀，如 `Beginor.AspNetCore.Middlewares.*` |

## 4. GitHub 仓库变量

GitHub 仓库 → Settings → Secrets and variables → Actions → **Variables** → New repository variable：

- 名称：`NUGET_USER`
- 值：nuget.org **用户名**（不是邮箱）

工作流通过 `${{ vars.NUGET_USER }}` 读取，传给 `NuGet/login@v1`。

## 5. 工作流要点

- 触发：推送 `[0-9]*` 形式的 tag，或手动 `workflow_dispatch`。
- 权限：只需 `contents: read` 与 `id-token: write`（后者用于 OIDC）。
- `NuGet/login@v1` 输出的 `NUGET_API_KEY` 是**临时 key，只有 1 小时有效期**，不要保存。
- OIDC token 本身**不能**直接当 `--api-key` 使用，必须经 `NuGet/login` 交换。
- `dotnet pack` 一次只接受一个项目，工作流用 `for` 循环逐个打包。
- `dotnet nuget push` 加 `--skip-duplicate`：已存在的版本会跳过而不是报错，便于重跑。

## 6. 发布步骤

```bash
# 1. 修改 release.env 中的 PACKAGE_VERSION / PACKAGE_RELEASE_NOTES
# 2. 本地验证 nuspec
source release.env
for proj in src/*/*.csproj; do dotnet pack "$proj" -c Release --output ./nupkgs/; done
unzip -p ./nupkgs/Beginor.AspNetCore.Middlewares.CustomHeader.$PACKAGE_VERSION.nupkg \
  Beginor.AspNetCore.Middlewares.CustomHeader.nuspec
rm -rf ./nupkgs

# 3. 提交
git add -A && git commit -m "chore: release $PACKAGE_VERSION"
git push origin main

# 4. 打标签并推送（标签名必须等于 PACKAGE_VERSION）
git tag "$PACKAGE_VERSION"
git push origin main "$PACKAGE_VERSION"
```

推送标签后 Actions 自动运行，成功后 nuget.org 上出现新版本，状态 `Validating` 属正常，
索引最长约 1 小时。

## 7. 常见问题

**No matching trust policy**
策略字段与工作流不匹配。重点检查 Workflow File 是否只填了文件名、Repository Owner/Repository
大小写、Environment 是否留空、Glob 是否覆盖包 ID。

**403 / key 过期**
临时 key 只有 1 小时。重跑工作流即可，不要缓存或复用 key。

**版本退化成 1.0.0**
pack 前没有 `source release.env`，`$(PACKAGE_VERSION)` 为空，SDK 回落到默认 `1.0.0`。

**策略显示 Pending**
策略创建后需要至少一次成功的 OIDC 登录才会转为 Active，属正常现象。

**工作流成功但版本没更新**
该版本已存在于 nuget.org，被 `--skip-duplicate` 跳过。检查 `release.env` 是否已递增版本号。
