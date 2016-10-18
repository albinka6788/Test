using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BHIC.Common.Client;
using BHIC.Common.XmlHelper;
using BHIC.Contract.Background;
using BHIC.Core.Background;
using BHIC.DML.WC.DTO;
using BHIC.Domain.Background;
using MoreLinq;

namespace BHIC.Portal.LP.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Return combined Scheme and Host as single value.
        /// </summary>
        /// <returns></returns>
        /// 
        internal ServiceProvider guardServiceProvider = new GuardServiceProvider() { ServiceCategory = ServiceProviderConstants.GuardServiceCategoryWC };

        protected string GetSchemeAndHostURLPart()
        {
            return string.Concat(this.HttpContext.Request.Url.Scheme, "://", this.HttpContext.Request.Url.Host);
        }


        /// <summary>
        /// Get current assembly version
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            //Version webAssemblyVersion = BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Version;
            //return string.Concat("?v=", webAssemblyVersion.Major, webAssemblyVersion.Minor, webAssemblyVersion.Build, webAssemblyVersion.MinorRevision);
            return string.Concat("_", ConfigCommonKeyReader.CdnVersion);
        }

        /// <summary>
        /// Get request base complete url path details
        /// </summary>
        /// <returns></returns>
        protected string Helper_GetBaseUrl()
        {
            string baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            return baseUrl;
        }

        /// <summary>
        /// UserSession : return true if user exists else return false;
        /// </summary>
        /// <returns></returns>
        public bool UserSession()
        {
            bool flag = false;
            try
            {
                if (Session["user"] != null)
                    flag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }

        /// <summary>
        /// Validate zipCode using county service
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        protected bool IsZipExists(string zip)
        {
            return string.IsNullOrWhiteSpace(zip) ? false : GetCountyData(zip).Any();
        }


        /// <summary>
        /// Get All State by zip code
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        protected List<ZipCodeStates> GetAllStateByZip(string zip)
        {
            var states = new List<ZipCodeStates>();
            zip = zip.Trim();

            if (!string.IsNullOrEmpty(zip))
            {
                //fetch all states from county by zip code
                var county = GetCountyData(zip);

                if (county != null && county.Count > 0)
                {
                    states.Add(new ZipCodeStates { ZipCode = county.FirstOrDefault().ZipCode, StateCode = county.FirstOrDefault().State });
                }
            }

            return states;
        }

        /// <summary>
        /// Get County data
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="state"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        private List<County> GetCountyData(string zip = null, string state = null, string city = null, bool isDistinct = true)
        {
            ICountyService countyService = new CountyService(guardServiceProvider);

            //return county list filtered by query parameter
            var countyData = countyService.GetCounty(false);

            List<County> county = new List<County>();

            if (countyData != null && countyData.ToList().Count() > 0)
            {
                List<County> filteredList = new List<County>();
                if (!(string.IsNullOrEmpty(zip) && string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city)))
                {
                    if (!countyData.Any(x => ((string.IsNullOrEmpty(zip) || (!string.IsNullOrEmpty(zip) && x.ZipCode.Equals(zip))) &&
                                    (string.IsNullOrEmpty(state) || (!string.IsNullOrEmpty(state) && x.State.Equals(state, StringComparison.OrdinalIgnoreCase))) &&
                                    (string.IsNullOrEmpty(city) || (!string.IsNullOrEmpty(city) && x.City.Equals(city, StringComparison.OrdinalIgnoreCase))))))
                    {
                        return county;
                    }

                    filteredList = countyData.Where(x => ((string.IsNullOrEmpty(zip) || (!string.IsNullOrEmpty(zip) && x.ZipCode.Equals(zip))) &&
                                (string.IsNullOrEmpty(state) || (!string.IsNullOrEmpty(state) && x.State.Equals(state, StringComparison.OrdinalIgnoreCase))) &&
                                (string.IsNullOrEmpty(city) || (!string.IsNullOrEmpty(city) && x.City.Equals(city, StringComparison.OrdinalIgnoreCase))))).ToList();
                }
                else
                {
                    filteredList = countyData.ToList();
                }

                county = isDistinct ? filteredList.DistinctBy(x => x.State).ToList() : filteredList.ToList();
            }

            return county;
        }
    }
}