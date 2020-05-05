using JuliusCaesarCryptographyChallengeCodenation.Models;
using JuliusCaesarCryptographyChallengeCodenation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace JuliusCaesarCryptographyChallengeCodenation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChallengeMessageController : ControllerBase
    {
        [HttpGet]
        public string GetEncrypted()
        {
            var request = WebRequest.CreateHttp("https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=3b87f90669afa2dc83e82e1b3ae7be47a40f24b8");
            request.Method = "GET";

            using var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream ?? throw new InvalidOperationException());
            var result = reader.ReadToEnd();

            stream.Close();
            response.Close();

            return result;
        }

        [HttpPost]
        public EnigmaModel PostDecrypted()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var enigmaJson = GetEncrypted();
            var enigmaModel = JsonSerializer.Deserialize<EnigmaModel>(enigmaJson, options);

            // java is, in many ways, c++??. unknown

            var enigmaSolved = new MessageDecrypter().Decripter(enigmaModel);

            return enigmaSolved;
        }

        [HttpPost("dynamic")]
        public EnigmaModel PostDynamicDecrypted([FromBody] DynamicModel model)
        {
            if (string.IsNullOrEmpty(model.UrlRequest))
                model.UrlRequest = "https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=";

            if (string.IsNullOrEmpty(model.Token))
                model.Token = "3b87f90669afa2dc83e82e1b3ae7be47a40f24b8";

            if (string.IsNullOrEmpty(model.UrlResponse))
                model.UrlResponse = "https://api.codenation.dev/v1/challenge/dev-ps/submit-solution?token=";

            var requestUrl = WebRequest.CreateHttp(model.UrlRequest + model.Token);
            requestUrl.Method = "GET";

            using var response = requestUrl.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream ?? throw new InvalidOperationException());
            var result = reader.ReadToEnd();

            stream.Close();
            response.Close();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var enigmaModel = JsonSerializer.Deserialize<EnigmaModel>(result, options);

            var enigmaSolved = new MessageDecrypter().Decripter(enigmaModel);

            var responseUrl = WebRequest.CreateHttp(model.UrlResponse + model.Token);
            responseUrl.Method = "POST";



            return enigmaSolved;
        }
    }
}
