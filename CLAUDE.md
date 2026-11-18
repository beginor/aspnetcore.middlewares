# CLAUDE.md

ASP.NET Core 中间件集合（CustomHeader / GzipStatic / SpaFailback），以 `Beginor.AspNetCore.Middlewares.*` 发布到 nuget.org。

## 构建

```bash
dotnet build AspNetCore.Middlewares.slnx
```

## 发布

1. 修改 `release.env` 中的 `PACKAGE_VERSION` 与 `PACKAGE_RELEASE_NOTES`（版本必须大于 nuget.org 已有版本）。
2. `source release.env` 后逐个 `dotnet pack src/*/*.csproj -c Release --output ./nupkgs/` 本地验证 nuspec。
3. 提交后 `git tag $PACKAGE_VERSION && git push origin main $PACKAGE_VERSION`，由 Actions 通过 OIDC 发布。

详见 [docs/publish-nuget.md](docs/publish-nuget.md)。
