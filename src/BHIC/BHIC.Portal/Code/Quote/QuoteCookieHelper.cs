using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BHIC.Portal.Code.Quote
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

        public static void Cookie_SaveTestCookie(HttpContextBase context)
        {
            Helper_CookieSet(context, "Ds007", "Cookies Enabled");
        }

        public static bool Cookie_GetTestCookie(HttpContextBase context)
        {
            // variables / initializatio
            string cookieName = "Ds007";

            // get the current session id
            string sessionId = context.Session.SessionID;

            // get the cookie, return the value
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId];
                if (existingCookie.Value == "Cookies Enabled")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // no cookie?  return false
            else
            {
                return false;
            }
        }

        // ----------------------------------------
        // quote id cookie helpers
        // ----------------------------------------

        public static void Cookie_SaveQuoteId(HttpContextBase context, int cookieValue)
        {
            Helper_CookieSet(context, "Ds006", cookieValue.ToString());
        }

        public static int Cookie_GetQuoteId(HttpContextBase context)
        {
            // variables / initializatio
            string cookieName = "Ds006";

            // get the current session id
            string sessionId = context.Session.SessionID;

            // get the cookie, return the value
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId];
                try
                {
                    return int.Parse(existingCookie.Value);
                }
                catch
                {
                    return 0;
                }
            }
            // no cookie?  return zero
            else
            {
                return 0;
            }
        }

        public static void Cookie_DeleteQuoteId(HttpContextBase context)
        {
            Helper_CookieDelete(context, "Ds006", false);
        }

        // ----------------------------------------
        // generic cookie helpers
        // ----------------------------------------

        private static void Helper_CookieSet(HttpContextBase context, string cookieName, string cookieValue)
        {

            // get the current session id
            string sessionId = context.Session.SessionID;

            // if we already have the cookie associated with the current session, grab it and update the value
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId];
                existingCookie.Value = cookieValue.ToString();

                // SSL should be enabled on production servers; the following setting can be disabled for testing purposes
                if (ConfigurationManager.AppSettings["RequireSecureCookies"] == "Y")
                {
                    existingCookie.Secure = true;
                    existingCookie.HttpOnly = true;
                }
                existingCookie.Expires = DateTime.Now.AddHours(2);
                context.Response.Cookies.Add(existingCookie);
            }
            // otherwise, save a cookie on the user's machine that uniquely identifies their cookie value and session
            else
            {
                HttpCookie newCookie = new HttpCookie(cookieName + sessionId);
                newCookie.Value = cookieValue.ToString();

                // SSL should be enabled on production servers; the following setting can be disabled for testing purposes
                if (ConfigurationManager.AppSettings["RequireSecureCookies"] == "Y")
                {
                    newCookie.Secure = true;
                    newCookie.HttpOnly = true;
                }
                newCookie.Expires = DateTime.Now.AddHours(2);
                context.Response.Cookies.Add(newCookie);
            }
        }

        private static string Helper_CookieGet(HttpContextBase context, string cookieName)
        {
            // APPROACH: tie the cookie value to the session id and quote id to prevent attackers from easily brute-forcing cookie values during forced-browsing

            // get the current DsQuoteId
            int dsQuoteId = Cookie_GetQuoteId(context);

            // get the current session id
            string sessionId = context.Session.SessionID;

            // if we already have the cookie, grab it; used to establish a relationship to the current Quote within the current session
            if (context.Request.Cookies.AllKeys.Contains(cookieName + sessionId + dsQuoteId.ToString()))
            {
                var existingCookie = context.Request.Cookies[cookieName + sessionId + dsQuoteId.ToString()];
                return existingCookie.Value;
            }
            // no cookie?  return zero
            else
            {
                return "";
            }
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
                if (ConfigurationManager.AppSettings["RequireSecureCookies"] == "Y")
                {
                    existingCookie.Secure = true;
                    existingCookie.HttpOnly = true;
                }

                context.Response.Cookies.Add(existingCookie);
            }
        }
    }
}