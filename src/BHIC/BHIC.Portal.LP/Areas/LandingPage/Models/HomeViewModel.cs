using BHIC.Domain.LP;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BHIC.Portal.LP.Areas.LandingPage.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            landingPageTransactions = new List<LandingPageTransaction>();
        }
        
        public LandingPageTransaction landingPageTransaction { get; set; }
        public MasterCollection masterCollection { get; set; }
        public List<LandingPageTransaction> landingPageTransactions { get; set; }
        public IPagedList<LandingPageTransaction> landingPageTransactionlist { get; set; }

        public string filter { get; set; }
        public string SearchFilter { get; set; }
        public int PageIndex { get; set; }
        public int SelectedUser { get; set; }
    }
}