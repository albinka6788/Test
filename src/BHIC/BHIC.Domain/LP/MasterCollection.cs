using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BHIC.Domain.LP
{
    public class MasterCollection
    {
        public MasterCollection()
        {
            Lob = new List<SelectListItem>();
            State = new List<SelectListItem>();
            Logo = new List<SelectListItem>();
            MainImage = new List<SelectListItem>();
            Templates = new List<SelectListItem>();
            CTAMessages = new List<CTAMessage>();
        }
        public List<SelectListItem> Lob { get; set; }
        public List<SelectListItem> State { get; set; }
        public List<SelectListItem> Logo { get; set; }
        public List<SelectListItem> Header { get; set; }
        public List<SelectListItem> Footer { get; set; }
        public List<SelectListItem> MainImage { get; set; }
        public List<SelectListItem> Templates { get; set; }
        public List<CTAMessage> CTAMessages { get; set; }
    }
}
