using System.Collections.Generic;

namespace Beginor.AspNetCore.Middlewares.SpaFailback;

public class SpaFailbackOptions {
    public List<SpaFailbackRule> Rules { get; set; } = new();
}
