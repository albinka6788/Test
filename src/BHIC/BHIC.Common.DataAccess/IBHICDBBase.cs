#region System references
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
#endregion

namespace BHIC.Common.DataAccess
{
    /// <summary>
    /// This is BHIC DB Helper Interface for executing any SQL query/stored procedure for updating/fetching data.
    /// </summary>
    public interface IBHICDBBase : IDisposable
    {

        #region Property
        string DBConnectionString { get; set; }
        #endregion

        #region Methods

        #region Public Methods

        #region Connection Members

        /// <summary>
        /// Open a connection if not opened already
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// Close a connection if already opened
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Begin a Transaction if not started earlier and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        void BeginTransaction(bool throwError);

        /// <summary>
        /// Begin a Transaction if not started earlier and throw error when connection not open
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit a Transaction if already in process and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        void CommitTransaction(bool throwError);

        /// <summary>
        /// Commit a Transaction if already in process and throw error transaction not in process or connection not open
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollback a Transaction if already in process and throw error if requested by consumer for non active connection
        /// </summary>
        /// <param name="throwError"></param>
        void RollbackTransaction(bool throwError);

        /// <summary>
        /// Rollback a Transaction if already in process and throw error transaction not in process or connection not open
        /// </summary>
        void RollbackTransaction();

        #endregion

        #region Execute Scalar Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return value received from database to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        object ExecuteScalar(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

        /// <summary>
        ///  Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        object ExecuteScalar(string sqlQuery);

        /// <summary>
        /// Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        object ExecuteScalar(string sqlQuery, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Execute Scalar
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        object ExecuteScalar(string sqlQuery, List<IDbDataParameter> sqlParams);


        #endregion

        #region Execute Non Query Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return status to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

        /// <summary>
        ///  Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlQuery);

        /// <summary>
        /// Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlQuery, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Execute Non Query
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlQuery, List<IDbDataParameter> sqlParams);

        #endregion

        #region Execute Reader Method and its Overload

        /// <summary>
        /// Execute Query/Stored Procedure and return Reader object to calling modules
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

        /// <summary>
        ///  Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlQuery);

        /// <summary>
        /// Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlQuery, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Execute Reader
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sqlQuery, List<IDbDataParameter> sqlParams);

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
        DataSet LoadDataSet(string sqlQuery, List<string> tableNames, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

        /// <summary>
        ///  Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, List<string> tableNames);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, List<string> tableNames, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableNames"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, List<string> tableNames, List<IDbDataParameter> sqlParams);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, string tableName);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, string tableName, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, QueryCommandType queryCommandType);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, List<IDbDataParameter> sqlParams);

        /// <summary>
        /// Overload of Load Data Set
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="tableName"></param>
        /// <param name="queryCommandType"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        DataSet LoadDataSet(string sqlQuery, string tableName, QueryCommandType queryCommandType, List<IDbDataParameter> sqlParams);

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
        int UpdateDataSet(DataSet dataSet, string tableName, IDbCommand insertCommand, IDbCommand updateCommand, IDbCommand deleteCommand);

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
        int UpdateDataSet(DataSet dataSet, string tableName, string insertSqlQuery, string updateSqlQuery, string deleteSqlQuery,
                                    QueryCommandType insertQueryCommandType, QueryCommandType updateQueryCommandType, QueryCommandType deleteQueryCommandType,
                                    List<IDbDataParameter> insertSqlParams, List<IDbDataParameter> updateSqlParams, List<IDbDataParameter> deleteSqlParams);

        #endregion

        #region Generate Single Parameter and its Overload

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size,
            ParameterDirection parameterDirection, bool isNullAble, byte precision, byte scale, string sourceColumn,
            DataRowVersion dataRowVersion, object parameterValue);


        IDbDataParameter GetSingleParameter(string parameterName, object parameterValue);

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, object parameterValue);

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, object parameterValue);

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, ParameterDirection parameterDirection, object parameterValue);

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, string sourceColumn);

        IDbDataParameter GetSingleParameter(string parameterName, ParameterType parameterType, int size, ParameterDirection parameterDirection, object parameterValue);


        #endregion

        #region Add Single Parameter and its Overload

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size,
            ParameterDirection parameterDirection, bool isNullAble, byte precision, byte scale, string sourceColumn,
            DataRowVersion dataRowVersion, object parameterValue);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, object parameterValue);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, object parameterValue);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, object parameterValue);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, ParameterDirection parameterDirection, object parameterValue);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, string sourceColumn);

        void AddSingleParameter(List<IDbDataParameter> sqlParams, string parameterName, ParameterType parameterType, int size, ParameterDirection parameterDirection, object parameterValue);



        #endregion

        #endregion

        #endregion

    }
}
