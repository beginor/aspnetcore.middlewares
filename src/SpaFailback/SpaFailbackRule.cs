using System;
using System.Text.RegularExpressions;

namespace Beginor.AspNetCore.Middlewares.SpaFailback; 

public class SpaFailbackRule {

    private Regex? pathBaseRegex;

    public Regex PathBaseRegex {
        get {
            if (string.IsNullOrEmpty(PathBase)) {
                throw new InvalidOperationException($"{nameof(PathBase)} is empty!");
            }
            return pathBaseRegex ??= new Regex(PathBase);
        }
    }
    public string PathBase { get; init; } = string.Empty;

    public string Failback { get; init; } = String.Empty;
}