using BHIC.Domain.PolicyCentre;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PolicyCentre
{
    public interface IGetUserQuotes
    {
        DataSet UserQuotes(int userID);
        bool DeleteQuote(string quoteNumber,int userId);
       
    }
}
