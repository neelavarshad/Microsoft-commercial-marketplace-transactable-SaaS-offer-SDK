using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Marketplace.SaaS.Accelerator.Services.Helpers;
public class CertificateHelper
{
    private readonly SecretClient _secretClient;
    private readonly string _certificateName;
    private readonly string _certificatePassword;

    public CertificateHelper(string keyVaultUrl, string certificateName, string certificatePassword)
    {
        _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        _certificateName = certificateName;
        _certificatePassword = certificatePassword;
    }

    public async Task<X509Certificate2> GetCertificateAsync()
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(_certificateName);
        byte[] certBytes = Convert.FromBase64String(secret.Value);
        System.Console.WriteLine($"Read the certificate");
        KeyVaultSecret keyvaultCertPassword = _secretClient.GetSecret(_certificatePassword);
        string certificatepassword = keyvaultCertPassword.Value;
        System.Console.WriteLine($"Read the certificate password");

        return new X509Certificate2(certBytes, certificatepassword);
    }

    public X509Certificate2 GetCertificate()
    {
        return GetCertificateAsync().GetAwaiter().GetResult();
    }

    public static string ExtractSecretName(string input)
    {
        System.Console.WriteLine($"Extracting secret name from input: {input}");
        string pattern = @"SecretName=([^;)]+)";
        Match match = Regex.Match(input, pattern);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

}
