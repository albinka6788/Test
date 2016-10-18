#region Using directives

using System;

#endregion

namespace BHIC.DML.WC
{
    public class BaseClass
    {
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    }
}
