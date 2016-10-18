using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Comment : Here Other than default library

namespace BHIC.Domain.QuestionEngine
{
    public class MyQuestionParameters
    {
        public int ZipCode { get; set; }
        public int IndustryId { get; set; }
        public int SubindustryId { get; set; }
        public int ClassId { get; set; }
        public IDictionary<string, object> OtherKVs { get; set; }

        //Comment : Here in case API is callable through KeyNames
        public string IndustryName { get; set; }
        public string SubindustryName { get; set; }
        public string ClassName { get; set; }
    }
}
