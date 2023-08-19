using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Beginor.AspNetCore.Middlewares.CustomHeader;

public class CustomHeaderRule {

    public CustomHeaderRule(string pattern, Dictionary<string, string> headers) {
        if (string.IsNullOrEmpty(pattern)) {
            throw new ArgumentNullException(nameof(pattern), "Custom header pattern can not be empty!");
        }
        Pattern = new Regex(pattern, RegexOptions.IgnoreCase);
        Headers = headers ?? throw new ArgumentNullException(nameof(headers), "Custom headers can not be empty!");
    }

    public Regex Pattern { get; }

    public Dictionary<string, string> Headers { get; }

}
