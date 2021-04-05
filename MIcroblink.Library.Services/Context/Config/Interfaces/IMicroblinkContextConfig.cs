namespace Microblink.Library.Services.Context.Config.Interfaces
{
    /// <summary>
    /// Config for MIcroblink context source
    /// </summary>
    public interface IMicroblinkContextConfig
    {
        string AuthorizationScheme { get; set; }
        string AuthorizationToken { get; set; }
        string BlinkIdRecognizerEndpoint { get; set; }
    }
}
