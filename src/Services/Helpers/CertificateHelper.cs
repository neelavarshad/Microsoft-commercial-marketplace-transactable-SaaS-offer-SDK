using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Security.KeyVault.Certificates;
using System.Security;

namespace Marketplace.SaaS.Accelerator.Services.Helpers;
public class CertificateHelper
{

    private readonly string _certificateName;
    private readonly string _certificatePassword;
    private readonly SecretClient _secretClient;

    public CertificateHelper(string keyVaultUrl, string certificateName, string certificatePassword)
    {
        _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        _certificateName = certificateName;
        _certificatePassword = certificatePassword;
    }

    public async Task<X509Certificate2> GetCertificateAsync()
    {
        
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(_certificateName); // This should be the PFX
        byte[] certBytes = Convert.FromBase64String(secret.Value);
        X509Certificate2 certificate = new X509Certificate2(certBytes, _certificatePassword);

        return certificate;
    }

    public X509Certificate2 GetCertificate()
    {
        return GetCertificateAsync().GetAwaiter().GetResult();
    }

}
