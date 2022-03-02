using System;
using System.Text.RegularExpressions;

namespace Beginor.AspNetCore.Middlewares.SpaFailback;

public class SpaFailbackRule {

    public Regex? PathBaseRegex => string.IsNullOrEmpty(PathBase) ? null : new Regex(PathBase, RegexOptions.IgnoreCase);

    public string PathBase { get; init; } = string.Empty;

    public string Failback { get; init; } = String.Empty;

}
