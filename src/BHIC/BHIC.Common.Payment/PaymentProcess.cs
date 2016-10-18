#region Using directives

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

using AuthorizeNet;
using BHIC.Common.Configuration;
using BHIC.Common.Logging;
using BHIC.Common.XmlHelper;
using BHIC.Common.Config;

#endregion

namespace BHIC.Common.Payment
{
    public class PaymentProcess : IPaymentProcess
    {
        internal ILoggingService loggingService = LoggingService.Instance;

        #region Variables

        public string CommonRedirectUrl
        {
            get
            {
                string paymentURLIP = ConfigCommonKeyReader.PaymentURLIP;

                loggingService.Trace(string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentURLIP) ? System.Web.HttpContext.Current.Request.Url.Host : paymentURLIP),
                    ConfigCommonKeyReader.PaymentURL + "PurchasePath/Payment/ProcessResponse"));

                return string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                    (string.IsNullOrEmpty(paymentURLIP) ? System.Web.HttpContext.Current.Request.Url.Host : paymentURLIP),
                    ConfigCommonKeyReader.PaymentURL + "PurchasePath/Payment/ProcessResponse");
            }
        }

        #endregion

        #region Methods

        #region Interface Implementation

        /// <summary>
        /// It will Generate payment form dynamically
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        PaymentResponse IPaymentProcess.ProcessPayment(PaymentRequest transaction, bool isCalledFromWC = true)
        {
            #region Comment : Here pass CDN based image content base URL reference

            var cdnCommonImagesBaseUrl = ThemeManager.ThemeSharedCommonImagesBaseUrl();

            #endregion

            PaymentResponse paymentResponse = new PaymentResponse();
            PaymentCredentials account = new PaymentCredentials();

            string agencyCode = transaction.AgencyCode;
            decimal paymentAmount = transaction.PaymentAmount;
            decimal feeAmount = transaction.FeeAmount;
            //string contactName = transaction.ContactName;
            //string contactEmail = transaction.ContactEmail;
            //string contactPhone = transaction.ContactPhone;
            string formName = PaymentConstants.FormName;
            string submitButtonText = PaymentConstants.SubmitButtonText;
            bool fromPolicyCenter = false;
            string paymentCode = transaction.PaymentCode;

            var pageOrMessage = fromPolicyCenter ? PaymentConstants.PaymentConfirmationTypeEnum.ExistingPolicy : PaymentConstants.PaymentConfirmationTypeEnum.NewPolicy;

            // Get the Form to Generate and Inject Custom Values
            var formData = new NameValueCollection();
            var checkoutForm = SIMFormGenerator.OpenForm(account.TransactionUserId, account.TransactionKey, paymentAmount, CommonRedirectUrl, Convert.ToBoolean(account.IsTestMode));
            checkoutForm += SIMFormGenerator.EndForm();

            // Use Regular Expressions to grab values for posting
            var actionUrlMatch = Regex.Match(checkoutForm, "action[ ]?=[ ]?['\"][\\d\\s\\D]+?['\"]", RegexOptions.IgnoreCase);
            if (!actionUrlMatch.Success || actionUrlMatch.Length == 0)
                throw new Exception("The Authorize.net SIMFormGenerator markup could not be parsed.", new Exception("The action attribute is not found in: " + checkoutForm));
            var actionUrl = actionUrlMatch.Value.Split('=')[1].Replace("\"", "").Replace("'", "").Trim();

            var inputMatches = Regex.Matches(checkoutForm, "<input.*", RegexOptions.IgnoreCase);
            if (inputMatches.Count == 0)
                throw new Exception("The Authorize.net SIMFormGenerator markup could not be parsed.", new Exception("Could not find input elements in: " + checkoutForm));

            foreach (Match input in inputMatches)
            {
                // Parse each input
                var nameMatch = Regex.Match(input.Value, "name[ ]?=[ ]?['\"][\\d\\s\\D]+?['\"]", RegexOptions.IgnoreCase);
                if (!nameMatch.Success || nameMatch.Length == 0)
                    throw new Exception("The Authorize.net SIMFormGenerator markup could not be parsed.", new Exception("The name attribute is not found in: " + input));
                var name = nameMatch.Value.Split('=')[1].Replace("\"", "").Replace("'", "").Trim();

                var valuteMatch = Regex.Match(input.Value, "value[ ]?=[ ]?['\"][\\d\\s\\D]+?['\"]", RegexOptions.IgnoreCase);
                if (!valuteMatch.Success || valuteMatch.Length == 0)
                    throw new Exception("The Authorize.net SIMFormGenerator markup could not be parsed.", new Exception("The value attribute is not found in: " + input));
                var value = valuteMatch.Value.Split('=')[1].Replace("\"", "").Replace("'", "").Trim();

                formData.Add(name, value);
            }

            // Convert SIM to DPM
            formData.Remove("x_show_form");
            formData.Remove("x_card_num");
            formData.Remove("x_exp_date");
            formData.Remove("x_relay_response");
            formData.Remove("x_relay_always");
            formData.Remove("x_relay_url");

            formData.Add("x_relay_response", "true");
            formData.Add("x_relay_always", "true");
            //formData.Add("x_relay_url", HttpUtility.HtmlEncode(account.CommonRedirectUrl));
            formData.Add("x_relay_url", HttpUtility.HtmlEncode(CommonRedirectUrl));
            formData.Add("x_invoice_num", HttpUtility.HtmlEncode(paymentCode));

            // Add other form inputs
            /*formData.Add("ds_agency_code", HttpUtility.HtmlEncode(agencyCode));
            formData.Add("ds_apply_payment", applyPayment.ToString());
            formData.Add("ds_contact_email", HttpUtility.HtmlEncode(contactEmail));
            formData.Add("ds_contact_name", HttpUtility.HtmlEncode(contactName));
            formData.Add("ds_contact_phone", HttpUtility.HtmlEncode(contactPhone));
            formData.Add("ds_customer_ip", this.Request.UserHostAddress);
            formData.Add("ds_fee_amount", feeAmount.ToString());
            formData.Add("ds_payment_amount", paymentAmount.ToString());*/
            formData.Add("ds_submit_text", HttpUtility.HtmlEncode(submitButtonText));


            // Add other form inputs
            formData.Add("ds_confirm_type", ((int)pageOrMessage).ToString());
            /*formData.Add("ds_customer_ip", this.Request.UserHostAddress);
            formData.Add("ds_host", this.Request.Url.Host);
            formData.Add("ds_host_port", this.Request.Url.Port.ToString());*/
            formData.Add("ds_payment_code_type", transaction.IsPaymentRequestReceivedFromPurchasePath.ToString());


            // Build card number field
            var cardnum = "<div class='form-group row p0'>";
            cardnum += "<label for='' class='col-12'>Credit Card Number</label>";
            cardnum += "<div class='md-col-8 lg-col-8'><input type='text' maxlength='20' class='field inspectletIgnore' id='x_card_num' name='x_card_num' autocomplete='off'  placeholder='Enter Your Credit Card Number' data-ng-model='cardNumber' numeric-only credit-card-number focus='true' required>";
            cardnum += "<div class='error'>";
            cardnum += "<span class='error' data-ng-show='paymentForm.x_card_num.$error.required && !paymentPlan.isInvalidCard && submitted'>Please enter credit card number</span>";
            cardnum += "<span class='error' data-ng-show='!paymentForm.x_card_num.$error.required && paymentForm.x_card_num.$error.validCard && submitted'>Invalid credit card number</span>";
            cardnum += "</div></div>";
            cardnum += "<div><span class=\"field-validation-valid\" data-valmsg-replace=\"true\" data-valmsg-for=\"x_card_num\"></span></div>";
            cardnum += "</div>";

            // Build expiration fields
            var currentYear = DateTime.Now.Year;
            var years = "";
            for (var i = 0; i < 21; i++)
            {
                years += "<option value=\"" + (currentYear + i).ToString().Substring(2) + "\">" + (currentYear + i).ToString() + "</option>";
            }

            var cardexp = "<div class='form-group'>";
            cardexp += "<label for=''>Expiration Date</label>";
            cardexp += "<div class='row p0'>";
            cardexp += "<div class='col-6 sm-col-6 md-col-4 lg-col-4'>";
            cardexp += "<input type='hidden' id='x_exp_date' name='x_exp_date'>";
            cardexp += "<select class='field inspectletIgnore' name='ccMonth' id='ccMonth' data-ng-model='ccMonth' ng-change='submitted=false' >";
            cardexp += "<option value=''>Month</option>";
            cardexp += "<option value='01'>January</option>";
            cardexp += "<option value='02'>February</option>";
            cardexp += "<option value='03'>March</option>";
            cardexp += "<option value='04'>April</option>";
            cardexp += "<option value='05'>May</option>";
            cardexp += "<option value='06'>June</option>";
            cardexp += "<option value='07'>July</option>";
            cardexp += "<option value='08'>August</option>";
            cardexp += "<option value='09'>September</option>";
            cardexp += "<option value='10'>October</option>";
            cardexp += "<option value='11'>November</option>";
            cardexp += "<option value='12'>December</option>";
            cardexp += "</select>";
            cardexp += "</div>";
            cardexp += "<div class='col-6 sm-col-6 md-col-4 lg-col-4'>";
            cardexp += "<select  class='field inspectletIgnore' name='ccYear' id='ccYear' data-ng-model='ccYear' ng-change='submitted=false' >";
            cardexp += "<option value=''>Year</option>";
            cardexp += years;
            cardexp += "</select>";
            cardexp += "</div>";
            cardexp += "</div>";
            cardexp += "<div class='error'>";
            cardexp += "<span class='error' data-ng-show='!paymentPlan.isValidYear && !paymentPlan.isEmptyExpiration && submitted'>Invalid Expiration date</span>";
            cardexp += "<span class='error' data-ng-show='paymentPlan.isEmptyExpiration  && submitted'>Please enter Expiration month and year</span>";
            cardexp += "</div>";
            cardexp += "</div>";

            //Build name on card
            var nameoncard = "<div class='form-group row p0'>";
            nameoncard += "<label for='' class='col-12'>Name on Card</label>";
            nameoncard += "<div class='md-col-8 lg-col-8'> <input name='user_name' data-ng-model='userName' required  type='text' placeholder='Enter Your Name' class='field inspectletIgnore'>";
            nameoncard += "<div class='error'>";
            nameoncard += "<span class='error' data-ng-show='paymentForm.user_name.$error.required && submitted'>Please enter Name on Card</span>";
            nameoncard += "</div></div>";
            nameoncard += "</div></div>";

            // Build new form
            var newform = "<form action=\"" + actionUrl + "\" method=\"POST\" name=\"" + formName + "\" class=\"creditcardform\" novalidate>";

            newform += "<div class='payment-block mb1'>";
            newform += "<div class='row heading mb1'> Payment Method </div>";
            newform += "<p class='text-bold mb-half'>  Pay using Credit Card  </p>";
            newform += "<ul class='accepted-cards no-style'>";
            newform += "<li>";
            newform += "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' width='56.7' height='35.9' viewBox='0 0 56.7 35.9' xml:space='preserve'>";
            newform += "<style type='text/css'>";
            newform += ".st0{fill-rule: evenodd;clip-rule: evenodd;fill:none;stroke:#E1E1E1;}";
            newform += ".st1{fill:#0058A0;}";
            newform += ".st2{fill:#FAA61A;}";
            newform += "</style>";
            newform += "<path class='st0' d='M3.6 0.5h49.4c1.7 0 3.1 1.4 3.1 3.2v28.5c0 1.8-1.4 3.2-3.1 3.2H3.6c-1.7 0-3.1-1.4-3.1-3.2V3.7C0.5 1.9 1.9 0.5 3.6 0.5z'></path>";
            newform += "<polygon class='st1' points='21.3 25 23.7 10.9 27.6 10.9 25.2 25 '></polygon>";
            newform += "<path class='st1' d='M39.2 11.2c-0.8-0.3-2-0.6-3.5-0.6 -3.8 0-6.5 1.9-6.5 4.7 0 2 1.9 3.2 3.4 3.9 1.5 0.7 2 1.1 2 1.8 0 1-1.2 1.4-2.3 1.4 -1.5 0-2.4-0.2-3.6-0.7l-0.5-0.2 -0.5 3.2c0.9 0.4 2.6 0.7 4.3 0.8 4.1 0 6.7-1.9 6.7-4.8 0-1.6-1-2.8-3.2-3.9 -1.4-0.7-2.2-1.1-2.2-1.8 0-0.6 0.7-1.2 2.2-1.2 1.3 0 2.2 0.3 2.9 0.5l0.3 0.2L39.2 11.2'></path>";
            newform += "<path class='st1' d='M49.1 10.9h-3c-0.9 0-1.6 0.3-2 1.2l-5.7 13h4.1c0 0 0.7-1.7 0.8-2.1 0.4 0 4.4 0 5 0 0.1 0.5 0.5 2.1 0.5 2.1h3.6L49.1 10.9M44.3 20c0.3-0.8 1.5-4 1.5-4 0 0 0.3-0.8 0.5-1.4l0.3 1.2c0 0 0.7 3.4 0.9 4.1H44.3z'></path>";
            newform += "<path class='st1' d='M18 10.9l-3.8 9.7 -0.4-2c-0.7-2.3-2.9-4.7-5.4-6L12 25l4.1 0 6.1-14.2H18'></path>";
            newform += "<path class='st2' d='M10.8 10.9H4.5l0 0.3c4.9 1.2 8.1 4 9.4 7.4l-1.4-6.5C12.3 11.2 11.6 10.9 10.8 10.9'></path>";
            newform += "</svg>";
            newform += "</li>";
            newform += "<li>";
            newform += "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' width='56.7' height='35.9' viewBox='0 0 56.7 35.9' xml:space='preserve'>";
            newform += "<style type='text/css'>";
            newform += ".st3{fill:#ED1C2E;}";
            newform += ".st4{fill:#FCB131;}";
            newform += ".st5{fill:#003473;}";
            newform += ".st6{fill:#FFFFFF;}";
            newform += ".st7{fill-rule:evenodd;clip-rule:evenodd;fill:none;stroke:#E1E1E1;}";
            newform += "</style>";
            newform += "<circle class='st3' cx='18.6' cy='17.9' r='14.5'></circle>";
            newform += "<path class='st4' d='M38.1 3.4c-3.8 0-7.2 1.4-9.7 3.8 -0.5 0.5-1 1-1.5 1.5h2.9c0.4 0.5 0.8 1 1.1 1.5h-5.1c-0.3 0.5-0.6 1-0.8 1.5h6.8c0.2 0.5 0.4 1 0.6 1.5h-8.1c-0.2 0.5-0.3 1-0.4 1.5h8.9c0.2 1 0.3 2 0.3 3.1 0 1.6-0.3 3.2-0.7 4.6h-8.1c0.2 0.5 0.4 1 0.6 1.5h6.8c-0.2 0.5-0.5 1-0.8 1.5h-5.1c0.3 0.5 0.7 1 1.1 1.5h2.9c-0.5 0.5-0.9 1.1-1.5 1.5 2.6 2.3 6 3.8 9.7 3.8 8 0 14.5-6.5 14.5-14.5C52.6 9.9 46.1 3.4 38.1 3.4z'></path>";
            newform += "<path class='st5' d='M15.2 19.5c-0.2 0-0.2 0-0.4 0 -0.9 0-1.3 0.3-1.3 0.9 0 0.4 0.2 0.6 0.6 0.6C14.7 21 15.2 20.4 15.2 19.5zM16.4 22.2c-0.2 0-1.3 0-1.3 0l0-0.6c-0.4 0.5-0.9 0.7-1.7 0.7 -0.9 0-1.4-0.7-1.4-1.6 0-1.5 1-2.3 2.8-2.3 0.2 0 0.4 0 0.6 0 0-0.2 0.1-0.3 0.1-0.4 0-0.4-0.3-0.5-1-0.5 -0.8 0-1.4 0.2-1.7 0.3 0-0.1 0.2-1.3 0.2-1.3 0.8-0.2 1.3-0.3 1.9-0.3 1.4 0 2.1 0.6 2.1 1.8 0 0.3 0 0.7-0.1 1.2C16.7 19.9 16.4 21.7 16.4 22.2z'></path>";
            newform += "<polygon class='st5' points='11.3 22.2 9.7 22.2 10.7 16.5 8.6 22.2 7.6 22.2 7.4 16.6 6.5 22.2 5 22.2 6.2 14.8 8.5 14.8 8.6 19 10 14.8 12.5 14.8 '></polygon>";
            newform += "<path class='st5' d='M40 19.5c-0.2 0-0.2 0-0.4 0 -0.9 0-1.3 0.3-1.3 0.9 0 0.4 0.2 0.6 0.6 0.6C39.6 21 40 20.4 40 19.5zM41.2 22.2c-0.2 0-1.3 0-1.3 0l0-0.6c-0.4 0.5-0.9 0.7-1.7 0.7 -0.9 0-1.4-0.7-1.4-1.6 0-1.5 1-2.3 2.8-2.3 0.2 0 0.4 0 0.6 0 0-0.2 0.1-0.3 0.1-0.4 0-0.4-0.3-0.5-1-0.5 -0.8 0-1.4 0.2-1.7 0.3 0-0.1 0.2-1.3 0.2-1.3 0.8-0.2 1.3-0.3 1.9-0.3 1.4 0 2.1 0.6 2.1 1.8 0 0.3 0 0.7-0.1 1.2C41.5 19.9 41.2 21.7 41.2 22.2zM23.4 22.1c-0.4 0.1-0.8 0.2-1.1 0.2 -0.8 0-1.2-0.5-1.2-1.3 0-0.3 0.1-1 0.2-1.6 0.1-0.6 0.7-4.1 0.7-4.1h1.6l-0.2 0.9h0.9L24 17.6H23c-0.2 1.1-0.4 2.6-0.4 2.7 0 0.3 0.2 0.4 0.5 0.4 0.2 0 0.3 0 0.4-0.1L23.4 22.1zM28.2 22.1c-0.5 0.2-1.1 0.2-1.6 0.2 -1.8 0-2.7-0.9-2.7-2.7 0-2 1.2-3.6 2.7-3.6 1.3 0 2.1 0.8 2.1 2.2 0 0.4-0.1 0.9-0.2 1.5h-3.1c-0.1 0.9 0.5 1.2 1.4 1.2 0.6 0 1.1-0.1 1.6-0.4L28.2 22.1zM27.3 18.5c0-0.1 0.2-1.1-0.7-1.1 -0.5 0-0.9 0.4-1 1.1H27.3zM17.3 18.1c0 0.8 0.4 1.3 1.2 1.7 0.6 0.3 0.7 0.4 0.7 0.7 0 0.4-0.3 0.5-0.9 0.5 -0.5 0-0.9-0.1-1.4-0.2 0 0-0.2 1.3-0.2 1.4 0.4 0.1 0.7 0.2 1.6 0.2 1.7 0 2.4-0.6 2.4-2 0-0.8-0.3-1.3-1.1-1.7 -0.7-0.3-0.7-0.4-0.7-0.7 0-0.3 0.3-0.5 0.8-0.5 0.3 0 0.7 0 1.1 0.1l0.2-1.4c-0.4-0.1-1-0.1-1.4-0.1C17.9 16.1 17.3 17 17.3 18.1zM35.8 16.2c0.4 0 0.8 0.1 1.4 0.4l0.3-1.6c-0.2-0.1-1-0.6-1.7-0.6 -1.1 0-1.9 0.5-2.6 1.4 -0.9-0.3-1.3 0.3-1.8 0.9L31 16.8c0-0.2 0.1-0.4 0.1-0.6h-1.4c-0.2 1.9-0.5 3.7-0.8 5.6l-0.1 0.4h1.6c0.3-1.7 0.4-2.8 0.5-3.5l0.6-0.3c0.1-0.3 0.4-0.4 0.9-0.4 -0.1 0.4-0.1 0.8-0.1 1.2 0 2 1.1 3.2 2.8 3.2 0.4 0 0.8-0.1 1.4-0.2l0.3-1.7c-0.5 0.3-1 0.4-1.3 0.4 -0.9 0-1.5-0.7-1.5-1.8C33.8 17.4 34.7 16.2 35.8 16.2zM49.2 14.8L48.8 17c-0.4-0.6-0.9-1-1.5-1 -0.8 0-1.5 0.6-2 1.5 -0.7-0.1-1.3-0.4-1.3-0.4l0 0c0.1-0.5 0.1-0.8 0.1-0.9h-1.4c-0.2 1.9-0.5 3.7-0.8 5.6l-0.1 0.4h1.6c0.2-1.4 0.4-2.5 0.5-3.4 0.5-0.5 0.8-0.9 1.4-0.9 -0.2 0.6-0.4 1.3-0.4 1.9 0 1.5 0.8 2.5 1.9 2.5 0.6 0 1-0.2 1.5-0.7L48 22.2h1.5l1.2-7.4H49.2zM47.2 20.8c-0.5 0-0.8-0.4-0.8-1.2 0-1.2 0.5-2 1.2-2 0.5 0 0.8 0.4 0.8 1.2C48.5 20 47.9 20.8 47.2 20.8z'></path>";
            newform += "<polygon class='st6' points='11.8 21.8 10.2 21.8 11.1 16.1 9.1 21.8 8 21.8 7.9 16.1 6.9 21.8 5.5 21.8 6.7 14.4 9 14.4 9 19 10.6 14.4 13 14.4 '></polygon>";
            newform += "<path class='st6' d='M15.7 19.1c-0.2 0-0.2 0-0.4 0 -0.9 0-1.3 0.3-1.3 0.9 0 0.4 0.2 0.6 0.6 0.6C15.2 20.6 15.6 20 15.7 19.1zM16.8 21.8c-0.2 0-1.3 0-1.3 0l0-0.6c-0.4 0.5-0.9 0.7-1.7 0.7 -0.9 0-1.4-0.7-1.4-1.6 0-1.5 1-2.3 2.8-2.3 0.2 0 0.4 0 0.6 0 0-0.2 0.1-0.3 0.1-0.4 0-0.4-0.3-0.5-1-0.5 -0.8 0-1.4 0.2-1.7 0.3 0-0.1 0.2-1.3 0.2-1.3 0.8-0.2 1.3-0.3 1.9-0.3 1.4 0 2.1 0.6 2.1 1.8 0 0.3 0 0.7-0.1 1.2C17.1 19.4 16.8 21.3 16.8 21.8zM38 14.6l-0.3 1.6c-0.6-0.3-1-0.4-1.4-0.4 -1.2 0-2 1.1-2 2.8 0 1.1 0.6 1.8 1.5 1.8 0.4 0 0.8-0.1 1.3-0.4l-0.3 1.7c-0.6 0.2-1 0.2-1.4 0.2 -1.7 0-2.8-1.2-2.8-3.2 0-2.6 1.5-4.5 3.6-4.5C36.9 14.2 37.7 14.5 38 14.6zM40.5 19.1c-0.2 0-0.2 0-0.4 0 -0.9 0-1.3 0.3-1.3 0.9 0 0.4 0.2 0.6 0.6 0.6C40 20.6 40.5 20 40.5 19.1zM41.6 21.8c-0.2 0-1.3 0-1.3 0l0-0.6c-0.4 0.5-0.9 0.7-1.7 0.7 -0.9 0-1.4-0.7-1.4-1.6 0-1.5 1-2.3 2.8-2.3 0.2 0 0.4 0 0.6 0 0-0.2 0.1-0.3 0.1-0.4 0-0.4-0.3-0.5-1-0.5 -0.8 0-1.4 0.2-1.7 0.3 0-0.1 0.2-1.3 0.2-1.3 0.8-0.2 1.3-0.3 1.9-0.3 1.4 0 2.1 0.6 2.1 1.8 0 0.3 0 0.7-0.1 1.2C42 19.4 41.7 21.3 41.6 21.8zM23.8 21.7c-0.4 0.1-0.8 0.2-1.1 0.2 -0.8 0-1.2-0.5-1.2-1.3 0-0.3 0.1-1 0.2-1.6 0.1-0.6 0.7-4.1 0.7-4.1h1.6l-0.2 0.9h0.8l-0.2 1.4h-0.8c-0.2 1.1-0.4 2.6-0.4 2.7 0 0.3 0.2 0.4 0.5 0.4 0.2 0 0.3 0 0.4-0.1L23.8 21.7zM28.6 21.6c-0.5 0.2-1.1 0.2-1.6 0.2 -1.8 0-2.7-0.9-2.7-2.7 0-2 1.2-3.6 2.7-3.6 1.3 0 2.1 0.8 2.1 2.2 0 0.4-0.1 0.9-0.2 1.5h-3.1c-0.1 0.9 0.5 1.2 1.4 1.2 0.6 0 1.1-0.1 1.6-0.4L28.6 21.6zM27.7 18.1c0-0.1 0.2-1.1-0.7-1.1 -0.5 0-0.9 0.4-1 1.1H27.7zM17.8 17.7c0 0.8 0.4 1.3 1.2 1.7 0.6 0.3 0.7 0.4 0.7 0.7 0 0.4-0.3 0.5-0.9 0.5 -0.5 0-0.9-0.1-1.4-0.2 0 0-0.2 1.3-0.2 1.4 0.4 0.1 0.7 0.2 1.6 0.2 1.7 0 2.4-0.6 2.4-2 0-0.8-0.3-1.3-1.1-1.7 -0.7-0.3-0.7-0.4-0.7-0.7 0-0.3 0.3-0.5 0.8-0.5 0.3 0 0.7 0 1.1 0.1l0.2-1.4c-0.4-0.1-1-0.1-1.4-0.1C18.4 15.6 17.7 16.6 17.8 17.7zM50 21.8h-1.5l0.1-0.6c-0.4 0.5-0.9 0.7-1.5 0.7 -1.1 0-1.9-1-1.9-2.5 0-2 1.2-3.7 2.6-3.7 0.6 0 1.1 0.2 1.5 0.8l0.3-2.1h1.6L50 21.8zM47.7 20.4c0.7 0 1.3-0.8 1.3-2 0-0.8-0.3-1.2-0.8-1.2 -0.7 0-1.2 0.8-1.2 2C46.9 20 47.1 20.4 47.7 20.4zM43.1 15.8c-0.2 1.9-0.5 3.7-0.8 5.6l-0.1 0.4h1.6c0.6-3.7 0.7-4.4 1.6-4.3 0.1-0.7 0.4-1.4 0.6-1.7 -0.7-0.1-1 0.2-1.5 0.9 0-0.3 0.1-0.6 0.1-0.9H43.1zM30.1 15.8c-0.2 1.9-0.5 3.7-0.8 5.6l-0.1 0.4h1.6c0.6-3.7 0.7-4.4 1.6-4.3 0.1-0.7 0.4-1.4 0.6-1.7 -0.7-0.1-1 0.2-1.5 0.9 0-0.3 0.1-0.6 0.1-0.9H30.1z'></path>";
            newform += "<path class='st6' d='M50.7 21.3c0-0.3 0.2-0.5 0.5-0.5 0.3 0 0.5 0.2 0.5 0.5 0 0.3-0.2 0.5-0.5 0.5C50.9 21.8 50.7 21.5 50.7 21.3zM51.1 21.6c0.2 0 0.4-0.2 0.4-0.4 0-0.2-0.2-0.4-0.4-0.4 -0.2 0-0.4 0.2-0.4 0.4C50.8 21.5 50.9 21.6 51.1 21.6zM51.1 21.5L51.1 21.5 51 21.1h0.2c0 0 0.1 0 0.1 0 0 0 0.1 0.1 0.1 0.1 0 0 0 0.1-0.1 0.1l0.1 0.2h-0.1l-0.1-0.2h-0.1V21.5zM51.1 21.3L51.1 21.3c0.1 0 0.1 0 0.1 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0-0.1 0h-0.1V21.3z'></path>";
            newform += "<path class='st7' d='M3.6 0.5h49.4c1.7 0 3.1 1.4 3.1 3.2v28.5c0 1.8-1.4 3.2-3.1 3.2H3.6c-1.7 0-3.1-1.4-3.1-3.2V3.7C0.5 1.9 1.9 0.5 3.6 0.5z'></path>";
            newform += "</svg>";
            newform += "</li>";
            newform += "<li>";
            newform += "<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' id='Layer_1' x='0' y='0' width='56.7' height='35.9' viewBox='0 0 56.7 35.9' xml:space='preserve' enable-background='new 0 0 56.7 35.9'>";
            newform += "<style type='text/css'>";
            newform += ".st8{fill-rule:evenodd;clip-rule:evenodd;fill:none;stroke:#E1E1E1;}";
            newform += ".st9{fill:#F58220;}";
            newform += ".st10{fill:#E83530;}";
            newform += "</style>";
            newform += "<path class='st8' d='M3.6 0.5h49.4c1.7 0 3.1 1.4 3.1 3.2v28.5c0 1.8-1.4 3.2-3.1 3.2H3.6c-1.7 0-3.1-1.4-3.1-3.2V3.7C0.5 1.9 1.9 0.5 3.6 0.5z'></path>";
            newform += "<path class='st9' d='M52.5 16.5c0 0-13.9 9.8-39.5 14.2h39.5V16.5z'></path>";
            newform += "<path class='st10' d='M47.6 28.7'></path>";
            newform += "<path class='st9' d='M28.4 8.6c-2.1 0-3.8 1.6-3.8 3.7 0 2.1 1.6 3.8 3.8 3.8 2.1 0 3.8-1.6 3.8-3.7C32.1 10.2 30.5 8.6 28.4 8.6z'></path>";
            newform += "<path d='M6.8 8.7h-2v7.1h2c1.1 0 1.9-0.3 2.5-0.8 0.8-0.7 1.3-1.7 1.3-2.7C10.6 10.2 9.1 8.7 6.8 8.7zM8.4 14.1c-0.4 0.4-1 0.6-1.9 0.6H6.2V9.9h0.4c0.9 0 1.4 0.2 1.9 0.6 0.5 0.4 0.8 1.1 0.8 1.8C9.2 13 8.9 13.6 8.4 14.1z'></path>";
            newform += "<rect x='11.3' y='8.7' width='1.4' height='7.1'></rect>";
            newform += "<path d='M16.1 11.5c-0.8-0.3-1.1-0.5-1.1-0.9 0-0.4 0.4-0.8 1-0.8 0.4 0 0.8 0.2 1.1 0.6l0.7-0.9c-0.6-0.5-1.3-0.8-2.1-0.8 -1.3 0-2.2 0.9-2.2 2 0 1 0.4 1.5 1.7 2 0.5 0.2 0.8 0.3 1 0.4 0.3 0.2 0.4 0.4 0.4 0.7 0 0.6-0.5 1-1.1 1 -0.7 0-1.2-0.3-1.5-0.9l-0.9 0.9c0.6 0.9 1.4 1.4 2.5 1.4 1.4 0 2.5-1 2.5-2.3C18.1 12.5 17.6 12 16.1 11.5z'></path>";
            newform += "<path d='M18.5 12.3c0 2.1 1.6 3.7 3.8 3.7 0.6 0 1.1-0.1 1.7-0.4V14c-0.6 0.6-1 0.8-1.7 0.8 -1.4 0-2.4-1-2.4-2.5 0-1.4 1-2.4 2.3-2.4 0.7 0 1.2 0.2 1.7 0.8V9c-0.6-0.3-1.1-0.4-1.7-0.4C20.2 8.6 18.5 10.2 18.5 12.3z'></path>";
            newform += "<polygon points='35.3 13.5 33.4 8.7 31.9 8.7 34.9 16 35.6 16 38.7 8.7 37.2 8.7 '></polygon>";
            newform += "<polygon points='39.3 15.8 43.3 15.8 43.3 14.6 40.7 14.6 40.7 12.7 43.2 12.7 43.2 11.5 40.7 11.5 40.7 9.9 43.3 9.9 43.3 8.7 39.3 8.7 '></polygon>";
            newform += "<path d='M48.8 10.8c0-1.3-0.9-2.1-2.5-2.1h-2.1v7.1h1.4V13h0.2l1.9 2.9h1.7l-2.2-3C48.2 12.6 48.8 11.9 48.8 10.8zM46 12h-0.4V9.9H46c0.9 0 1.3 0.4 1.3 1.1C47.3 11.6 46.9 12 46 12z'></path>";
            newform += "<path d='M49.7 8.5c0-0.1-0.1-0.2-0.2-0.2h-0.2V9h0.1V8.7L49.6 9h0.2l-0.2-0.3C49.7 8.7 49.7 8.6 49.7 8.5zM49.5 8.6L49.5 8.6l0-0.2h0c0.1 0 0.1 0 0.1 0.1C49.6 8.6 49.5 8.6 49.5 8.6z'></path>";
            newform += "<path d='M49.5 8.1c-0.3 0-0.5 0.2-0.5 0.5 0 0.3 0.2 0.5 0.5 0.5 0.3 0 0.5-0.2 0.5-0.5C50.1 8.4 49.8 8.1 49.5 8.1zM49.5 9.1c-0.2 0-0.4-0.2-0.4-0.4 0-0.3 0.2-0.4 0.4-0.4 0.2 0 0.4 0.2 0.4 0.4C50 8.9 49.8 9.1 49.5 9.1z'></path>";
            newform += "<path d='M17.2 17.7l1.9 1.9v-1.8h0.3v2.5l-1.9-1.9v1.8h-0.3V17.7z'></path>";
            newform += "<path d='M17.2 17.7v2.5h0.3c0 0 0-1.7 0-1.8 0.1 0.1 1.9 1.9 1.9 1.9v-2.6H19c0 0 0 1.7 0 1.8C19 19.5 17.2 17.7 17.2 17.7L17.2 17.7zM17.2 17.8c0.1 0.1 1.9 1.9 1.9 1.9s0-1.8 0-1.9c0 0 0.2 0 0.2 0 0 0 0 2.3 0 2.4 -0.1-0.1-1.9-1.9-1.9-1.9s0 1.8 0 1.9c0 0-0.2 0-0.2 0C17.2 20.1 17.2 17.9 17.2 17.8z'></path>";
            newform += "<path d='M20.6 17.8h1.2v0.2h-1v0.7h0.9V19h-0.9v0.9h1v0.2h-1.2V17.8z'></path>";
            newform += "<path d='M21.9 17.8h-1.3v2.4h1.3v-0.3c0 0-0.9 0-1 0 0 0 0-0.8 0-0.9 0 0 0.9 0 0.9 0v-0.3c0 0-0.9 0-0.9 0 0 0 0-0.6 0-0.7 0 0 1 0 1 0L21.9 17.8 21.9 17.8zM21.8 17.8c0 0 0 0.2 0 0.2 0 0-1 0-1 0v0.8c0 0 0.9 0 0.9 0 0 0 0 0.2 0 0.2 0 0-0.9 0-0.9 0v1c0 0 0.9 0 1 0 0 0 0 0.2 0 0.2 0 0-1.1 0-1.2 0 0 0 0-2.3 0-2.3C20.7 17.8 21.8 17.8 21.8 17.8z'></path>";
            newform += "<path d='M23.7 20.2h-0.3v-2.1h-0.6v-0.2h1.4v0.2h-0.6V20.2z'></path>";
            newform += "<path d='M24.3 17.8h-1.4v0.3c0 0 0.5 0 0.6 0 0 0 0 2.1 0 2.1h0.3c0 0 0-2.1 0-2.1 0 0 0.6 0 0.6 0L24.3 17.8 24.3 17.8zM24.3 17.8c0 0 0 0.2 0 0.2 0 0-0.6 0-0.6 0s0 2.1 0 2.1c0 0-0.2 0-0.2 0 0 0 0-2.1 0-2.1s-0.5 0-0.6 0c0 0 0-0.2 0-0.2C23 17.8 24.2 17.8 24.3 17.8z'></path>";
            newform += "<path d='M26.1 19.6l0.8-1.9 0.8 1.9 0.6-1.8h0.3l-0.9 2.5 -0.8-1.9 -0.8 1.9 -0.9-2.5h0.3L26.1 19.6z'></path>";
            newform += "<path d='M26.8 17.7c0 0-0.7 1.7-0.7 1.8 0-0.1-0.6-1.7-0.6-1.7h-0.3l0.9 2.6c0 0 0.7-1.8 0.8-1.9 0 0.1 0.8 1.9 0.8 1.9l0.9-2.6h-0.3c0 0-0.6 1.6-0.6 1.7C27.6 19.4 26.8 17.6 26.8 17.7L26.8 17.7zM26.1 19.6c0 0 0.7-1.7 0.7-1.8 0 0.1 0.8 1.9 0.8 1.9s0.6-1.8 0.6-1.8c0 0 0.2 0 0.2 0 0 0.1-0.8 2.3-0.9 2.4 0-0.1-0.8-1.9-0.8-1.9s-0.7 1.8-0.8 1.9c0-0.1-0.9-2.3-0.9-2.4 0.1 0 0.2 0 0.2 0C25.4 17.9 26.1 19.7 26.1 19.6L26.1 19.6z'></path>";
            newform += "<path d='M32 19c0 0.7-0.6 1.2-1.2 1.2 -0.7 0-1.2-0.5-1.2-1.2 0-0.7 0.6-1.2 1.2-1.2C31.4 17.8 32 18.3 32 19zM29.7 19c0 0.5 0.4 1 1 1 0.5 0 1-0.4 1-1 0-0.5-0.4-1-1-1C30.2 18 29.7 18.5 29.7 19z'></path>";
            newform += "<path d='M29.5 19c0 0.7 0.6 1.3 1.3 1.3 0.7 0 1.3-0.6 1.3-1.3 0-0.7-0.6-1.3-1.3-1.3C30 17.7 29.5 18.3 29.5 19zM29.5 19c0-0.7 0.5-1.2 1.2-1.2 0.7 0 1.2 0.5 1.2 1.2 0 0.7-0.5 1.2-1.2 1.2C30 20.2 29.5 19.7 29.5 19z'></path>";
            newform += "<path d='M29.7 19c0 0.6 0.4 1 1 1 0.6 0 1-0.5 1-1 0-0.6-0.4-1-1-1C30.2 18 29.7 18.4 29.7 19zM29.8 19c0-0.5 0.4-1 1-1 0.5 0 0.9 0.4 0.9 1 0 0.5-0.4 1-0.9 1C30.2 20 29.8 19.5 29.8 19z'></path>";
            newform += "<path d='M33.4 20.2h-0.3v-2.4h0.3c0.5 0 0.9 0.1 0.9 0.7 0 0.4-0.2 0.6-0.6 0.7l0.8 1.1h-0.3l-0.7-1h-0.1V20.2zM33.4 18.9L33.4 18.9c0.4 0 0.7-0.1 0.7-0.4 0-0.4-0.3-0.4-0.6-0.4h-0.1V18.9z'></path>";
            newform += "<path d='M33.1 17.8L33.1 17.8l0 2.4h0.3c0 0 0-1 0-1 0 0 0 0 0 0l0 0c0 0 0.7 1 0.7 1l0.3 0h0c0 0-0.7-1-0.8-1.1 0.4 0 0.6-0.3 0.6-0.7 0-0.5-0.3-0.7-0.9-0.7H33.1zM33.5 17.8c0.6 0 0.8 0.2 0.8 0.6 0 0.4-0.2 0.6-0.6 0.6l0 0c0 0 0.7 1 0.8 1.1 -0.1 0-0.3 0-0.3 0h0c0 0-0.7-1-0.7-1l-0.1 0h0c0 0 0 1 0 1 0 0-0.2 0-0.2 0 0 0 0-2.3 0-2.3C33.2 17.8 33.5 17.8 33.5 17.8z'></path>";
            newform += "<path d='M33.4 18L33.4 18l0 0.9h0.1c0.3 0 0.6-0.1 0.6-0.5C34.1 18.1 33.8 18 33.4 18L33.4 18zM33.5 18.1c0.3 0 0.6 0 0.6 0.4 0 0.4-0.3 0.4-0.6 0.4 0 0 0 0-0.1 0C33.4 18.8 33.4 18.1 33.5 18.1 33.4 18.1 33.5 18.1 33.5 18.1z'></path>";
            newform += "<path d='M36.9 17.8h0.4l-1.1 1.1 1.2 1.3h-0.4l-1-1.1 -0.1 0.1v1h-0.3v-2.4h0.3v1L36.9 17.8z'></path>";
            newform += "<path d='M37.2 17.8h-0.4c0 0-0.9 0.9-1 1 0-0.1 0-1 0-1h-0.3v2.4h0.3c0 0 0-1 0-1 0 0 0 0 0 0 0 0 1 1.1 1 1.1l0.4 0h0.1c0 0-1.1-1.3-1.2-1.3C36.2 18.9 37.3 17.8 37.2 17.8L37.2 17.8zM37.2 17.8c-0.1 0.1-1.1 1.1-1.1 1.1s1.1 1.2 1.1 1.2c-0.1 0-0.3 0-0.3 0l0 0c0 0-1-1.1-1-1.1l0 0 -0.1 0.1c0 0 0 1 0 1 0 0-0.2 0-0.2 0 0 0 0-2.3 0-2.3 0 0 0.2 0 0.2 0 0 0 0 1.1 0 1.1s1-1 1-1.1C36.9 17.8 37.1 17.8 37.2 17.8z'></path>";
            newform += "</svg>";
            newform += "</li>";
            newform += "<li>";
            newform += "<svg xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' width='56.7' height='35.9' viewBox='0 0 56.7 35.9' xml:space='preserve'>";
            newform += "<style type='text/css'>";
            newform += ".st11{fill-rule:evenodd;clip-rule:evenodd;fill:none;stroke:#E1E1E1;}";
            newform += ".st12{fill:#0986C8;}";
            newform += ".st13{fill:#0884C7;}";
            newform += "</style>";
            newform += "<path class='st11' d='M3.6 0.5h49.4c1.7 0 3.1 1.4 3.1 3.2v28.5c0 1.8-1.4 3.2-3.1 3.2H3.6c-1.7 0-3.1-1.4-3.1-3.2V3.7C0.5 1.9 1.9 0.5 3.6 0.5z'></path>";
            newform += "<path class='st12' d='M19.2 17h5.2v-1.3h-3.6v-1.3l3.5 0v-1.3h-3.5v-1.2h3.6v-1.4h-5.2V17zM13.6 15l-2-4.4H9v6.1l-2.6-6.1H4.1L1.4 17H3l0.6-1.4h3.2L7.4 17h3.1V12l2.2 5.1h1.4l2.2-5.1 0 5.1H18v-6.5h-2.6L13.6 15zM4.2 14.3l1.1-2.6 1.1 2.6H4.2zM50.8 10.6v4.5L48 10.6h-2.4v6.1l-2.6-6.1h-2.3l-2.2 5.1h-0.7c-0.4 0-0.8-0.1-1.1-0.3 -0.3-0.3-0.4-0.8-0.4-1.5 0-0.7 0.2-1.2 0.4-1.5 0.3-0.3 0.6-0.4 1.2-0.4h1.5v-1.4H38c-1.1 0-1.8 0.2-2.3 0.8 -0.7 0.7-0.8 1.6-0.8 2.5 0 1.2 0.3 1.9 0.8 2.5 0.5 0.5 1.5 0.7 2.2 0.7h1.8l0.6-1.4h3.2l0.6 1.4h3.1l0-4.8 2.9 4.8h2.2v-6.5H50.8zM40.8 14.3l1.1-2.6 1.1 2.6H40.8zM31.6 12.3c0-0.7-0.3-1.1-0.8-1.4 -0.5-0.3-1.1-0.3-1.9-0.3h-3.6V17h1.5v-2.3h1.7c0.6 0 0.9 0 1.1 0.3 0.3 0.3 0.2 0.9 0.2 1.3l0 0.8h1.6v-1.3c0-0.6 0-0.9-0.3-1.2 -0.1-0.2-0.4-0.4-0.8-0.5C30.9 13.9 31.6 13.4 31.6 12.3zM29.6 13.2c-0.2 0.1-0.5 0.1-0.8 0.1h-1.9v-1.4h1.9c0.3 0 0.6 0 0.7 0.1 0.2 0.1 0.3 0.3 0.3 0.6C29.9 12.9 29.8 13.1 29.6 13.2zM32.5 17h1.6v-6.5h-1.6V17z'></path>";
            newform += "<path class='st13' d='M27.6 18.3h-4.9l-2 2.1 -1.9-2.1h-6.2v6.5h6.1l2-2.1 1.9 2.1h3v-2.2h1.9c1.4 0 2.7-0.4 2.7-2.2C30.2 18.6 28.8 18.3 27.6 18.3zM18 23.4h-3.8v-1.3h3.4v-1.3h-3.4v-1.2H18l1.7 1.9L18 23.4zM24.1 24.1l-2.4-2.6 2.4-2.5V24.1zM27.6 21.2h-2v-1.6h2c0.6 0 0.9 0.2 0.9 0.8C28.6 20.9 28.2 21.2 27.6 21.2zM55.2 21.3c-0.3-0.3-0.9-0.5-1.8-0.5l-0.8 0c-0.3 0-0.4 0-0.6-0.1 -0.2-0.1-0.3-0.2-0.3-0.5 0-0.2 0.1-0.4 0.2-0.5 0.2-0.1 0.3-0.1 0.6-0.1h3.2v-1.4h-3.5c-1.6 0-2.2 1-2.2 1.9 0 2 1.8 2 3.3 2 0.3 0 0.4 0 0.5 0.1 0.1 0.1 0.2 0.2 0.2 0.4 0 0.2-0.1 0.3-0.2 0.4 -0.1 0.1-0.3 0.2-0.6 0.2h-3v1.4h3c1.6 0 2.4-0.6 2.4-2C55.6 22 55.5 21.6 55.2 21.3zM37.2 20c0-0.7-0.3-1.1-0.8-1.4 -0.5-0.3-1.1-0.3-1.9-0.3H31v6.5h1.5v-2.4h1.7c0.6 0 0.9 0.1 1.1 0.3 0.3 0.3 0.3 0.9 0.3 1.3v0.8h1.5v-1.3c0-0.6 0-0.9-0.3-1.2 -0.1-0.2-0.4-0.4-0.8-0.5C36.5 21.5 37.2 21 37.2 20zM35.1 20.9C34.9 21 34.7 21 34.4 21h-1.9v-1.4h1.9c0.3 0 0.6 0 0.7 0.1 0.2 0.1 0.3 0.3 0.3 0.6C35.5 20.6 35.3 20.8 35.1 20.9zM47.3 20.8l-0.8 0c-0.3 0-0.4 0-0.6-0.1 -0.2-0.1-0.3-0.2-0.3-0.5 0-0.2 0.1-0.4 0.2-0.5 0.2-0.1 0.3-0.1 0.6-0.1h2.8v-1.4h-3.1c-1.6 0-2.2 1-2.2 1.9 0 2 1.8 2 3.3 2 0.3 0 0.4 0 0.5 0.1 0.1 0.1 0.2 0.2 0.2 0.4 0 0.2-0.1 0.3-0.2 0.4 -0.1 0.1-0.3 0.2-0.6 0.2h-3v1.4h3c1.6 0 2.4-0.6 2.4-2 0-0.7-0.2-1.1-0.5-1.4C48.7 21 48.1 20.8 47.3 20.8zM38 24.7h5.2v-1.3l-3.6 0v-1.3h3.5v-1.3h-3.5v-1.2h3.6v-1.3H38V24.7z'></path>";
            newform += "</svg>";
            newform += "</li>";
            newform += "</ul>";


            foreach (var key in formData.AllKeys)
            {
                newform += "<input type=\"hidden\" name=\"" + key + "\" value=\"" + formData[key] + "\" />";
            }

            newform += cardnum;
            newform += cardexp;
            newform += nameoncard;

            newform += "<p class='ssl-text text-blue text-medium'>";
            newform += "<svg class='icon-lock' xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' width='20' height='20.4' viewBox='-289.8 386.3 18.5 20.4' xml:space='preserve'>";
            newform += "<path class='fill-teal' d='M-274.8 394h-1.1v-2c0-2.5-2.1-4.6-4.6-4.6s-4.6 2.1-4.6 4.6v2h-1v-2c0-3.2 2.6-5.7 5.7-5.7s5.7 2.5 5.7 5.7v2H-274.8z'></path>";
            newform += "<rect x='-281.1' y='398.9' class='fill-teal' width='1.1' height='2.8'></rect>";
            newform += "<path class='fill-teal' d='M-274.2 406.7h-12.7c-1.6 0-2.9-1.3-2.9-2.9V397c0-1.6 1.3-2.9 2.9-2.9h12.7c1.6 0 2.9 1.3 2.9 2.9v6.8C-271.2 405.4-272.5 406.7-274.2 406.7zM-286.9 395.1c-1 0-1.8 0.8-1.8 1.8v6.8c0 1 0.8 1.8 1.8 1.8h12.7c1 0 1.8-0.8 1.8-1.8V397c0-1-0.8-1.8-1.8-1.8h-12.7V395.1z'></path>";
            newform += "</svg>";
            newform += "This is a secure SSL encrypted payment. You are safe.";
            newform += "</p>";
            newform += "<p class='mb1 text-bold'> Please review the terms and conditions below:</p>";
            newform += "<div class='terms-conditions-box'>";
            newform += "<p class='text-bold'>If you cancel the policy, the premium earned prior to cancellation will be increased (multiplied by a factor to determine the short rate penalty premium). The maximum factor that can be applied to your earned premium is 18.24. This factor applies if you cancel the first day of your policy period. The final premium will not be less than the full highest minimum premium for the classifications covered by this policy.</p>";
            newform += "<p> Personal information about you, including information from a credit or other investigative report, may be collected from persons other than you in connection with this application for insurance and subsequent amendments and renewals. Such information as well as other personal and privileged information collected by us or our agents may in certain circumstances be disclosed to third parties without your authorization. Credit scoring information may be used to help determine either your eligibility for insurance or the premium you will be charged. We may use a third party in connection with the development of your score. You may have the right to review your personal information in our files and request correction of any inaccuracies. You may also have the right to request in writing that we consider extraordinary life circumstances in connection with the development of your credit score. These rights may be limited in some states. Please contact your agent or broker to learn how these rights may apply in your state or for instructions on how to submit a request to us for a more detailed description of your rights and our practices regarding personal information. (not applicable in AZ, CA, DE, KS, MA, MN, ND, NY, or, VA, or WV.) </p>";
            newform += "<h4>Applicable in Arizona</h4>";
            newform += "<p>As described in Arizona revised statute 20-2104(d), a credit report or other investigative report about you may be requested in connection with this application for insurance. Any information which we have or may obtain about you or other individuals listed as policyholders on our policy will be treated confidentially. However, this information, as well as other personal or privileged information subsequently collected, may under certain circumstances, be disclosed without prior authorization to non-affiliated third parties. We may also share such information with affiliated companies for such purposes as claims handling, servicing, underwriting and insurance marketing. You have the right to see personal information collected about you, and you have the right to correct any information which may be wrong. Also, pursuant to Arizona revised statute 20-2104(c), if you are interested in obtaining a complete description of information practices, and your rights regarding information we collect, please write us at the address provided with your policy.</p>";
            newform += "<h4> Applicable in California</h4>";
            newform += "<p> This authorization shall expire one year from the date you signed the authorization.</p>";
            newform += "<h4> Applicable in Massachusetts: </h4>";
            newform += "<p> Credit scoring information may be used to determine your eligibility for insurance but not for rating purposes.</p>";
            newform += "<h4> Applicable in Minnesota</h4>";
            newform += "<p>I, the undersigned, hereby authorize my agent named in this application, if any, and/or the underwriting department of the insurance company named in this application to collect credit-related and other information about me from the following types of organizations: credit bureaus and other organizations providing personal or privileged information. I understand this information will be used for the purpose of making underwriting decisions in connection with the insurance for which I have applied, sought reinstatement or requested a change in benefits. These decisions may include determinations to grant or deny me coverage and/or the rates I will be charged. I also understand that I have the right to request in writing that extraordinary life circumstances be considered in connection with the development of my credit score.</p>";
            newform += "<h4> Applicable in Oregon: </h4>";
            newform += "<p>In connection with my application for insurance to the company shown above, I hereby authorized you to collect and disclose personal, privileged information, about me, by and to consumer reporting agencies, your authorized representatives, assignees, agents and affiliates. The information collected and disclosed extends to my credit standing, credit worthiness, credit capacity, personal characteristics and mode of living. I understand that credit scoring information may be used to either determine my eligibility for insurance or the premium I will be charged. Credit scoring cannot be used for renewals unless requested by the insured. I understand that I am entitled to receive a copy of this authorization and, upon request, a record of any subsequent disclosures of personal or privileged information that must include the name, mailing address ad institutional affiliation of the party to which the information was disclosed as well as the date of the disclosure, and to the extent practicable, a description of the information being disclosed.</p>";
            newform += "<h4> Applicable in AL, AR, DC, LA, MD, NM, RI and WV:  </h4>";
            newform += "<p> Any person who knowingly (or willfully)* presents a false or fraudulent claim for payment of a loss or benefit or knowingly (or willfully)* presents false information in an application for insurance is guilty of a crime and may be subject to fines and confinement in prison. </p>";
            newform += "<p class='text-bold'> *Applies in MD Only </p>";
            newform += "<h4> Applicable in CO: </h4>";
            newform += "<p> It is unlawful to knowingly provide false, incomplete, or misleading facts or information to an insurance company for the purpose of defrauding or attempting to defraud the company. Penalties may include imprisonment, fines, denial of insurance and civil damages. Any insurance company or agent of an insurance company who knowingly provides false, incomplete, or misleading facts or information to a policyholder or claimant for the purpose of defrauding or attempting to defraud the policyholder or claimant with regard to a settlement or award payable from insurance proceeds shall be reported to the Colorado Division of Insurance within the Department of Regulatory Agencies. </p>";
            newform += "<h4> Applicable in FL and OK </h4>";
            newform += "<p> Any person who knowingly and with intent to injure, defraud, or deceive any insurer files a statement of claim or an application containing any false, incomplete, or misleading information is guilty of a felony (of the third degree)*. </p>";
            newform += "<p class='text-bold'> *Applies in FL Only </p>";
            newform += "<h4> Applicable in KS:  </h4>";
            newform += "<p> Any person who, knowingly and with intent to defraud, presents, causes to be presented or prepares with knowledge or belief that it will be presented to or by an insurer, purported insurer, broker or any agent thereof, any written statement as part of, or in support of, an application for the issuance of, or the rating of an insurance policy for personal or commercial insurance, or a claim for payment or other benefit pursuant to an insurance policy for commercial or personal insurance which such person knows to contain materially false information concerning any fact material thereto; or conceals, for the purpose of misleading, information concerning any fact material thereto commits a fraudulent insurance act. </p>";
            newform += "<h4> Applicable in KY, NY, OH and PA: </h4>";
            newform += "<p> Any person who knowingly and with intent to defraud any insurance company or other person files an application for insurance or statement of claim containing any materially false information or conceals for the purpose of misleading, information concerning any fact material thereto commits a fraudulent insurance act, which is a crime and subjects such person to criminal and civil penalties (not to exceed five thousand dollars and the stated value of the claim for each such violation)*. </p>";
            newform += "<p class='text-bold'> *Applies in NY Only </p>";
            newform += "<h4> Applicable in ME, TN, VA and WA:  </h4>";
            newform += "<p> It is a crime to knowingly provide false, incomplete or misleading information to an insurance company for the purpose of defrauding the company. Penalties (may)* include imprisonment, fines and denial of insurance benefits. </p>";
            newform += "<p class='text-bold'> *Applies in ME Only. </p>";
            newform += "<h4> Applicable in NJ </h4>";
            newform += "<p> Any person who includes any false or misleading information on an application for an insurance policy is subject to criminal and civil penalties. </p>";
            newform += "<h4> Applicable in OR: </h4>";
            newform += "<p> Any person who knowingly and with intent to defraud or solicit another to defraud the insurer by submitting an application containing a false statement as to any material fact may be violating state law. </p>";
            newform += "<h4> Applicable in UT: </h4>";
            newform += "<p> Any person who knowingly presents false or fraudulent underwriting information, files or causes to be filed a false or fraudulent claim for disability compensation or medical benefits, or submits a false or fraudulent report or billing for health care fees or other professional services is guilty of a crime and may be subject to fines and confinement in state prison. </p>";
            newform += "<p class='text-bold'> I agree that I am the applicant or an authorized representative of the applicant and represents that reasonable inquiry has been made to obtain the answers to questions on this application. I agree that I fully read and understand the terms of this application. I certify and represent that the answers are true, correct and complete to the best of my knowledge. </p>";
            newform += "<p class='text-bold'> The insurance premium presented does not include Workers' Compensation coverage for your business's owners, officers, or partners. If regulations in your state require that these individuals be covered, we will contact you shortly about a modification to your insurance premium. If regulations in your state do not require that these individuals be covered but you are interested in securing Workers' Comp coverage for an owner, officer, or partner, please contact us at your convenience. </p>";
            newform += "</div>";
            newform += "<label class='checkbox mb2'>";
            newform += "<input type=\"checkbox\"  data-ng-model=\"acceptTermModel\" ng-change=\"acceptTerms(acceptTermModel);\">";
            newform += "<span>";
            newform += "<svg class='icon-check' xmlns='http://www.w3.org/2000/svg' version='1.1' x='0' y='0' viewBox='-124 239.2 281 242' xml:space='preserve'>";
            newform += "<path class='fill-teal--dark' d='M130.5 257.9c-9.3-9.3-24.4-9.3-33.7 0.7L-37.5 397l-35.1-35.1c-8.6-8.6-23-9.3-32.3-0.7l-11.5 10c-10 8.6-10 24.4-0.7 33.7l56.7 56.7c0 0 0 0 0.7 0l5.7 6.5c9.3 9.3 24.4 9.3 33.7-0.7l162.2-167c8.6-9.3 8.6-23.7-0.7-32.3L130.5 257.9z'></path>";
            newform += "</svg>";
            newform += "</span> I have read and agree to Cover your Business Terms &amp; Conditions.";
            newform += "</label>";
            newform += "<div class='btn-actions clear mb2'>";
            newform += "<input type='submit' class='btn btn-primary pull-right' ng-click='submitted=true' value='Pay Now' ng-disabled='paymentPlan.isPaymentDisabled'>";
            newform += "</div>";

            newform += "</form>";

            paymentResponse.ReturnString = newform;

            return paymentResponse;
        }

        /// <summary>
        /// It will set payment response into session
        /// </summary>
        /// <param name="isSuccess"></param>
        PaymentResponseViewModel IPaymentProcess.SetPaymentResponseInSession(int isSuccess, string paymentErrorMessage = "", string invoiceNumber = "", string encodedInvoiceNumber = "", bool isPaymentRequestReceivedFromPurchasePath = true, string transactionId = "", string amount = "")
        {
            PaymentResponseViewModel customSession = new PaymentResponseViewModel();

            customSession.PaymentErrorMessage = paymentErrorMessage;
            customSession.IsPaymentSuccess = isSuccess;

            var encryptedText = Encryption.EncryptText(string.Format("invoiceNumber={0}&transactionId={1}&amount={2}", invoiceNumber, transactionId, amount));

            //save encrypted data into session
            customSession.TransactionCode = ((isSuccess != 0) ? encryptedText : string.Concat(Constants.FailedPaymentCode, encryptedText));

            customSession.PaymentURL = string.Concat(System.Web.HttpContext.Current.Request.Url.Scheme, "://",
                                (string.IsNullOrEmpty(ConfigCommonKeyReader.PaymentURLIP) ?
                                System.Web.HttpContext.Current.Request.Url.Host : ConfigCommonKeyReader.PaymentURLIP),
                                ConfigCommonKeyReader.PaymentURL);

            if (isPaymentRequestReceivedFromPurchasePath)
            {
                if (customSession.IsPaymentSuccess == 0)
                {
                    customSession.PaymentURL = string.Concat(customSession.PaymentURL, "PurchasePath/Quote/Index#/BuyPolicy/", customSession.TransactionCode);
                }
                else
                {
                    //   customSession.PaymentURL = string.Concat(customSession.PaymentURL, "PurchasePath/OrderSummary/Confirmation?transactionCode=", @customSession.TransactionCode);
                    customSession.PaymentURL = string.Concat(customSession.PaymentURL, "PurchasePath/Quote/Index#/OrderSummary/", customSession.TransactionCode);
                }
            }
            else
            {
                if (customSession.IsPaymentSuccess == 0)
                {
                    customSession.PaymentURL = string.Concat(customSession.PaymentURL, ConfigCommonKeyReader.PolicyCentreURL, "Dashboard/Index#/MakePayment/", Constants.FailedPaymentCode, "|:|", customSession.TransactionCode + "1");
                }
                else
                {
                    customSession.PaymentURL = string.Concat(customSession.PaymentURL, ConfigCommonKeyReader.PolicyCentreURL, "Dashboard/Index#/PolicyInformation/", @customSession.TransactionCode);
                }
            }
            return customSession;
        }

        /// <summary>
        /// It will write payment info into log
        /// </summary>
        /// <param name="x_response_code"></param>
        /// <param name="x_response_reason_code"></param>
        /// <param name="x_response_reason_text"></param>
        /// <param name="x_auth_code"></param>
        /// <param name="x_avs_code"></param>
        /// <param name="x_trans_id"></param>
        /// <param name="x_invoice_num"></param>
        /// <param name="x_amount"></param>
        /// <param name="x_type"></param>
        /// <param name="x_MD5_Hash"></param>
        /// <param name="x_cvv2_resp_code"></param>
        /// <param name="x_cavv_response"></param>
        /// <param name="ds_apply_payment"></param>
        /// <param name="ds_customer_ip"></param>
        /// <param name="ds_contact_name"></param>
        /// <param name="ds_contact_email"></param>
        /// <param name="ds_contact_phone"></param>
        /// <param name="ds_agency_code"></param>
        /// <param name="ds_payment_code_type"></param>
        /// <param name="ds_confirm_type"></param>
        /// <param name="ds_submit_text"></param>
        /// <param name="ds_payment_amount"></param>
        /// <param name="ds_fee_amount"></param>
        /// <returns></returns>
        bool IPaymentProcess.WriteLogForPaymentResponse(string x_response_code, string x_response_reason_code, string x_response_reason_text, string x_auth_code, string x_avs_code, string x_trans_id, string x_invoice_num, string x_amount, string x_type, string x_MD5_Hash, string x_cvv2_resp_code, string x_cavv_response, string ds_apply_payment, string ds_customer_ip, string ds_contact_name, string ds_contact_email, string ds_contact_phone, string ds_agency_code, string ds_payment_code_type, string ds_confirm_type, string ds_submit_text, string ds_payment_amount, string ds_fee_amount)
        {
            loggingService.Trace(string.Concat("x_response_code = ", (string.IsNullOrEmpty(x_response_code) ? x_response_code : string.Empty), System.Environment.NewLine,
                  "x_response_reason_code = ", x_response_reason_code, System.Environment.NewLine,
                  "x_response_reason_text = ", x_response_reason_text, System.Environment.NewLine,
                  "x_auth_code = ", x_auth_code, System.Environment.NewLine,
                  "x_avs_code = ", x_avs_code, System.Environment.NewLine,
                  "x_trans_id = ", x_trans_id, System.Environment.NewLine,
                  "x_invoice_num = ", x_invoice_num, System.Environment.NewLine,
                  "x_amount = ", x_amount, System.Environment.NewLine,
                  "x_type = ", x_type, System.Environment.NewLine,
                  "x_MD5_Hash = ", x_MD5_Hash, System.Environment.NewLine,
                  "x_cvv2_resp_code = ", x_cvv2_resp_code, System.Environment.NewLine,
                  "x_cavv_response = ", x_cavv_response, System.Environment.NewLine,
                  "ds_apply_payment = ", (string.IsNullOrEmpty(ds_apply_payment) ? string.Empty : ds_apply_payment), System.Environment.NewLine,
                  "ds_customer_ip = ", (string.IsNullOrEmpty(ds_customer_ip) ? string.Empty : ds_customer_ip), System.Environment.NewLine,
                  "ds_contact_name = ", (string.IsNullOrEmpty(ds_contact_name) ? string.Empty : ds_contact_name), System.Environment.NewLine,
                  "ds_contact_email = ", (string.IsNullOrEmpty(ds_contact_email) ? string.Empty : ds_contact_email), System.Environment.NewLine,
                  "ds_contact_phone = ", (string.IsNullOrEmpty(ds_contact_phone) ? string.Empty : ds_contact_phone), System.Environment.NewLine,
                  "ds_agency_code = ", (string.IsNullOrEmpty(ds_agency_code) ? string.Empty : ds_agency_code), System.Environment.NewLine,
                  "ds_payment_code_type = ", (string.IsNullOrEmpty(ds_payment_code_type) ? string.Empty : ds_payment_code_type), System.Environment.NewLine,
                  "ds_confirm_type = ", (string.IsNullOrEmpty(ds_confirm_type) ? string.Empty : ds_confirm_type), System.Environment.NewLine,
                  "ds_submit_text = ", (string.IsNullOrEmpty(ds_submit_text) ? string.Empty : ds_submit_text), System.Environment.NewLine,
                  "ds_payment_amount = ", (string.IsNullOrEmpty(ds_payment_amount) ? string.Empty : ds_payment_amount), System.Environment.NewLine,
                  "ds_fee_amount = ", (string.IsNullOrEmpty(ds_fee_amount) ? string.Empty : ds_fee_amount)));

            return true;
        }

        #endregion

        #endregion
    }
}
