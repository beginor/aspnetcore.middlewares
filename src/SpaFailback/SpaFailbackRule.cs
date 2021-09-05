using System.Text.RegularExpressions;

namespace Beginor.AspNetCore.Middlewares.SpaFailback {

    public class SpaFailbackRule {
        private Regex pathBaseRegex;
        public Regex PathBaseRegex {
            get {
                if (string.IsNullOrEmpty(PathBase)) {
                    return null;
                }
                if (pathBaseRegex == null) {
                    pathBaseRegex = new Regex(PathBase);
                }
                return pathBaseRegex;
            }
        }
        public string PathBase { get; set; }
        public string Failback { get; set; }
    }


}
