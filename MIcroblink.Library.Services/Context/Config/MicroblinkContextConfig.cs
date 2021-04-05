using Microblink.Library.Services.Context.Config.Interfaces;

namespace Microblink.Library.Services.Context
{

    public class MicroblinkContextConfig : IMicroblinkContextConfig
    {
        public string AuthorizationScheme { get; set; }
        public string AuthorizationToken { get; set; }
        public string BlinkIdRecognizerEndpoint { get; set; }
    }
}
