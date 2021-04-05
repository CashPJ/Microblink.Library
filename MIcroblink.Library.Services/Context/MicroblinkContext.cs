using Microblink.Common.MrzValidator;
using Microblink.Library.Services.Context.Config.Interfaces;
using Microblink.Library.Services.Context.Interfaces;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto;
using Microblink.Library.Services.Models.Dto.Interfaces;
using Microblink.Library.Services.Models.Interfaces;
using Microblink.Library.Services.Models.Microblink;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Services.Context
{
    /// <summary>
    /// Database context
    /// </summary>
    public class MicroblinkContext : IMicroblinkContext
    {
        private readonly IDataContextConfig _config;
        private readonly ILogger<MicroblinkContext> _logger;

        public MicroblinkContext(ILogger<MicroblinkContext> logger, IDataContextConfig config)
        {
            _config = config;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IModelContainer<IUser>> OcrData(string idCardBase64Image)
        {
            var json = "{\"imageSource\" : \""+ idCardBase64Image + "\"}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_config.MicroblinkContextConfig.AuthorizationScheme, _config.MicroblinkContextConfig.AuthorizationToken);
                using (var jsonContent = new StringContent(json))
                {
                    jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    using (var response = await client.PostAsync(_config.MicroblinkContextConfig.BlinkIdRecognizerEndpoint, jsonContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var responseModel = JsonConvert.DeserializeObject<BlinkIdResponseModel>(content);

                            if (responseModel.SuccessfullyParsed)
                            {
                                //validating mzr captured data
                                var isValidMrz = new MrzTD1Validator(responseModel.Result.MrzData.RawMrzString).IsValidMrz();

                                var user = new UserDto
                                {
                                    FirstName = responseModel.Result.FirstName,
                                    LastName = responseModel.Result.LastName,
                                    DateOfBirth = responseModel.Result.DateOfBirth.Date.Value,
                                    IsValidMrz = isValidMrz
                                };

                                return new ModelContainer<IUser>(user);
                            }

                            //some OCR data is missing
                            _logger.LogWarning("Some data is missing during BlinkID OCR");
                            return new ModelContainer<IUser>
                            {
                                ErrorMessage = ModelContainerErrorMessages.Missing_One_Or_More_Obligatory_Fields
                            };
                        }

                        //response was not successfull
                        _logger.LogWarning($"OcrData BlinkId response returned: {(int)response.StatusCode} {response.StatusCode}");
                        return new ModelContainer<IUser>
                        {
                            ErrorMessage = ModelContainerErrorMessages.Unprocessable_Entity
                        };
                    }
                }
            }
        }

        private StringContent GetStringContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, UnicodeEncoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}
