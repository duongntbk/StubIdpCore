using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using StubIdpCore.Model;
using Sustainsys.Saml2.Metadata;

namespace StubIdpCore.Pages
{
    public class InteractiveModel : LoginPageModel
    {
        public ActionResult OnGetMetadata()
        {
            var content = MetadataHelper.CreateIdpMetadata(
                UrlResolver.InteractiveIdpEntityId, UrlResolver.InteractiveSsoServiceUri)
                    .ToXmlString(CertificateHelper.SigningCertificate, SignedXml.XmlDsigRSASHA256Url);

            return Content(content, "application/samlmetadata+xml");
        }

        public ActionResult OnGetLogin()
        {
            Model = AssertionModel.CreateFromConfiguration();

            HandleReceivedAuthenRequest();

            return Page();
        }

        public ActionResult OnPostLogin()
        {
            if (ModelState.IsValid)
            {
                return HandleReceivedAuthenResponse(UrlResolver.InteractiveIdpEntityId);
            }

            if (Model == null)
            {
                Model = AssertionModel.CreateFromConfiguration();
            }

            return Page();
        }
    }
}
