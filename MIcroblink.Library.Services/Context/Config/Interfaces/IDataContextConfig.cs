namespace Microblink.Library.Services.Context.Config.Interfaces
{
    /// <summary>
    /// Configs for multiple context sources
    /// </summary>
    public interface IDataContextConfig
    {
        IDatabaseContextConfig DatabaseContextConfig { get; set; }
        IMicroblinkContextConfig MicroblinkContextConfig { get; set; }
    }
}
