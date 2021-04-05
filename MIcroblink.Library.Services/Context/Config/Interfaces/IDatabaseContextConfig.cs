namespace Microblink.Library.Services.Context.Config.Interfaces
{
    /// <summary>
    /// Config for database context source
    /// </summary>
    public interface IDatabaseContextConfig
    {
        string DatabaseConnectionString { get; set; }
    }
}
