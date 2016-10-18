using System;
using System.Linq;
using System.Web;
using BHIC.Common.XmlHelper;

namespace BHIC.Common.Quote
{
    public static class QuoteCookieHelper
    {

        // ----------------------------------------
        // cookie test helpers (used to confirm that cookies are enabled on the user's machine)
        // ----------------------------------------
        // Use Cases: if a user requests a page where a cookie is expected, but none is found, it could mean that:
        //	a) The user's device doesn't support cookies, or
        //	b) The session used to write the cookie expired, or
        //	c) The user force-browsed to the page, outside an active quote session, via a bookmark (this is really just a specific instance of b) above)
        //
        // Business Rules:
        // a) if the user if a user requests a page where a cookie is expected, but none is found:
        //		- write a test cookie
        //		- redirect to an action that tries to read the test cookie
        //		- if the test cookie is found, cookies are enabled; redirect to the start page.
        //		- if the test cookie is not found, redirect to a page that advises the user to enable cookies.

        // ----------------------------------------
        // quote id cookie helpers
        // ----------------------------------------

        public static void Cookie_SaveQuoteId(HttpContextBase context, int cookieValue)
        {
            BHIC.ViewDomain.CustomSession appSession = null;

            if (!context.Session["CustomSession"].IsNull())
            {
                appSession = (BHIC.ViewDomain.CustomSession)context.Session["CustomSession"];

                if (appSession.QuoteID >0)
                {
                    appSession.QuoteID = Convert.ToInt32(cookieValue);
                }
                //if (!appSession.QuoteVM.IsNull())
                //{
                //    appSession.QuoteVM.QuoteId = cookieValue.ToString();
                //}
            }

            #region Old Commented Code

            //Helper_CookieSet(context, "Ds006", cookieValue.ToString(), false);
            ////Helper_CookieSet(context, "Ds007", cookieValue.ToString());

            #endregion
        }

        public static int Cookie_GetQuoteId(HttpContextBase context)
        {
            BHIC.ViewDomain.CustomSession appSession = null;

            if (!context.Session["CustomSession"].IsNull())
            {
                appSession = (BHIC.ViewDomain.CustomSession)context.Session["CustomSession"];
            }

            return (!appSession.IsNull() && appSession.QuoteID > 0) ? appSession.QuoteID : 0;    
            //return (!appSession.IsNull() && !appSession.QuoteVM.IsNull()) ? Convert.ToInt32(appSession.QuoteVM.QuoteId) : 0;  //Old Line

            #region Old Commented Code

            ////int encryptedQuoteID;
            //int quoteID;
            //int.TryParse(Helper_CookieGet(context, "Ds006", false), out quoteID);
            ////int.TryParse(Helper_CookieGet(context, "Ds007"), out encryptedQuoteID);

            ////if (encryptedQuoteID != quoteID)
            ////{
            ////    LogCookie(context, "Quote ID", quoteID.ToString(), encryptedQuoteID.ToString());
            ////}

            ////return ((encryptedQuoteID != quoteID) ? 0 : encryptedQuoteID);
            //return quoteID;

            #endregion
        }

        public static void Cookie_DeleteQuoteId(HttpContextBase context)
        {
            Helper_CookieDelete(context, "Ds006", false);
            //Helper_CookieDelete(context, "Ds007", false);
            LogCookie(context, "Quote ID", isCookieDeleted: true);
        }

        // ----------------------------------------
        // Policy Center User cookie helpers
        // ----------------------------------------

        public static void Cookie_SavePcUserId(HttpContextBase context, string cookieValue)
        {
            Helper_CookieSet(context, "Ds008", cookieValue, false);
            //Helper_CookieSet(context, "Ds009", cookieValue);
        }

        public static string Cookie_GetPcUserId(HttpContextBase context)
        {
            return Helper_CookieGet(context, "Ds008", false);
            //string userID = Helper_CookieGet(context, "Ds008", false);
            //string encryptedUserID = Helper_CookieGet(context, "Ds009");

            //if (!encryptedUserID.Equals(userID))
            //{
            //    LogCookie(context, "User ID", userID, encryptedUserID);
            //}

            //return (encryptedUserID.Equals(userID) ? encryptedUserID : string.Empty);
        }

        public static void Cookie_DeletePcUserId(HttpContextBase context)
        {
            Helper_CookieDelete(context, "Ds008", false);
            //Helper_CookieDelete(context, "Ds009", false);
            LogCookie(context, "User ID", isCookieDeleted: true);
        }

        // ----------------------------------------
        // Token id cookie helpers
        // ----------------------------------------
        public static void Cookie_SaveTokenId(HttpContextBase context, string cookieValue)
        {
            Helper_CookieSet(context, "Ds004", cookieValue, false);
            //Helper_CookieSet(context, "Ds005", cookieValue);
        }

        public static string Cookie_GetTokenId(HttpContextBase context)
        {
            return Helper_CookieGet(context, "Ds004", false);
            //string TokenID = Helper_CookieGet(context, "Ds004", false);
            //string encryptedTokenID = Helper_CookieGet(context, "Ds005");

            //if (!encryptedTokenID.Equals(TokenID))
            //{
            //    LogCookie(context, "Token ID", TokenID, encryptedTokenID);
            //}

            //return (encryptedTokenID.Equals(TokenID) ? encryptedTokenID : string.Empty);
        }

        public static void Cookie_DeleteTokenId(HttpContextBase context)
        {
            Helper_CookieDelete(context, "Ds004", false);
            //Helper_CookieDelete(context, "Ds005", false);
            LogCookie(context, "Token ID", isCookieDeleted: true);
        }
        //public static void Cookie_SaveTokenId(HttpContextBase context, string cookieValue)
        //{
        //    Helper_CookieSet(context, "Ds004", cookieValue, false);
        //    Helper_CookieSet(context, "Ds005", cookieValue);
        //}

        //public static string Cookie_GetTokenId(HttpContextBase context)
        //{
        //    string encryptedTokenId = Helper_CookieGet(context, "Ds005");
        //    string tokenId = Helper_CookieGet(context, "Ds004", false);


        //    if (!encryptedTokenId.Equals(tokenId))
        //    {
        //        LogCookie(context, "Token ID", tokenId, encryptedTokenId);
        //    }

        //    return (encryptedTokenId.Equals(tokenId) ? encryptedTokenId : string.Empty);

        //}

        //public static void Cookie_DeleteTokenId(HttpContextBase context)
        //{
        //    Helper_CookieDelete(context, "Ds004", false);
        //    Helper_CookieDelete(context, "Ds005", false);
        //    LogCookie(context, "Token ID", isCookieDeleted: true);
        //}

        // ----------------------------------------
        // generic cookie helpers
        // ----------------------------------------

        private static void Helper_CookieSet(HttpContextBase context, string cookieName, string cookieValue, bool isValueEncrypt = true)
        {

            // get the current session id
            string sessionId = context.Session.SessionID;
            HttpCookie cybCookie = null;

            // if we already have the cookie associated with the current session, grab it and update the value
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId))
            {
                cybCookie = context.Request.Cookies[cookieName + sessionId];
            }
            // otherwise, save a cookie on the user's machine that uniquely identifies their cookie value and session
            else
            {
                cybCookie = new HttpCookie(cookieName + sessionId);
            }

            cybCookie.Value = (isValueEncrypt ? Encryption.EncryptText(cookieValue.ToString(), sessionId) : cookieValue.ToString());

            // SSL should be enabled on production servers; the following setting can be disabled for testing purposes
            if (ConfigCommonKeyReader.RequireSecureCookies)
            {
                cybCookie.Secure = true;
                cybCookie.HttpOnly = true;
            }
            cybCookie.Expires = DateTime.Now.AddMinutes(context.Session.Timeout);
            context.Response.Cookies.Add(cybCookie);
        }

        private static string Helper_CookieGet(HttpContextBase context, string cookieName, bool isValueEncrypt = true)
        {
            // get the current session id
            string sessionId = context.Session.SessionID;

            // if we already have the cookie, grab it; used to establish a relationship to the current Quote within the current session
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId];
                return (isValueEncrypt ? Encryption.DecryptText(existingCookie.Value, sessionId) : existingCookie.Value);
            }
            // no cookie?  return zero
            else
            {
                return "";
            }
        }

        private static bool Helper_CookieExist(HttpContextBase context, string cookieName)
        {
            // get the current session id
            string sessionId = context.Session.SessionID;

            // if we already have the cookie, grab it; used to establish a relationship to the current Quote within the current session
            return (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId));
        }

        private static void Helper_CookieDelete(HttpContextBase context, string cookieName, bool? appendQuoteId = true)
        {
            // APPROACH: tie the cookie value to the session id and quote id to prevent attackers from easily brute-forcing cookie values during forced-browsing

            // get the current DsQuoteId (unless we're working with the QuoteId, which won't have the DsQuoteId embedded)
            string dsQuoteIdString = "";
            if (appendQuoteId == true)
            {
                int dsQuoteId = Cookie_GetQuoteId(context);
                dsQuoteIdString = dsQuoteId.ToString();
            }

            // get the current session id
            string sessionId = context.Session.SessionID;

            // if we have the cookie associated with the current session, grab it and expire it
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId + dsQuoteIdString))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId + dsQuoteIdString];
                existingCookie.Expires = DateTime.Now.AddDays(-1d);

                // SSL should be enabled on production servers; the following setting can be disabled for testing purposes
                if (ConfigCommonKeyReader.RequireSecureCookies)
                {
                    existingCookie.Secure = true;
                    existingCookie.HttpOnly = true;
                }

                context.Response.Cookies.Add(existingCookie);
            }
        }

        /// <summary>
        /// Log cookie information
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cookieName"></param>
        /// <param name="normalValue"></param>
        /// <param name="encryptedValue"></param>
        /// <param name="isCookieDeleted"></param>
        private static void LogCookie(HttpContextBase context, string cookieName, string cookieValue = "", string cookieValueDecrpyted = "",
            bool isCookieDeleted = false)
        {
            string visitorsIPAddr = UtilityFunctions.GetUserIPAddress(context.ApplicationInstance.Context);

            Logging.LoggingService.Instance.Trace(string.Format("{0} Cookie having {1} {2}{3} using URL '{4}' and request received from IP '{5}'",
                cookieName, (!string.IsNullOrEmpty(cookieValue) ? string.Format("normal value '{0}'", cookieValue) : string.Empty),
                (!string.IsNullOrEmpty(cookieValueDecrpyted) ? string.Format("and decrypted value '{0}'", cookieValueDecrpyted) : string.Empty),
                (isCookieDeleted ? " is deleted" : string.Empty), context.Request.Url.ToString(), visitorsIPAddr));
        }



    }
}