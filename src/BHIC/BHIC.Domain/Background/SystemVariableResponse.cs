using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
    public class SystemVariableResponse
    {
        // ----------------------------------------
        // constructor
        // ----------------------------------------

        public SystemVariableResponse()
        {
            // init objects to help avoid issues related to null reference exceptions
            SystemVariables = new List<SystemVariable>();
            OperationStatus = new OperationStatus();
        }

        // ----------------------------------------
        // properties
        // ----------------------------------------

        public List<SystemVariable> SystemVariables { get; set; }
        public OperationStatus OperationStatus { get; set; }
    }
}