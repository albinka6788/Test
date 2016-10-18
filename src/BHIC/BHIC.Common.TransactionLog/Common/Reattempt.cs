using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHIC.Domain.Policy;
using Newtonsoft.Json;
using BHIC.Common.XmlHelper;
using System.Reflection;
using System.IO;

namespace BHIC.Common.TransactionLog.Common
{
    /// <summary>
    /// Reattempt Register to log db/api failures for retry through background application(Service).
    /// Register will get all required information from the methodbase and will customize into ReattemptInfo class.
    /// After that seralize the reattemptinfo to json, then store the file in the configured path(Based on DB/API)
    /// 
    ///   
    /// </summary>
    public static class ReattemptLog
    {
        #region Methods
        #region Public Methods
        /*For DB & API Failures*/
        public static void Register(MethodBase serviceCall,params object[] inputParameters)
        {
            CreateReattemptFile(serviceCall, inputParameters);
        }
        /*For API Failures*/
        #endregion
        #region Private Methods
        /*Form the reattempinfo object, serialize and store it in a json file*/
        private static void CreateReattemptFile(MethodBase serviceCall, object[] inputParams)
        {
            try
            {
                string serviceName = serviceCall.DeclaringType.AssemblyQualifiedName;
                List<string> inputParametersTypeName = new List<string>();
                foreach(object obj in inputParams)
                {
                    inputParametersTypeName.Add(obj.GetType().FullName);
                }
                

                ReattemptInfo info = new ReattemptInfo();
                info.CreatedDateTime = System.DateTime.Now;
                info.InputParameters = inputParams.ToArray();
                info.MethodName = serviceCall.Name;
                info.ClassName = serviceCall.ReflectedType.FullName;
                info.InputParametersTypeName = inputParametersTypeName;

                string reattemptJson = JsonConvert.SerializeObject(info);
                string fileName = System.Guid.NewGuid().ToString() + ".json";
                string folderPath = "";

               
                folderPath = ConfigCommonKeyReader.ReattemptFolderPath + "/";
                System.IO.File.WriteAllText(folderPath + fileName, reattemptJson);
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Fatal(ex);
            }
        }
        #endregion
        #endregion
    }

    
   
}
