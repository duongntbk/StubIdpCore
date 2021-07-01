using Sustainsys.Saml2.Metadata;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace StubIdpCore
{
    public class CertificateHelper
    {
        public static X509Certificate2 SigningCertificate
        {
            get
            {
                var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var keyPath = Path.Combine(executableLocation, "AppData", "stubidp.sustainsys.com.pfx");
                return new X509Certificate2(keyPath, "", X509KeyStorageFlags.MachineKeySet);
            }
        }

        public static KeyDescriptor SigningKey => CreateKeyDescriptor(SigningCertificate);

        private static KeyDescriptor CreateKeyDescriptor(X509Certificate2 cert)
        {
            var keyDescriptor = new KeyDescriptor();
            keyDescriptor.KeyInfo = new DSigKeyInfo();
            var x509Data = new X509Data();
            x509Data.Certificates.Add(cert);
            keyDescriptor.KeyInfo.Data.Add(x509Data);
            return keyDescriptor;
        }
    }
}
