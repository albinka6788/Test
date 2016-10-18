using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;

namespace BHIC.Portal
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PolicyCode { get; set; }
        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                    return FirstName + " " + LastName;

                if (!string.IsNullOrWhiteSpace(FirstName))
                    return FirstName;

                if (!string.IsNullOrWhiteSpace(LastName))
                    return LastName;

                return "";
            }
        }

        #endregion
    }
}