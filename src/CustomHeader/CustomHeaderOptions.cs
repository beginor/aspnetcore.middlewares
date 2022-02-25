using System;
using System.Collections.Generic;

namespace Beginor.AspNetCore.Middlewares.CustomHeader {

    public class CustomHeaderOptions {

        public Dictionary<string, string> Headers { get; } = new (StringComparer.OrdinalIgnoreCase);

    }

}
