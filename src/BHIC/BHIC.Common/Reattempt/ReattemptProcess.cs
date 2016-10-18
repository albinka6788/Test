using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BHIC.Common.XmlHelper;

using Newtonsoft.Json;

namespace BHIC.Common.Reattempt
{
    /// <summary>
    /// Watch for the reattempt files and process it.
    /// </summary>
    public class ReattemptProcess
    {
        #region Private variables
        string archiveFolderPath = ConfigCommonKeyReader.ReattemptArchiveFolderPath; //ConfigurationManager.AppSettings["ReattemptArchiveFolderPath"];
        string failureFolderPath = ConfigCommonKeyReader.ReattemptFailureFolderPath;//.AppSettings["ReattemptFailureFolderPath"];
        #endregion

        #region Methods
        #region Public Methods
        public void start()
        {
            try
            {

                /*Process the pending files in the dbreattempt folder path*/
                #region Process Pending Files
                string reattemptFolderPath = ConfigCommonKeyReader.ReattemptFolderPath;// ConfigurationManager.AppSettings["ReattemptFolderPath"];
                if (!string.IsNullOrEmpty(archiveFolderPath) && !Directory.Exists(archiveFolderPath))
                {
                    Directory.CreateDirectory(archiveFolderPath);
                }
                if (!string.IsNullOrEmpty(failureFolderPath) && !Directory.Exists(failureFolderPath))
                {
                    Directory.CreateDirectory(failureFolderPath);
                }
                if (!string.IsNullOrEmpty(reattemptFolderPath) && Directory.Exists(reattemptFolderPath))
                {
                    var dir = new DirectoryInfo(reattemptFolderPath);
                    FileInfo[] files = dir.GetFiles("*.json");
                    foreach (FileInfo file in files)
                    {
                        StartReattempt(file.FullName, file.Name);
                    }
                }
                else
                {
                    Logging.LoggingService.Instance.Fatal(string.Format("{0} in the configuration file", (string.IsNullOrEmpty(reattemptFolderPath) ? 
                        "Reattempt folder name not specified" : 
                        string.Format("Specified invalid Reattempt folder name i.e. {0},", reattemptFolderPath))));
                }
                #endregion

                #region Watch for new file creation
                ///* Watch for newly created files and process it. */
                //FileSystemWatcher watcher = new FileSystemWatcher();
                //watcher.Path = reattemptFolderPath;
                //watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                //// Only watch json files.
                //watcher.Filter = "*.json";
                //watcher.Created += new FileSystemEventHandler(GetFileInfo);
                //watcher.EnableRaisingEvents = true;
                #endregion

                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logging.LoggingService.Instance.Trace(ex.Message);
                throw;
            }

        }
        #endregion
        #region private methods
        /// <summary>
        /// Triggers on file creation in dbreattempt folder path through file watcher.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void GetFileInfo(object source, FileSystemEventArgs e)
        {
            StartReattempt(e.FullPath, e.Name);
        }
        /// <summary>
        /// Process the reattempt 
        /// </summary>
        /// <param name="fullPath"> File Full Path</param>
        /// <param name="fileName">File Name</param>
        private void StartReattempt(string fullPath, string fileName)
        {
            try
            {

                /*Declaring variables*/
                #region Variable Declaration
                ReattemptInfo info = null;
                Assembly coreAssembly;
                Assembly domainAssembly;
                Assembly domainDTOAssembly;
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
                coreAssembly = Assembly.Load(new AssemblyName("BHIC.Core")); //Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.Core.dll"));
                domainAssembly = Assembly.Load(new AssemblyName("BHIC.Domain"));//Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.Domain.dll"));
                domainDTOAssembly = Assembly.Load(new AssemblyName("BHIC.DML.WC"));// Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BHIC.DML.WC.dll"));
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

                            if (!obj.GetType().ToString().Contains("System"))
                            {
                                parameterType = domainAssembly.GetType(info.InputParametersTypeName[index]);
                                if (parameterType == null)
                                    parameterType = domainDTOAssembly.GetType(info.InputParametersTypeName[index]);

                                if (parameterType == null)
                                    parameterType = obj.GetType();

                                object objParamaterType = Activator.CreateInstance(parameterType);
                                JsonConvert.PopulateObject(obj.ToString(), objParamaterType);
                                inputParameters.Add(objParamaterType);
                            }
                            else
                            {
                                inputParameters.Add(obj);
                            }

                            index = index + 1;
                        }
                        var rtnVal = methodInfo.Invoke(objClass, inputParameters.ToArray());

                        try
                        {
                            File.Delete(fullPath);
                        }
                        catch (Exception ex)
                        {
                            Logging.LoggingService.Instance.Fatal(string.Format("Unable to delete file '{0}' due to following issue{1}{2}", fullPath, Environment.NewLine, ex.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        info.Log.Add(JsonConvert.SerializeObject(ex));
                        /*On failure create a new file in failure folder with log*/
                        WriteToFile(failureFolderPath, fileName, JsonConvert.SerializeObject(info));
                    }
                }
                else
                {
                    info.Log.Add("Invalid Class Name");
                    /*On failure create a new file in failure folder with log*/
                    WriteToFile(failureFolderPath, fileName, JsonConvert.SerializeObject(info));
                }

                /*Once processed, move the files to archive folder*/
                // Commented below code as not requried now i.e. archive of file after processing.
                //if (!string.IsNullOrEmpty(archiveFolderPath))
                //{
                //    File.Move(fullPath, archiveFolderPath + "\\" + fileName);
                //}
                #endregion
            }
            catch (Exception)
            {

                throw;
            }

        }

        private T GenericType<T>(T obj) where T : class
        {
            var type = obj.GetType();
            return Activator.CreateInstance(type) as T;
        }

        /// <summary>
        /// Write to File
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="reattemptJson"></param>
        private void WriteToFile(string folderName, string fileName, string reattemptJson)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                System.IO.File.WriteAllText(folderName + "\\" + fileName, reattemptJson);
            }

        }
        #endregion
        #endregion

    }

}
