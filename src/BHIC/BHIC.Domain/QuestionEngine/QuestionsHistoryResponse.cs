#region Using directives

using BHIC.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace BHIC.Domain.QuestionEngine
{
    public class QuestionsHistoryResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public QuestionsHistoryResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            OperationStatus = new OperationStatus();
            Questions = new List<Question>();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        /// <summary>
        /// Read-only response/status information; populated by the service.
        /// </summary>
        public OperationStatus OperationStatus { get; set; }

        // questions (and responses) list
        /// <summary>
        /// Questions to be displayed to the user, along with rules for conditional display, along with previously-saved user responses.
        /// </summary>
        public List<Question> Questions { get; set; }
    }
}