using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComponentSpace.SAML2.Assertions;

namespace Portal.Web.Models
{
    public class SamlAttributesViewModel
    {
        public SAMLAttribute[] Attributes { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}