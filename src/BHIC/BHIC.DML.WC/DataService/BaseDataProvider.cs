#region Using directives

using BHIC.Common.DataAccess;
using System;

#endregion

namespace BHIC.DML.WC.DataService
{
    #region Methods

    #region Public Methods

    public class BaseDataProvider
    {
        /// <summary>
        /// Retuen generic DbConnector object to communicate with database
        /// </summary>
        /// <returns></returns>
        public BHICDBBase GetDbConnector()
        {
            BHICDBBase dbConnector = new BHICDBBase();

            #region Comment : Here using XmlReader get DB connection string

            dbConnector.DBName = "GuinnessDB";

            #endregion

            //dbConnector.DBConnectionString = DbConnectionString;
            //dbConnector.CreateDBObjects(Providers.SqlServer);
            //dbConnector.CreateConnection();

            return dbConnector;
        }

    }

    #endregion

    #endregion
}
