using Microblink.Library.Services.Context.Config.Interfaces;

namespace Microblink.Library.Services.Context
{

    public class DataContextConfig : IDataContextConfig
    {
        public IDatabaseContextConfig DatabaseContextConfig { get; set; }
        public IMicroblinkContextConfig MicroblinkContextConfig { get; set; }
    }
}
