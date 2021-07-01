using Microsoft.AspNetCore.Mvc;
using StubIdpCore.Model;
using Sustainsys.Saml2.Metadata;
using System.Security.Cryptography.Xml;

namespace StubIdpCore.Pages
{
    public class AutoModel : LoginPageModel
    {
        public ActionResult OnGetMetadata()
        {
            var content = MetadataHelper.CreateIdpMetadata(UrlResolver.AutoIdpEntityId, UrlResolver.AutoSsoServiceUri)
                .ToXmlString(CertificateHelper.SigningCertificate, SignedXml.XmlDsigRSASHA256Url);

            return Content(content, "application/samlmetadata+xml");
        }

        public ActionResult OnGetLogin()
        {
            Model = AssertionModel.CreateFromConfiguration();

            HandleReceivedAuthenRequest();

            return HandleReceivedAuthenResponse(UrlResolver.AutoIdpEntityId);
        }
    }
}
