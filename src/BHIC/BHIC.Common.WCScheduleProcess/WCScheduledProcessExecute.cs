#region Using directives

using System;
using System.Reflection;

#endregion

namespace BHIC.Common.WCScheduleProcess
{
    public class WCScheduledProcessExecute
    {
        public void Execute(string processName)
        {
            Assembly currentAssembly;
            Type backGroundProcess;
            object objClass;

            try
            {
                string className = "BHIC.Common.WCScheduleProcess." + processName;

                currentAssembly = Assembly.GetExecutingAssembly();

                backGroundProcess = currentAssembly.GetType(className);

                if (backGroundProcess != null)
                {
                    objClass = currentAssembly.CreateInstance(className);

                    bool objReturn = (bool)backGroundProcess.InvokeMember("Process", BindingFlags.Default | BindingFlags.InvokeMethod, null, objClass, null);

                    if (!objReturn)
                    {
                        throw new Exception(string.Format("Some error occurred while executing {0} Service. Please check Log Table for detail error.", processName));
                    }
                }
                else
                {
                    throw new Exception("Invalid Process Name. Please provide correct process name in the argument.");
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                objClass = null;
                backGroundProcess = null;
                currentAssembly = null;
            }
        }
    }
}
