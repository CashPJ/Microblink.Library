using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Context.Interfaces
{
    /// <summary>
    /// Microblink context
    /// </summary>
    public interface IMicroblinkContext
    {
        /// <summary>
        /// OCR users document data with BlinkId service
        /// </summary>
        /// <param name="idCardBase64Image"></param>
        Task<IModelContainer<IUser>> OcrData(string idCardBase64Image);
    }
}
