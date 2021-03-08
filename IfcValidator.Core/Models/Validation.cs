using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IfcValidator.Core.Models
{
    public class DisplayDomain
    {
        public DisplayDomain(DomainContractV2 domain)
        {
            Domain = domain;
            DisplayName = $"{domain.Name} - {domain.Version}";
        }
        public string DisplayName { get; set; }
        public DomainContractV2 Domain { get; set; }
    }
}
