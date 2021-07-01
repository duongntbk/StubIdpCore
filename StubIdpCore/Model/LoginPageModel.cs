using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sustainsys.Saml2.Saml2P;
using Sustainsys.Saml2.WebSso;
using System;

namespace StubIdpCore.Model
{
    [BindProperties]
    public class LoginPageModel : PageModel
    {
        public AssertionModel Model { get; set; }

        protected bool HandleReceivedAuthenRequest()
        {
            var requestData = Request.ToHttpRequestData();
            var binding = Saml2Binding.Get(requestData);
            if (binding != null)
            {
                var extractedMessage = binding.Unbind(requestData, null);

                var request = new Saml2AuthenticationRequest(
                    extractedMessage.Data,
                    extractedMessage.RelayState);

                if (request.AssertionConsumerServiceUrl != null)
                {
                    Model.AssertionConsumerServiceUrl =
                        request.AssertionConsumerServiceUrl.ToString();
                }

                if (new Uri(Model.AssertionConsumerServiceUrl).Scheme == "https")
                {
                    // Chrome will throw error if InResponseTo exists on http response to consumer
                    Model.InResponseTo = request.Id.Value;
                }

                Model.RelayState = extractedMessage.RelayState;
                Model.Audience = request.Issuer.Id;

                return true;
            }
            return false;
        }

        protected ActionResult HandleReceivedAuthenResponse(string entityId)
        {
            var response = Model.ToSaml2Response(entityId);

            return Saml2Binding.Get(Model.ResponseBinding)
                .Bind(response).ToActionResult();
        }
    }
}
