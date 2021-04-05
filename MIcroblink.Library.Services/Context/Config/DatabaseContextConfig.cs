using Microblink.Library.Services.Context.Config.Interfaces;

namespace Microblink.Library.Services.Context
{

    public class DatabaseContextConfig : IDatabaseContextConfig
    {
        public string DatabaseConnectionString { get; set; }
    }
}
