using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common.Client;

namespace BHIC.Contract.Provider
{
    public class IServiceProviders
    {
        /// <summary>
        /// Set service provider into property taht can be inherited into all implmenetation classes
        /// </summary>
        /// <param name="provider"></param>
        public IServiceProviders(ServiceProvider provider = null) 
        {
            ServiceProvider = provider;
        }

        public ServiceProvider ServiceProvider { get; set; }
    }
}
