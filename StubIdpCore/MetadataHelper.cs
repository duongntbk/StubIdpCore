using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.WebSso;
using System;

namespace StubIdpCore
{
    public class MetadataHelper
    {
        public static EntityDescriptor CreateIdpMetadata(string idpEntityId, Uri ssoServiceUrl)
        {
            var metadata = new EntityDescriptor()
            {
                EntityId = new EntityId(idpEntityId)
            };

            metadata.CacheDuration = new XsdDuration(minutes: 15);
            metadata.ValidUntil = DateTime.UtcNow.AddDays(1);

            var idpSsoDescriptor = new IdpSsoDescriptor();
            idpSsoDescriptor.ProtocolsSupported.Add(new Uri("urn:oasis:names:tc:SAML:2.0:protocol"));
            metadata.RoleDescriptors.Add(idpSsoDescriptor);

            idpSsoDescriptor.SingleSignOnServices.Add(new SingleSignOnService()
            {
                Binding = Saml2Binding.HttpRedirectUri,
                Location = ssoServiceUrl
            });
            idpSsoDescriptor.SingleSignOnServices.Add(new SingleSignOnService()
            {
                Binding = Saml2Binding.HttpPostUri,
                Location = ssoServiceUrl
            });

            idpSsoDescriptor.Keys.Add(CertificateHelper.SigningKey);

            return metadata;
        }
    }
}
