using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHIC.Domain.Background;
using BHIC.Common.Client;

namespace BHIC.Contract.Background
{
    public interface IClassDescriptionService
    {
        /// <summary>
        /// Return list of classdescriptions based on Lob, SubIndustryId and ClassDescriptionId
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<ClassDescription> GetClassDescriptionList(ClassDescriptionRequestParms args,ServiceProvider Provider);

        /// <summary>
        /// Return specific ClassDescription detail based on ClassDescriptionId
        /// </summary>
        /// <param name="classDescriptionId"></param>
        /// <returns></returns>
        ClassDescription GetClassDescription(int classDescriptionId, ServiceProvider Provider);
    }
}
