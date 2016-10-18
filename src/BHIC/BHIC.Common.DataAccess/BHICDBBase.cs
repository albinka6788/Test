#region System references
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
#endregion

#region BHIC References
using BHIC.Common;
//using System.Data.OracleClient;
using System.Data.Common;
using BHIC.Common.Config;
using BHIC.Common.XmlHelper;
using BHIC.Common.CommonUtilities;
using BHIC.Common.Reattempt;
#endregion

namespace BHIC.Common.DataAccess
{
    public class BHICDBBase : IBHICDBBase
    {

        #region Variables

        IDbConnection connection = null;
        IDbTransaction transaction = null;
        //IDbCommand command = null;
        DbProviderFactory factory = null;
        Providers provider = Providers.SqlServer;
        string dbName = ConfigCommonKeyReader.DbKeyName;
        bool isOutputParameterExist = false;
        bool isRefCursorParameterExists = false;

        #endregion

        #region Property
        public string DBConnectionString { get; set; }

        /// <summary>
        /// Connection Object
        /// </summary>
        public IDbConnection DBConnection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        /// <summary>
        /// Transaction Object
        /// </summary>
        //public IDbTransaction DBTransaction
        //{
        //    get
        //    {
        //        return transaction;
        //    }
        //    set
        //    {
        //        transaction = value;
        //    }
        //}

        ///// <summary>
        ///// Command Object
        ///// </summary>
        //public IDbCommand DbCommand
        //{
        //    get
        //    {
        //        return command;
        //    }
        //    set
        //    {
        //        command = value;
        //    }
        //}

        /// <summary>
        /// Database Name of which the settings will be used
        /// </summary>
        public string DBName
        {
            get
            {
                return dbName;
            }
            set
            {
                dbName = value;
            }
        }

        #endregion

        #region Methods


        #region Public Methods

        #region Connection Members

        /// <summary>
        /// Create Database Objects on the basis of the type of provider
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="providerList"></param>
        public void CreateDBObjects(Providers providerHandle)
        {
            //CreateDBObjects(connectString, providerList, null);
            if (factory.IsNull())
                switch (providerHandle)
                {
                    //case Providers.Oracle:
                    //    provider = Providers.Oracle;
                    //    factory = OracleClientFactory.Instance;
                    //    break;
                    case Providers.OleDB:
                        provider = Providers.OleDB;
                        factory = OleDbFactory.Instance;
                        break;
                    case Providers.ODBC:
                        provider = Providers.ODBC;
                        factory = OdbcFactory.Instance;
                        break;
                    case Providers.SqlServer:
                    default:
                        provider = Providers.SqlServer;
                        factory = SqlClientFactory.Instance;
                        break;

                }
        }
        /// <summary>
        /// Create connection from the factory instance
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            CreateDBObjects(provider);
            connection = factory.CreateConnection();
            connection.ConnectionString = DBConnectionString;
            return connection;
        }

        
        /// <summary>
        /// Open a connection if not opened already
        /// </summary>
        public void OpenConnection()
        {
            if (connection == null)
            {
                if (string.IsNullOrEmpty(DBConnectionString) || string.IsNullOrWhiteSpace(DBConnectionString))
                {
                    GetConnectionString(DBName);
                }
                connection = CreateConnection();
            }
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Close a connection if already opened
        /// </summary>
        public void CloseConnection()
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    if (transaction != null)
                    {
                        RollbackTransaction(false);
                    }
                    connection.Close();
                }
                connection.Dispose();
                connection = null;
                GC.Collect(GC.MaxGeneration);
            }
        }

        /// <summary>
        /// Begin a Transaction if not started earlier and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        public void BeginTransaction(bool throwError)
        {
            string errorMessage = "";

            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    if (transaction == null)
                    {
                        transaction = connection.BeginTransaction();
                    }
                }
                else
                {
                    errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
                }
            }
            else
            {
                errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
            }

            if (throwError && !string.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Begin a Transaction if not started earlier and throw error when connection not open
        /// </summary>
        public void BeginTransaction()
        {
            this.BeginTransaction(true);
        }

        /// <summary>
        /// Commit a Transaction if already in process and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        public void CommitTransaction(bool throwError)
        {
            string errorMessage = "";

            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                        transaction = null;
                    }
                    else
                    {
                        errorMessage = Constants.DatabaseErrors.START_TRANSACTION_COMMIT;
                    }
                }
                else
                {
                    errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
                }
            }
            else
            {
                errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
            }

            if (throwError && !string.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Commit a Transaction if already in process and throw error transaction not in process or connection not open
        /// </summary>
        public void CommitTransaction()
        {
            this.CommitTransaction(true);
        }

        /// <summary>
        /// Rollback a Transaction if already in process and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        public void RollbackTransaction(bool throwError)
        {
            string errorMessage = "";
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                        transaction = null;
                    }
                    else
                    {
                        errorMessage = Constants.DatabaseErrors.START_TRANSACTION_REVERT;
                    }
                }
                else
                {
                    errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
                }
            }
            else
            {
                errorMessage = Constants.DatabaseErrors.OPEN_CONNECTION;
            }
            if (throwError && !string.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Rollback a Transaction if already in process and throw error transaction not in process or connection not open
        /// </summary>
        public void RollbackTransaction()
        {
            this.RollbackTransaction(true);
        }

        #endregion

        #region Execute Scalar Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return value received from database to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            object objRetValue;
            IDbCommand cmd = null;

            try
            {
                using (cmd = GetDBCommand(sqlQuery, queryCommandType, sqlParams))
                {
                    objRetValue = cmd.ExecuteScalar();

                    // Setting return output parameter type parameter values back to SQL Parameter collection.
                    GetParametersOutputValues(cmd, sqlParams);

                    CleanUpRefCursors(cmd, sqlParams);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CleanUpRefCursors(cmd, sqlParams);
            }

            return objRetValue;
        }

        /// <summary>
        ///  Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlQuery)
        {
            return this.ExecuteScalar(sqlQuery, QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlQuery, QueryCommandType queryCommandType)
        {
            return this.ExecuteScalar(sqlQuery, queryCommandType, null);
        }

        /// <summary>
        /// Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlQuery, List<IDbDataParameter> sqlParams)
        {
            return this.ExecuteScalar(sqlQuery, QueryCommandType.Text, sqlParams);
        }


        #endregion

        #region Execute Non Query Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return status to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            int iRetValue;
            IDbCommand cmd = null;

            try
            {
                using (cmd = GetDBCommand(string.Format("{0}{1}", AppSettings.SchemaName, sqlQuery), queryCommandType, sqlParams))
                {
                    iRetValue = cmd.ExecuteNonQuery();

                    // Setting return output parameter type parameter values back to SQL Parameter collection.
                    GetParametersOutputValues(cmd, sqlParams);
                    CleanUpRefCursors(cmd, sqlParams);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CleanUpRefCursors(cmd, sqlParams);
            }

            return iRetValue;
        }

        /// <summary>
        ///  Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            return this.ExecuteNonQuery(sqlQuery, QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery, QueryCommandType queryCommandType)
        {
            return this.ExecuteNonQuery(sqlQuery, queryCommandType, null);
        }

        /// <summary>
        /// Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlQuery, List<IDbDataParameter> sqlParams)
        {
            return this.ExecuteNonQuery(sqlQuery, QueryCommandType.Text, sqlParams);
        }

        #endregion

        #region Execute Reader Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return Reader object to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            IDataReader dr;
            IDbCommand cmd = null;

            try
            {
                using (cmd = GetDBCommand(string.Format("{0}{1}", AppSettings.SchemaName, sqlQuery), queryCommandType, sqlParams))
                {
                    // Executing command to generate Data Reader object
                    dr = cmd.ExecuteReader();

                    // Setting return output parameter type parameter values back to SQL Parameter collection.
                    GetParametersOutputValues(cmd, sqlParams);

                    CleanUpRefCursors(cmd, sqlParams);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CleanUpRefCursors(cmd, sqlParams);
            }

            return dr;
        }

        /// <summary>
        ///  Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sqlQuery)
        {
            return this.ExecuteReader(sqlQuery, QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sqlQuery, QueryCommandType queryCommandType)
        {
            return this.ExecuteReader(sqlQuery, queryCommandType, null);
        }

        /// <summary>
        /// Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sqlQuery, List<IDbDataParameter> sqlParams)
        {
            return this.ExecuteReader(sqlQuery, QueryCommandType.Text, sqlParams);
        }

        #endregion

        #region Load Data Set Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return Data Set to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, List<string> tableNames, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            DataSet ds = new DataSet();
            IDbCommand cmd = null;
            try
            {
                using (cmd = GetDBCommand(string.Format("{0}{1}", AppSettings.SchemaName, sqlQuery), queryCommandType, sqlParams))
                {
                    // Executing command to fill dataset from the database
                    DbDataAdapter oda = GetDataAdapter(cmd);

                    oda.Fill(ds);

                    for (int i = 0; i < tableNames.Count(); i++)
                    {
                        if (!(string.IsNullOrEmpty(tableNames[i]) || string.IsNullOrWhiteSpace(tableNames[i])) && (i < ds.Tables.Count))
                        {
                            ds.Tables[i].TableName = tableNames[i];
                        }
                    }

                    // Setting return output parameter type parameter values back to SQL Parameter collection.
                    GetParametersOutputValues(cmd, sqlParams);

                    CleanUpRefCursors(cmd, sqlParams);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CleanUpRefCursors(cmd, sqlParams);
            }

            return ds;
        }

        /// <summary>
        ///  Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, List<string> tableNames)
        {
            return this.LoadDataSet(sqlQuery, tableNames, QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, List<string> tableNames, QueryCommandType queryCommandType)
        {
            return this.LoadDataSet(sqlQuery, tableNames, queryCommandType, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, List<string> tableNames, List<IDbDataParameter> sqlParams)
        {
            return this.LoadDataSet(sqlQuery, tableNames, QueryCommandType.Text, sqlParams);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery)
        {
            return this.LoadDataSet(sqlQuery, "", QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, string tableName)
        {
            return this.LoadDataSet(sqlQuery, tableName, QueryCommandType.Text, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, string tableName, QueryCommandType queryCommandType)
        {
            return this.LoadDataSet(sqlQuery, tableName, queryCommandType, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, QueryCommandType queryCommandType)
        {
            return this.LoadDataSet(sqlQuery, "", queryCommandType, null);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            return this.LoadDataSet(sqlQuery, "", queryCommandType, sqlParams);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, List<IDbDataParameter> sqlParams)
        {
            return this.LoadDataSet(sqlQuery, "", QueryCommandType.Text, sqlParams);
        }

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet LoadDataSet(string sqlQuery, string tableName, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrWhiteSpace(tableName))
            {
                tableName = "DataTable1";
            }
            List<string> tableNames = new List<string>();
            tableNames.Add(tableName);
            return this.LoadDataSet(sqlQuery, tableNames, queryCommandType, sqlParams);
        }

        #endregion

        #region Update Data Set Method and its Overload

        /// <summary>
        /// Update dataset data into database and return status back to calling application
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="insertCommand"></param>
        /// <param name="updateCommand"></param>
        /// <param name="deleteCommand"></param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet dataSet, string tableName, IDbCommand insertCommand, IDbCommand updateCommand, IDbCommand deleteCommand)
        {
            int retValue;

            // Executing command to fill dataset from the database
            CreateDBObjects(provider);
            DbDataAdapter oda = factory.CreateDataAdapter();
            oda.InsertCommand = (DbCommand)insertCommand;
            oda.UpdateCommand = (DbCommand)updateCommand;
            oda.DeleteCommand = (DbCommand)deleteCommand;
            retValue = oda.Update(dataSet);

            return retValue;
        }

        /// <summary>
        ///  Overload of Update Data Set
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="insertSqlQuery"></param>
        /// <param name="updateSqlQuery"></param>
        /// <param name="deleteSqlQuery"></param>
        /// <param name="insertQueryCommandType"></param>
        /// <param name="updateQueryCommandType"></param>
        /// <param name="deleteQueryCommandType"></param>
        /// <param name="insertSqlParams"></param>
        /// <param name="updateSqlParams"></param>
        /// <param name="deleteSqlParams"></param>
        /// <returns></returns>
        public int UpdateDataSet(DataSet dataSet, string tableName, string insertSqlQuery, string updateSqlQuery, string deleteSqlQuery,
                                    QueryCommandType insertQueryCommandType, QueryCommandType updateQueryCommandType, QueryCommandType deleteQueryCommandType,
                                    List<IDbDataParameter> insertSqlParams, List<IDbDataParameter> updateSqlParams, List<IDbDataParameter> deleteSqlParams)
        {
            IDbCommand insertCommand = GetDBCommand(insertSqlQuery, insertQueryCommandType, insertSqlParams);
            IDbCommand updateCommand = GetDBCommand(updateSqlQuery, updateQueryCommandType, updateSqlParams);
            IDbCommand deleteCommand = GetDBCommand(deleteSqlQuery, deleteQueryCommandType, deleteSqlParams);
            return UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand);
        }

        #endregion

        #region Generate Single Parameter

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size,
            ParameterDirection parameterDirection, bool isNullAble, byte precision, byte scale, string sourceColumn,
            DataRowVersion dataRowVersion, object parameterValue)
        {
            //return new OracleParameter(parameterName, (OracleDbType)parameterType, size, parameterDirection, isNullAble,
            //    precision, scale, sourceColumn, dataRowVersion, parameterValue);
            CreateDBObjects(provider);
            DbParameter param = factory.CreateParameter();
            param.ParameterName = parameterName;
            param.Size = size;
            param.Direction = parameterDirection;
            param.SourceColumnNullMapping = isNullAble;
            param.Value = parameterValue;
            param.SourceVersion = dataRowVersion;
            param.SourceColumn = sourceColumn;
            param.DbType = (DbType)parameterType;

            return param;
        }

        public IDbDataParameter GetSingleParameter(string parameterName, object parameterValue)
        {
            return GetSingleParameter(parameterName, ParameterType.NVarchar2, 0, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, object parameterValue)
        {
            return GetSingleParameter(parameterName, parameterType, 0, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, object parameterValue)
        {
            return GetSingleParameter(parameterName, parameterType, size, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, ParameterDirection parameterDirection, object parameterValue)
        {
            return GetSingleParameter(parameterName, parameterType, 0, parameterDirection, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, string sourceColumn)
        {
            return GetSingleParameter(parameterName, parameterType, size, ParameterDirection.Input, true, 0, 0, sourceColumn, DataRowVersion.Default, null);
        }

        public IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, ParameterDirection parameterDirection, object parameterValue)
        {
            return GetSingleParameter(parameterName, parameterType, size, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        #endregion

        #region Add Single Parameter

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size,
            ParameterDirection parameterDirection, bool isNullAble, byte precision, byte scale, string sourceColumn,
            DataRowVersion dataRowVersion, object parameterValue)
        {
            sqlParams.Add(GetSingleParameter(parameterName, parameterType, size, parameterDirection, isNullAble,
                precision, scale, sourceColumn, dataRowVersion, parameterValue));
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, object parameterValue)
        {
            AddSingleParameter(sqlParams, parameterName, ParameterType.NVarchar2, 0, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, object parameterValue)
        {
            AddSingleParameter(sqlParams, parameterName, parameterType, 0, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, object parameterValue)
        {
            AddSingleParameter(sqlParams, parameterName, parameterType, size, ParameterDirection.Input, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, ParameterDirection parameterDirection, object parameterValue)
        {
            AddSingleParameter(sqlParams, parameterName, parameterType, 0, parameterDirection, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, string sourceColumn)
        {
            AddSingleParameter(sqlParams, parameterName, parameterType, size, ParameterDirection.Input, true, 0, 0, sourceColumn, DataRowVersion.Default, null);
        }

        public void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, ParameterDirection parameterDirection, object parameterValue)
        {
            AddSingleParameter(sqlParams, parameterName, parameterType, size, parameterDirection, true, 0, 0, "", DataRowVersion.Default, parameterValue);
        }

        public void Dispose()
        {
            CloseConnection();            
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Other useful methods

        //public DateTime ConvertOracleDate(object value)
        //{
        //    switch (provider)
        //    {
        //        case Providers.SqlServer:
        //            return (DateTime)value;
        //            break;
        //        case Providers.Oracle:
        //            return (DateTime)((OracleDateTime)value);
        //            break;
        //        case Providers.OleDB:
        //            return (DateTime)((OleDbType.Date)value);
        //            break;
        //        case Providers.ODBC:
        //            return (DateTime)((OracleDate)value);
        //            break;
        //    }
        //    return (DateTime)((OracleDate)value);
        //}
        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Xml Parameter to set
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private DbParameter GetXmlParameter(string xml)
        {
            CreateDBObjects(provider);
            DbParameter XmlParam = factory.CreateParameter();
            XmlParam.DbType = DbType.Xml;
            XmlParam.Value = xml;
            return XmlParam;
        }

        /// <summary>
        /// Create Command for the Type of Provider
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private IDbCommand CreateCommand(string query, IDbConnection connection)
        {
            switch (provider)
            {
                //case Providers.Oracle:
                //    return new OracleCommand(query, (OracleConnection)connection);
                case Providers.OleDB:
                    return new OleDbCommand(query, (OleDbConnection)connection);
                case Providers.ODBC:
                    return new OdbcCommand(query, (OdbcConnection)connection);
                case Providers.SqlServer:
                default:
                    return new SqlCommand(query, (SqlConnection)connection);
            }
        }

        /// <summary>
        /// Creating command object based on specified Parametere
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        private IDbCommand GetDBCommand(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams)
        {
            IDbCommand cmd = null;
            CreateDBObjects(provider);
            OpenConnection();
            cmd = CreateCommand(sqlQuery, connection);
            if (ConfigCommonKeyReader.IsTransactionLog && HttpContext.Current!=null)
            {
                TransactionLogCustomSessions.CustomSessionForDbRequest(sqlQuery);                
            }
            switch (queryCommandType)
            {
                case QueryCommandType.Text:
                    cmd.CommandType = CommandType.Text;
                    break;
                case QueryCommandType.StoredProcedure:
                    cmd.CommandType = CommandType.StoredProcedure;
                    break;
            }

            cmd.CommandTimeout = ConfigCommonKeyReader.DBCommandTimeOut;

            if (transaction != null)
            {
                cmd.Transaction = (IDbTransaction)transaction;
            }

            isOutputParameterExist = false;
            isRefCursorParameterExists = false;

            if (sqlParams != null)
            {
                int i = 0;
                foreach (IDbDataParameter sqlParam in sqlParams)
                {
                    DbParameter sqlParamClone = (DbParameter)((ICloneable)sqlParams[i]).Clone();
                    if (!isOutputParameterExist &&
                        (sqlParamClone.Direction == ParameterDirection.Output || sqlParamClone.Direction == ParameterDirection.InputOutput || sqlParamClone.Direction == ParameterDirection.ReturnValue))
                    {
                        isOutputParameterExist = true;
                    }
                    if (!isRefCursorParameterExists)
                        if (provider == Providers.Oracle || provider == Providers.OleDB)
                            if (((OracleParameter)sqlParamClone).OracleType == OracleType.Cursor)//|| ((OdbcParameter)sqlParamClone).OdbcType == OdbcType.
                            {
                                isRefCursorParameterExists = true;
                            }
                    if (sqlParamClone.DbType == DbType.Xml && sqlParamClone.Direction != ParameterDirection.Output &&
                        sqlParamClone.Direction != ParameterDirection.ReturnValue)
                    {
                        string xmlData = sqlParamClone.Value.ToString();
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            sqlParamClone.Value = GetXmlParameter(xmlData);
                        }
                    }
                    cmd.Parameters.Add(sqlParamClone);

                    i++;
                }
            }

            return cmd;
        }

        /// <summary>
        /// Setting return Output value for output parameters into SQL Parameter Collection
        /// only when command object has any output parameter
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlParams"></param>
        private void GetParametersOutputValues(IDbCommand cmd, List<IDbDataParameter> sqlParams)
        {
            // Checking whether last Command executing has any output type parameter.
            if (!isOutputParameterExist)
            {
                return;
            }

            int i = 0;
            foreach (IDbDataParameter sqlParam in cmd.Parameters)
            {
                if (sqlParam.Direction == ParameterDirection.InputOutput || sqlParam.Direction == ParameterDirection.Output || sqlParam.Direction == ParameterDirection.ReturnValue)
                {
                    if (sqlParam.DbType == DbType.String) // For Clob Data type in Oracle
                    {
                        sqlParams[i].Value = Convert.ToString(sqlParam.Value);
                    }
                    else
                    {
                        sqlParams[i].Value = sqlParam.Value;
                    }
                }
                i++;
            }
        }

        /// <summary>
        /// Cleanup Oracle Ref cursors
        /// </summary>
        /// <param name="cmd"></param>
        private void CleanUpRefCursors(IDbCommand cmd, List<IDbDataParameter> sqlParams)
        {
            if (ConfigCommonKeyReader.IsTransactionLog && HttpContext.Current != null)
            {
                TransactionLogCustomSessions.CustomSessionForDbResponse();
            }
            if (!isRefCursorParameterExists)
            {
                return;
            }

            if (cmd != null)
            {
                int i = 0;
                foreach (IDbDataParameter sqlParam in cmd.Parameters)
                {
                    if (((OracleParameter)sqlParam).OracleType == OracleType.Cursor)
                    {
                        ((IDisposable)sqlParam).Dispose();
                        ((IDisposable)sqlParams[i]).Dispose();
                    }
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < sqlParams.Count; i++)
                {
                    if (((OracleParameter)sqlParams[i]).OracleType == OracleType.Cursor)
                    {
                        ((IDisposable)sqlParams[i]).Dispose();
                    }
                }
            }
            isRefCursorParameterExists = false;
        }

        /// <summary>
        /// Get Connection String on the basis of Database Name
        /// </summary>
        /// <param name="dbAppSettingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private void GetConnectionString(string dbAppSettingName, string defaultValue = "")
        {
            bool decryptValue = true;

            if (string.IsNullOrWhiteSpace(dbAppSettingName) && string.IsNullOrWhiteSpace(defaultValue))
            {
                throw new ArgumentNullException("Please provide detail for Database that store application data");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(dbAppSettingName) && !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[dbAppSettingName]))
                {
                    defaultValue = ConfigurationManager.AppSettings[dbAppSettingName];
                }
            }

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["IsDBConnEncrypted"]))
            {
                decryptValue = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDBConnEncrypted"]);
            }

            DBConnectionString = (decryptValue ? Encryption.DecryptText(defaultValue) : defaultValue);
        }

        /// <summary>
        /// Get Data adapter for a particular provider
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private DbDataAdapter GetDataAdapter(IDbCommand command)
        {
            switch (provider)
            {
                //case Providers.Oracle:
                //    return new OracleDataAdapter((OracleCommand)(command));
                case Providers.OleDB:
                    return new OleDbDataAdapter((OleDbCommand)(command));
                case Providers.ODBC:
                    return new OdbcDataAdapter((OdbcCommand)(command));
                case Providers.SqlServer:
                default:
                    return new SqlDataAdapter((SqlCommand)(command));
            }
        }
        #endregion

        #endregion
    }
}