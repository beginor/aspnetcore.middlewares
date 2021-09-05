using System.Collections.Generic;

namespace Beginor.AspNetCore.Middlewares.CustomHeader {

    public class CustomHeaderOptions {
        public IDictionary<string, string> Headers { get; set; }
    }

}
