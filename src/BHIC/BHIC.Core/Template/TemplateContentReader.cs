using BHIC.Common;
using BHIC.Contract.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Core.Template
{
    public class TemplateContentReader : ITemplateContentReader
    {

        string ITemplateContentReader.GetContent(string TemplatePath)
        {
            string content ="";

            content = URL.ReadAllText(TemplatePath);
            return content;
        }
        

    }
}
