using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Service;

namespace BHIC.Domain.Background
{
    public class FieldDataResponse
    {
        public List<FieldData> FieldDatas { get; set; }
        public OperationStatus OperationStatus { get; set; }

        public FieldDataResponse()
        {
            FieldDatas = new List<FieldData>();
            OperationStatus = new OperationStatus();
        }
    }
}
