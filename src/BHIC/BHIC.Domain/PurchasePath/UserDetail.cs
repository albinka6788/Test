using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.PurchasePath
{
    public class UserDetail
    {
        public string EmailID { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Phone { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }
        //public string FullName
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(LastName))
        //            return FirstName;
        //        else
        //        {
        //            if (string.IsNullOrWhiteSpace(FirstName))
        //                return LastName;
        //            else
        //                return FirstName + " " + LastName;
        //        }
        //    }
        //}
    }

    public class PolicyInfo
    {
        public string PolicyCode { get; set; }
        public string PolicyStatus { get; set; }
        public int PolicyStatusCode
        {
            get
            {
                int code;
                switch (PolicyStatus)
                {
                    case "Active":
                        code = 0;
                        break;
                    case "Active Soon":
                        code = 1;
                        break;
                    case "Pending Cancellation":
                        code = 2;
                        break;
                    case "Expired":
                        code = 3;
                        break;
                    case "Cancelled":
                        code = 4;
                        break;
                    case "No Coverage":
                        code = 5;
                        break;
                    default:
                        code = 6;
                        break;
                }
                return code;
            }
        }
        public int LOB { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<BackEnd.Contact> BackEndContacts { get; set; }
    }
}
