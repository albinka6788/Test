using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Service
{
    /// <summary>
    /// Parameters associated with the ServiceEventLog service<br />
    /// Filters the response as indicated by the comments for each parameter
    /// </summary>
    public class ServiceEventLogRequestParms
    {
        /// <summary>
        /// return the specified event log row.  <br />
        /// Optional.
        /// </summary>
        public int? ServiceEventLogId { get; set; }

        /// <summary>
        /// return the ServiceTestResults associated with the specified ServiceTestEvent.  <br />
        /// Optional.
        /// </summary>
        public int? ServiceTestEventId { get; set; }

        /// <summary>
        /// return event logs for specified RecordKey (e.g. - Questions GET and POST save QuoteId as the RecordKey)  <br />
        /// Optional.
        /// </summary>
        public int? RecordKey { get; set; }

        /// <summary>
        /// Name of the service.  Example: "Questions"  <br />
        /// Optional.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Service Method.  Examples: "GET", "POST", "DELETE"  <br />
        /// Optional.
        /// </summary>
        public string ServiceMethod { get; set; }

        /// <summary>
        /// Log entries will be returned for the period specified between the StartTime and EndTime properties  <br />
        /// Required if ServiceEventLogId is not specified
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Log entries will be returned for the period specified between the StartTime and EndTime properties  <br />
        /// Required if ServiceEventLogId is not specified
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
