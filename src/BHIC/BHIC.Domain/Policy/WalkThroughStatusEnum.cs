using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Domain.Policy
{
    public enum WalkThroughStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed  = 2,
        Refer      = 3,
        Error      = 9
    }
}