using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {

        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CertValidationCallback);

        bool coneectionintercepted = CheckConnectionInterception();
        if (coneectionintercepted)
        {
            Console.WriteLine("Se ha detectado una interceptacion");
        }

        else {
            Console.Write("Digite pass: ");
            string pass = Console.ReadLine();
            System.Net.WebClient client = new System.Net.WebClient();
            string url = client.DownloadString("https://pastebin.com/raw/ycqe5Xzh");
            if (url.Contains("true"))
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }

        Console.ReadKey();

       
  
    }


    public static bool CertValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {

        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            return false;
        }


        if (chain.ChainPolicy.VerificationFlags == X509VerificationFlags.NoFlag &&
            chain.ChainPolicy.RevocationMode == X509RevocationMode.Online)
        {
            return true;
        }


        X509Chain newChain = new X509Chain();
        X509ChainElementCollection chainElements = chain.ChainElements;
        for (int i = 1; i < chainElements.Count - 1; i++)
        {
            newChain.ChainPolicy.ExtraStore.Add(chainElements[i].Certificate);
        }

        return newChain.Build(chainElements[0].Certificate);
    }


    static bool CheckConnectionInterception()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = client.GetAsync("https://www.google.com").Result;


                return response.IsSuccessStatusCode == false;
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine($"An error occurred: {ex.Message}");
            return true;
        }
    }
}
