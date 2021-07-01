using Microsoft.IdentityModel.Tokens.Saml2;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Saml2P;
using Sustainsys.Saml2.WebSso;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace StubIdpCore.Model
{
    public class AssertionModel
    {
        private const string DefaultSessionIndex = "42";

        [Required]
        [Display(Name = "Assertion Consumer Service Url")]
        public string AssertionConsumerServiceUrl { get; set; }

        [Display(Name = "Relay State")]
        [StringLength(80)]
        public string RelayState { get; set; }

        [Display(Name = "Subject NameId")]
        [Required]
        public string NameId { get; set; }

        [Display(Name = "Audience")]
        public string Audience { get; set; }

        [Display(Name = "In Response To ID")]
        public string InResponseTo { get; set; }

        public Saml2BindingType ResponseBinding { get; set; } = Saml2BindingType.HttpPost;

        [Display(Name = "Session Index")]
        public string SessionIndex { get; set; }

        /// <summary>
        /// Creates a new Assertion model with place holder value
        /// </summary>
        /// <returns>An <see cref="AssertionModel"/></returns>
        public static AssertionModel CreateFromConfiguration()
        {
            return new AssertionModel
            {
                AssertionConsumerServiceUrl = "https://sp.example.com/SAML2/Acs",
                NameId = "JohnDoe",
                SessionIndex = DefaultSessionIndex
            };
        }

        private static IEnumerable<string> YieldIfNotNullOrEmpty(string src)
        {
            if (!string.IsNullOrEmpty(src))
            {
                yield return src;
            }
        }

        public Saml2Response ToSaml2Response(string entityId)
        {
            var nameIdClaim = new Claim(ClaimTypes.NameIdentifier, NameId);
            nameIdClaim.Properties[ClaimProperties.SamlNameIdentifierFormat] =
                NameIdFormat.Unspecified.GetUri().AbsoluteUri;
            var claims =
                new Claim[] { nameIdClaim }
                .Concat(YieldIfNotNullOrEmpty(SessionIndex).Select(
                    s => new Claim(Saml2ClaimTypes.SessionIndex, SessionIndex)));
            var identity = new ClaimsIdentity(claims);

            Saml2Id saml2Id = null;
            if (!string.IsNullOrEmpty(InResponseTo))
            {
                saml2Id = new Saml2Id(InResponseTo);
            }

            var audienceUrl = string.IsNullOrEmpty(Audience)
                ? null
                : new Uri(Audience);

            return new Saml2Response(
                new EntityId(entityId),
                CertificateHelper.SigningCertificate, new Uri(AssertionConsumerServiceUrl),
                saml2Id, RelayState, audienceUrl, identity);
        }
    }
}
