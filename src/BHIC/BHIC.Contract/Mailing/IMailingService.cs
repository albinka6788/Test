#region Using directives

using BHIC.ViewDomain.Landing;
using BHIC.ViewDomain.Mailing;
using System;
using System.Collections.Generic;

#endregion

namespace BHIC.Contract.Mailing
{
    public interface IMailingService
    {
        /// <summary>
        /// Send confirmation mail to user, after successful registration
        /// </summary>
        /// <returns>return true on success,false otherwise</returns>
        bool UserRegistrationMail(string userEmail, PolicyRegistrationMailViewModel registrationMail);

        /// <summary>
        /// Send welcome mail to user
        /// </summary>
        /// <returns></returns>
        bool WelcomeMail(string userEmail, PolicyWelcomeMailViewModel welcomeMail);

        /// <summary>
        /// Send reset password link to user
        /// </summary>
        /// <param name="forgotPasswordVM"></param>
        /// <returns>return true on success, false otherwise</returns>
        bool ForgotPassword(ForgotPasswordViewModel forgotPasswordVM);

        /// <summary>
        /// send failed payment details in case of failure
        /// </summary>
        /// <param name="pfMailVm"></param>
        /// <returns></returns>
        bool PaymentFailure(PaymentFailureViewModel pfMailVm);

        /// <summary>
        /// Send welcome mail to user on puchase of BOP Policy
        /// </summary>
        /// <returns></returns>
        bool WelcomeMailBOP(string userEmail, PolicyWelcomeMailViewModel welcomeMail);

        /// <summary>
        /// Send mail(having details of scheduled call) to internal team
        /// </summary>
        /// <param name="scheduleCallViewModel">Contains mail details</param>
        /// <returns>return true on success, false otherwise</returns>
        bool ScheduleCall(ScheduleCallViewModel scheduleCallViewModel);

        /// <summary>
        /// Send Mail for save for later
        /// </summary>
        /// <param name="saveForLaterMailViewModel"></param>
        /// <returns></returns>
        bool SaveForLater(string userEmail, SaveForLaterMailViewModel saveForLaterMailViewModel);
    }
}
