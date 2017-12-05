using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Threading.Tasks;

namespace KeyVaultManager
{
    public class SecretManager
    {
        public async Task<string> GetVaultSecret(string vaultName, string secretName)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            try
            {
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                var secret = await keyVaultClient.GetSecretAsync($"https://{vaultName}.vault.azure.net/secrets/{secretName}")
                    .ConfigureAwait(false);

                return secret.Value;

            }
            catch
            {

                //TODO error

            }

            return null;
        }
    }
}
