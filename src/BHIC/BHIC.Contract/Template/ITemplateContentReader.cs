using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHIC.Contract.Template
{
    public interface ITemplateContentReader
    {
        string GetContent(string TemplatePath);
    }
}
