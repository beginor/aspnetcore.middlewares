using System;
using System.Text.RegularExpressions;

namespace Beginor.AspNetCore.Middlewares.SpaFailback;

public class SpaFailbackRule {

    public SpaFailbackRule(string pattern, string failback) {
        if (string.IsNullOrEmpty(pattern)) {
            throw new ArgumentNullException(nameof(pattern), "Failback pattern can not be empty!");
        }
        if (string.IsNullOrEmpty(failback)) {
            throw new ArgumentNullException(nameof(failback), "Failback path can not be empty!");
        }
        Pattern = new Regex(pattern, RegexOptions.IgnoreCase);
        Failback = failback;
    }

    public Regex Pattern { get; }

    public string Failback { get; }

}
