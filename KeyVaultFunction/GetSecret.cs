using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using System;
using KeyVaultManager;

namespace KeyVaultFunction
{
    public static class GetSecret
    {
        [FunctionName("GetSecret")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"GetSecret being triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            if (data.vaultName == null || data.secretName == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = "Please pass vaultName/secretName properties in the input object"
                });
            }

            //reference library that interacts with azure key vault
            var kvm = new SecretManager();
            var secretValue = await kvm.GetVaultSecret(data.vaultName.ToString(), data.secretName.ToString());

            if (secretValue != null)
            {
                return req.CreateResponse(HttpStatusCode.OK, new
                {
                    SecretName = data.secretName,
                    SecretValue = secretValue,
                });
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    Message = "Could not get the secret from the key vault."
                });
            }
        }
    }
}
