#region Using directives

using System;

#endregion

namespace BHIC.DML.WC.DTO
{
    public class LineOfBusiness
    {
        public Int32 Id { get; set; }
        public string Abbreviation { get; set; }
        public string LobFullName { get; set; }
        public string StateCode { get; set; }
        public Int32 StateLineOfBusinessId { get; set; }
        public string Status { get; set; }
    }
}
