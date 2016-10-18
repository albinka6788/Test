using BHIC.Domain.CommercialAuto;
using BHIC.ViewDomain;
using System.Collections.Generic;

namespace BHIC.ViewDomain.Landing
{
    public class NavigationModel
    {
        public string DisplayText { get; set; }
        public string ClassName { get; set; }
        public string NavigationLink { get; set; }
        public string Href { get; set; }
    }
}
