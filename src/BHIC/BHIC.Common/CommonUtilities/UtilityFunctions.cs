using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using BHIC.Common.XmlHelper;
using BHIC.Common.Config;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Security;
using System.Web;

namespace BHIC.Common
{
    public static class UtilityFunctions
    {
        #region Methods

        #region Public Methods

        public static string ConvertMessagesToString(List<Message> messageList, string message = "")
        {
            StringBuilder sb = new StringBuilder(message);

            messageList.ForEach(res => sb.AppendLine(res.Text));

            return sb.ToString();
        }

        /// <summary>
        /// Return string concatenated in form of URL request parameters e.g. "api/ServiceName&#63;Param1={Value1}&#38;Param2={Value2}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <param name="allowNull"></param>
        /// <returns>string</returns>
        public static string CreateQueryString<T>(T requestParam, bool allowNull = false)
        {
            StringBuilder sb = new StringBuilder();
            string retStr = string.Empty;

            if (typeof(T) != null && requestParam != null)
            {
                var type = requestParam.GetType();
                var properties = type.GetProperties();

                foreach (var property in properties)
                {
                    //sb.AppendLine(string.Format("Propert Name : {0}, Value : {1}", property.Name, property.GetValue(requestParam, null)));
                    object value = property.GetValue(requestParam, null);                   

                    //Comment : here in case null property value continue execution except allowNull is true
                    if ((value != null) || (value == null && allowNull))
                    {
                        sb.Append(string.Format("{0}={1}&", property.Name, value ?? "null"));
                    }
                    else
                    {
                        continue;
                    }
                }

                //Comment : Here append "?" in beginning and remove last extra "&" charachter
                if (sb.Length > 0)
                {
                    sb.Replace(sb.ToString(), "?" + sb.ToString().TrimEnd('&'));
                }
                else
                    return string.Empty;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes special character if any from the input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str ?? string.Empty, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Removes any non numeric to Character
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNumeric(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
                return Regex.Replace(RemoveSpecialCharacters(str ?? string.Empty), "[^0-9]", "", RegexOptions.Compiled);
            else return String.Empty;
        }

        /// <summary>
        /// Checks for Nulls for Reference Types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return (object)obj == null;
        }

        /// <summary>
        /// Checks for Nulls for Value Types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T? obj) where T : struct
        {
            return !obj.HasValue;
        }

        /// <summary>
        /// Checks if two lists are same
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool CompareList<T>(this List<T> list1, List<T> list2)
        {
            //if any of the list is null, return false
            if ((list1 == null && list2 != null) || (list2 == null && list1 != null))
                return false;
            //if both lists are null, return true, since its same
            else if (list1 == null && list2 == null)
                return true;
            //if count don't match between 2 lists, then return false
            if (list1.Count != list2.Count)
                return false;
            bool IsEqual = true;
            foreach (T item in list1)
            {
                T Object1 = item;
                T Object2 = list2.ElementAt(list1.IndexOf(item));
                Type type = typeof(T);
                //if any of the object inside list is null and other list has some value for the same object  then return false
                if ((Object1 == null && Object2 != null) || (Object2 == null && Object1 != null))
                {
                    IsEqual = false;
                    break;
                }

                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    if (property.Name != "ExtensionData")
                    {
                        string Object1Value = string.Empty;
                        string Object2Value = string.Empty;
                        if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                            Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                        if (type.GetProperty(property.Name).GetValue(Object2, null) != null)
                            Object2Value = type.GetProperty(property.Name).GetValue(Object2, null).ToString();
                        //if any of the property value inside an object in the list didnt match, return false
                        if (Object1Value.Trim() != Object2Value.Trim())
                        {
                            IsEqual = false;
                            break;
                        }
                    }
                }
            }
            //if all the properties are same then return true
            return IsEqual;
        }

        /// <summary>
        /// Checks if the value is a valid integer value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidInt(int? val)
        {
            return !val.IsNull() && val != 0;
        }

        /// <summary>
        /// Removes element matching condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExceptWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return source.Where(x => !predicate(x));
        }
        /// <summary>
        /// Hhelper method will prepare populated content string based on supplied content-template and model-values
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string FillTemplateWithModelValues(string contents, object model)
        {
            try
            {
                if (model != null)
                {
                    var propDictionary = model.GetType().GetProperties().Where(w => w.GetGetMethod() != null).Select(s => new { Name = s.Name, Value = s.GetGetMethod().Invoke(model, null) });

                    foreach (var prop in propDictionary)
                    {
                        contents = contents.Replace("##" + prop.Name + "##", (prop.Value != null) ? prop.Value.ToString() : "");	// mjl - adjust to return an empty string if the property value is null
                    }
                }
            }
            catch { }

            return contents;
        }

        public static bool ValidateCaptcha(string captchaResponse)
        {
            bool Valid = false;
            string key = ConfigCommonKeyReader.CaptchaSecretKey;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + key + "&response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        dynamic obj = JsonConvert.DeserializeObject(jsonResponse, typeof(object));
                        Valid = obj.success.Value;
                    }
                }
                return Valid;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method will check null, trim spaces and check content existance 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool HasValidString(this string content)
        {
            return (content != null && content.Trim().Length > 0) ? true : false;
        }

        public static string GetCompleteMessage(string serviceName, string request, string response, DateTime startTime, DateTime endTime, bool isBatchRequest = false)
        {
            StringBuilder sb = new StringBuilder();
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            string QuoteIDDetail = string.Empty;

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CustomSession"] != null &&
                ((BHIC.ViewDomain.CustomSession)HttpContext.Current.Session["CustomSession"]).QuoteID > 0)
            {
                QuoteIDDetail = string.Format("Executed API Call for Quote Number = '{0}'{1}", ((BHIC.ViewDomain.CustomSession)HttpContext.Current.Session["CustomSession"]).QuoteID, Environment.NewLine);
            }

            sb.Append(string.Concat(QuoteIDDetail, "--- Service Request Log Start ---", Environment.NewLine,
                (isBatchRequest ? "--- Batch Request ---" : "--- Service Name ---"), Environment.NewLine, serviceName,
                Environment.NewLine, Environment.NewLine, (string.IsNullOrEmpty(request) ? string.Empty : "--- Request Message ---"),
                (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine), request,
                (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine), (string.IsNullOrEmpty(request) ? string.Empty : Environment.NewLine),
                "--- Response Received ---", Environment.NewLine, response,
                Environment.NewLine, Environment.NewLine, "Start Date & Time (EST) : ", TimeZoneInfo.ConvertTime(startTime, timeZoneInfo).ToString("F"),
                Environment.NewLine, "End Date & Time (EST) : ", TimeZoneInfo.ConvertTime(endTime, timeZoneInfo).ToString("F"),
                Environment.NewLine, "Total Time taken (ms) : ", (endTime - startTime).TotalMilliseconds,
                Environment.NewLine, "--- Service Request Log End ---", Environment.NewLine, Environment.NewLine));

            return sb.ToString();
        }

        /// <summary>
        /// Get list of string values based on supplied text and seprator
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seprator"></param>
        /// <returns></returns>
        public static List<string> GetListOfSplitedValues(string text, char seprator = Constants.DefaultMailIdsSeprator)
        {
            return !string.IsNullOrEmpty(text.Trim()) && !string.IsNullOrEmpty(seprator.ToString().Trim()) ? text.Split(seprator).ToList() : new List<string>();
        }

        /// <summary>
        /// It will validate string using different regex expression
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsValidRegex(string input, string regex)
        {
            return Regex.Match(input, regex, RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// It will validate string using different regex expression
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string input, string regex)
        {
            return Regex.Match(input, regex).Success;
        }

        /// <summary>
        /// Fetch result from json string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Return true if jsonResult contains valid value, false otherwise</returns>
        public static bool ValidateInputUsingJsonResult(JsonResult input)
        {
            string jsonText = new JavaScriptSerializer().Serialize(input.Data);

            Dictionary<string, string> deserializedObj = (Dictionary<string, string>)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText, typeof(Dictionary<string, string>));

            if (deserializedObj.FirstOrDefault().Value.Equals("True"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Fetch result from json(string format)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Return true if jsonResult contains valid value, false otherwise</returns>
        public static bool GetJsonResult(string input)
        {
            Dictionary<string, string> deserializedObj = (Dictionary<string, string>)JsonConvert.DeserializeObject(input, typeof(Dictionary<string, string>));

            return (deserializedObj.FirstOrDefault().Value.Equals("True", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// create secure string for confidential variables
        /// </summary>
        /// <param name="strPassword"></param>
        /// <returns>Secure string</returns>
        public static SecureString ConvertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }


        /// <summary>
        /// Fetch data from secure string
        /// </summary>
        /// <param name="secstrPassword">Secure String</param>
        /// <returns>String</returns>
        public static string ConvertToString(SecureString secstrPassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        /// <summary>
        /// Match two secure strings
        /// </summary>
        /// <param name="secureString1">Secure String 1</param>
        /// <param name="secureString2">Secure String 2</param>
        /// <returns>bool</returns>
        public static Boolean SecureStringCompare(SecureString secureString1, SecureString secureString2)
        {
            if (secureString1 == null)
            {
                throw new ArgumentNullException("s1");
            }
            if (secureString2 == null)
            {
                throw new ArgumentNullException("s2");
            }

            if (secureString1.Length != secureString2.Length)
            {
                return false;
            }

            IntPtr ss_bstr1_ptr = IntPtr.Zero;
            IntPtr ss_bstr2_ptr = IntPtr.Zero;

            try
            {
                ss_bstr1_ptr = Marshal.SecureStringToBSTR(secureString1);
                ss_bstr2_ptr = Marshal.SecureStringToBSTR(secureString2);
                return Marshal.PtrToStringBSTR(ss_bstr1_ptr).Equals(Marshal.PtrToStringBSTR(ss_bstr2_ptr));
            }
            finally
            {
                if (ss_bstr1_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr1_ptr);
                }

                if (ss_bstr2_ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ss_bstr2_ptr);
                }
            }
        }

        public static DateTime ConvertToDate(string requestedDate, string format = "MM/dd/yyyy")
        {
            DateTime convertedDate;
            if (DateTime.TryParse(requestedDate, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.AssumeLocal, out convertedDate))
            {
                //return DateTime.ParseExact(requestedDate, format, System.Globalization.CultureInfo.InvariantCulture);
                return convertedDate;
            }
            else
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// Read and return string data from specified TXT file 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadTxtData(string filePath)
        {
            #region Comment : Here get local stored data from specific TEXT file

            //Comment : Here specified file exists then only call read-text method
            return (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) ? File.ReadAllText(filePath) : string.Empty;

            #endregion
        }

        /// <summary>
        /// Write text into specified TXT file 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        public static void WriteTxtData(string filePath, string text)
        {
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(text))
            {
                #region Comment : Here write data into specific file

                #region Comment : Here if directory not exist add file to folder to server location

                if (Path.GetDirectoryName(filePath).Length > 0 && !Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }

                #endregion

                #region Comment : Here specified file exists then only call read-text method

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(text);
                }

                #endregion

                #endregion
            }
        }

        /// <summary>
        /// Get user IP address from current http request context
        /// </summary>
        /// <param name="currentRequest"></param>
        /// <returns></returns>
        public static string GetUserIPAddress(HttpContext currentContext)
        {
            string VisitorsIPAddress = string.Empty;
            HttpRequest currentRequest = currentContext.Request;

            if (currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Last();
            }
            else if (currentRequest.ServerVariables["REMOTE_ADDR"] != null)
            {
                VisitorsIPAddress = currentRequest.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else if (currentRequest.UserHostAddress.Length != 0)
            {
                VisitorsIPAddress = currentRequest.UserHostAddress;
            }

            return VisitorsIPAddress;
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Format phone number and exntesion into Mask Format
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetMaskedPhoneNumber(string phoneNumber, string extension)
        {
            string retFormatedNumber = string.Empty;
            try
            {
                retFormatedNumber = !string.IsNullOrEmpty(phoneNumber) ?
                (
                    string.Format("{0}-{1}-{2} x{3}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6, 4),
                    !string.IsNullOrEmpty(extension) ? extension : string.Empty)
                ) : string.Empty;
            }
            catch { }

            return retFormatedNumber;
        }

        #endregion

        #endregion
    }
}
