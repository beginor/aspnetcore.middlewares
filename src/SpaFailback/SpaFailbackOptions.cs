using System.Collections.Generic;

namespace Beginor.AspNetCore.Middlewares.SpaFailback {

    public class SpaFailbackOptions {
        public IList<SpaFailbackRule> Rules { get; set; }
    }


}
