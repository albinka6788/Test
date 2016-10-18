using BHIC.Common.XmlHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BHIC.Common.Logging;

namespace BHIC.Common.Reattempt.Common
{
    /// <summary>
    /// Watch for the reattempt files and process it.
    /// </summary>
    public class ReattemptProcess
    {
        #region Methods
        #region Public Methods
        public void start()
        {
            /*Process the pending files in the dbreattempt folder path
                 * 
                 */
            #region Process Pending Files
            string reattemptFolderPath = ConfigCommonKeyReader.ReattemptFolderPath;// ConfigurationManager.AppSettings["ReattemptFolderPath"];
            var dir = new DirectoryInfo(reattemptFolderPath);
            FileInfo[] files = dir.GetFiles("*.json");
            foreach (FileInfo file in files)
            {
                StartReattempt(file.FullName, file.Name);
            }
            #endregion

            #region Watch for new file creation
            /* Watch for newly created files and process it. */
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = reattemptFolderPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch json files.
            watcher.Filter = "*.json";
            watcher.Created += new FileSystemEventHandler(GetFileInfo);
            watcher.EnableRaisingEvents = true;
            #endregion

            Console.ReadLine();
        }
        #endregion
        #region private methods
        /// <summary>
        /// Triggers on file creation in dbreattempt folder path through file watcher.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void GetFileInfo(object source, FileSystemEventArgs e)
        {
            StartReattempt(e.FullPath, e.Name);
        }
        /// <summary>
        /// Process the reattempt 
        /// </summary>
        /// <param name="fullPath"> File Full Path</param>
        /// <param name="fileName">File Name</param>
        private static void StartReattempt(string fullPath, string fileName)
        {
            /*Declaring variables*/
            #region Variable Declaration
            string archiveFolderPath = ConfigCommonKeyReader.ReattemptArchiveFolderPath; //ConfigurationManager.AppSettings["ReattemptArchiveFolderPath"];
            string failureFolderPath = ConfigCommonKeyReader.ReattemptFailureFolderPath;//.AppSettings["ReattemptFailureFolderPath"];
            ReattemptInfo info = null;
            Assembly coreAssembly;
            Assembly domainAssembly;
            Type classType;
            Type parameterType;
            object objClass;
            string className = "";
            string methodName = "";
            #endregion

            /*Deseralizing the json file to reattemptinfo object for reattempt process */
            #region Deserializing reattempt file
            info = JsonConvert.DeserializeObject<ReattemptInfo>(File.ReadAllText(fullPath));
            className = info.ClassName;
            methodName = info.MethodName;
            info.Log = new List<string>();
            string[] splMethodName = info.MethodName.Split('.');
            if (splMethodName.Length > 0)
            {
                methodName = splMethodName[splMethodName.Length - 1];
            }
            #endregion

            #region Reattempt Process Starts
            /* Creating instance and invoking method dynamically*/
            Logging.LoggingService.Instance.Trace(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.Core.dll"));
            coreAssembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.Core.dll"));
            domainAssembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.Domain.dll"));
            //coreAssembly = Assembly.LoadFrom("BHIC.Core.dll");
            //domainAssembly = Assembly.LoadFrom("BHIC.Domain.dll");
            classType = coreAssembly.GetType(className);

            if (classType != null)
            {
                objClass = coreAssembly.CreateInstance(className, true);
                MethodInfo methodInfo = objClass.GetType().GetMethod(methodName);
                Type[] interfaces = classType.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    methodInfo = interfaces[0].GetMethod(methodName);
                }
                try
                {
                    List<object> inputParameters = new List<object>();
                    int index = 0;
                    /*To convert the Json object to Class object respective to the parameter class type*/
                    foreach (object obj in info.InputParameters)
                    {
                        parameterType = domainAssembly.GetType(info.InputParametersTypeName[0]);
                        object objParamaterType = Activator.CreateInstance(parameterType);
                        //var rtn = GenericType(classType);
                        JsonConvert.PopulateObject(obj.ToString(), objParamaterType);
                        inputParameters.Add(objParamaterType);
                        index = index + 1;
                    }
                    var rtnVal = methodInfo.Invoke(objClass, inputParameters.ToArray());

                }
                catch (Exception ex)
                {
                    info.Log.Add(JsonConvert.SerializeObject(ex));
                    string reattemptJson = JsonConvert.SerializeObject(info);
                    /*On failure create a new file in failure folder with log*/
                    System.IO.File.WriteAllText(failureFolderPath + "\\" + fileName, reattemptJson);
                }
            }
            else
            {
                info.Log.Add("Invalid Class Name");
                string reattemptJson = JsonConvert.SerializeObject(info);
                /*On failure create a new file in failure folder with log*/
                System.IO.File.WriteAllText(failureFolderPath + "\\" + fileName, reattemptJson);
            }
            /*Once processed, move the files to archive folder*/
            File.Move(fullPath, archiveFolderPath + "\\" + fileName);
            #endregion

        }

        public static T GenericType<T>(T obj) where T : class
        {
            var type = obj.GetType();
            return Activator.CreateInstance(type) as T;
        }

        #endregion
        #endregion

    }

}
