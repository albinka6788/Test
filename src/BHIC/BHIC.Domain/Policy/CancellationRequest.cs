using System;
using System.ComponentModel.DataAnnotations;

namespace BHIC.Domain.Policy
{
    public class CancellationRequest
    {
        public int? CancellationRequestId { get; set; }

        [StringLength(10)]
        [Required]
        public string PolicyId { get; set; }

        [Required]
        public DateTime? RequestedEffectiveDate { get; set; }

        [Required]
        public string EffectiveDate { get; set; }

        public DateTime RequestDate { get; set; }

        [StringLength(60)]
        public string ContactIpAddress { get; set; }

        [StringLength(255)]
        [Required]
        public string ContactEmail { get; set; }

        [StringLength(20)]
        public string ContactPhoneNumber { get; set; }

        [StringLength(256)]
        public string ContactName { get; set; }

        //Added Custom Property from xceedance to Guard Domain Class        
        public string ReasonForCancellation { get; set; }

        [Required(ErrorMessage = "Reason for cancellation is required.")]
        public int ReasonID { get; set; }

        //Added Custom Property from xceedance to Guard Domain Class
        public string Description { get; set; }
    }
}

