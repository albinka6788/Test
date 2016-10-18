#region Using directives

using System;

#endregion

namespace BHIC.Domain.QuestionEngine
{
    public class BusinessContact
    {
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
