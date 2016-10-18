using BHIC.Common.Client;
using BHIC.Domain.Dashboard;
using BHIC.Domain.PurchasePath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.PurchasePath
{
    public interface IUserInfo
    {
        UserDetail GetUserInfo(UserRegistration user, ServiceProvider guardServiceProvider);
    }
}
