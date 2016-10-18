using BHIC.Common.DataAccess;
using BHIC.DML.WC.DataContract;
using BHIC.DML.WC.DTO;
using BHIC.Domain.LP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BHIC.DML.WC.DataService
{
    public class LandingPageDataService : BaseDataProvider, ILandingPageDataService
    {
        #region Public Method

        public List<LineOfBusiness> GetAllLineOfBusiness()
        {
            List<LineOfBusiness> lob = new List<LineOfBusiness>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetLineOfBusiness", QueryCommandType.StoredProcedure);

            while (rdr.Read())
            {
                lob.Add(new LineOfBusiness { Id = Convert.ToInt32(rdr["LobId"]), Abbreviation = Convert.ToString(rdr["Abbreviation"]), StateCode = Convert.ToString(rdr["StateCode"]) });

            }
            return lob;
        }


        /*Use to insert data related to landing page*/
        public long AddEditLandingPage(LandingPageTransaction landingPageTransaction, long userId)
        {

            long landingPageTransactionId = 0;

            List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@id", Value = landingPageTransaction.Id, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@lob", Value = landingPageTransaction.lob, SqlDbType = SqlDbType.VarChar,Size=3 } ,
                                        new SqlParameter() { ParameterName = "@state", Value = landingPageTransaction.State, SqlDbType = SqlDbType.VarChar,Size=3  } ,
                                        new SqlParameter() { ParameterName = "@transactionCounter", Value = landingPageTransaction.TransactionCounter, SqlDbType = SqlDbType.Int  ,IsNullable=true},

                                        new SqlParameter() { ParameterName = "@pageName", Value =landingPageTransaction.PageName, SqlDbType = SqlDbType.VarChar} ,
                                        new SqlParameter() { ParameterName = "@heading", Value =landingPageTransaction.Heading, SqlDbType = SqlDbType.VarChar} ,
                                        new SqlParameter() { ParameterName = "@subheading", Value =landingPageTransaction.SubHeading, SqlDbType = SqlDbType.VarChar} ,
                                        new SqlParameter() { ParameterName = "@productName", Value =landingPageTransaction.ProductName, SqlDbType = SqlDbType.VarChar} ,
                                        new SqlParameter() { ParameterName = "@productHighlight", Value =landingPageTransaction.ProductHighlight, SqlDbType = SqlDbType.VarChar} ,
                                        new SqlParameter() { ParameterName = "@btnText", Value =landingPageTransaction.BTNText, SqlDbType = SqlDbType.VarChar,Size=100} ,
                                        new SqlParameter() { ParameterName = "@calloutText", Value =landingPageTransaction.CalloutText, SqlDbType = SqlDbType.VarChar} ,

                                        new SqlParameter() { ParameterName = "@mainImage", Value = "/LandingPage/Images/MainImage/" + landingPageTransaction.MainImage ,SqlDbType = SqlDbType.VarChar,Size=100 } ,
                                        new SqlParameter() { ParameterName = "@templateId", Value = landingPageTransaction.TemplateId, SqlDbType = SqlDbType.Int,IsNullable=true} ,
                                       
                                        new SqlParameter() { ParameterName = "@controller", Value = landingPageTransaction.Controller,SqlDbType = SqlDbType.VarChar,Size=50 ,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@actionResult", Value = landingPageTransaction.ActionResult, SqlDbType = SqlDbType.VarChar,Size=50 ,IsNullable=true } ,

                                        new SqlParameter() { ParameterName = "@tokenId", Value = landingPageTransaction.TokenId, SqlDbType = SqlDbType.VarChar,Size=32}, 

                                        new SqlParameter() { ParameterName = "@createdBy", Value = userId, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@updatedBy", Value = userId, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@isDeployed", Value = landingPageTransaction.IsDeployed, SqlDbType = SqlDbType.Bit } ,

                                        new SqlParameter() { ParameterName = "@landingPageTransactionId",Value=0, SqlDbType = SqlDbType.Int,Direction= ParameterDirection.InputOutput } 
                                       
                };
            IBHICDBBase bhicDBBase = GetDbConnector();

            try
            {
                bhicDBBase.OpenConnection();
                bhicDBBase.BeginTransaction();
                //execute stored procedure
                bhicDBBase.ExecuteNonQuery("AddEditlandingPage", QueryCommandType.StoredProcedure, dbParams);
                landingPageTransactionId = Convert.ToInt64(dbParams[(dbParams.Count - 1)].Value);
                bhicDBBase.CommitTransaction();
            }
            catch
            {
                throw;
            }
            finally
            {
                bhicDBBase.Dispose();
            }

            return landingPageTransactionId;
        }



        public LandingPageTransaction GetLandingPage(string AdId)
        {
            var landingPageTransaction = new LandingPageTransaction();

            //execute stored procedure

            DataSet dset = GetDbConnector().LoadDataSet("GetLandingPage", QueryCommandType.StoredProcedure,
                            new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@AdId", Value = AdId, SqlDbType = SqlDbType.VarChar, Size=32} ,
                                    });

            if (dset.Tables[0].Rows.Count > 0)
            {
                DataRow row = dset.Tables[0].Rows[0];
                landingPageTransaction.Id = Convert.ToInt32(row["Id"]);
                landingPageTransaction.lob = Convert.ToString(row["lob"]);
                landingPageTransaction.State = Convert.ToString(row["State"]);
                landingPageTransaction.TransactionCounter = Convert.ToInt32(row["TransactionCounter"]);

                landingPageTransaction.PageName= Convert.ToString(row["PageName"]);
                landingPageTransaction.Heading = Convert.ToString(row["Heading"]);
                landingPageTransaction.SubHeading = Convert.ToString(row["SubHeading"]);
                landingPageTransaction.ProductName = Convert.ToString(row["ProductName"]);
                landingPageTransaction.ProductHighlight = Convert.ToString(row["ProductHighlight"]);
                landingPageTransaction.BTNText = Convert.ToString(row["BTNText"]);
                landingPageTransaction.CalloutText = Convert.ToString(row["CalloutText"]);

                landingPageTransaction.MainImage = Convert.ToString(row["MainImage"]);
                landingPageTransaction.TokenId = Convert.ToString(row["TokenId"]);
                landingPageTransaction.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                landingPageTransaction.TemplateId = Convert.ToInt32(row["TemplateId"]);
                landingPageTransaction.TemplateCss = Convert.ToString(row["TemplateCss"]);
                landingPageTransaction.logo = Convert.ToString(row["Logo"]);
                landingPageTransaction.IsDeployed = Convert.ToBoolean(row["IsDeployed"]);

                List<CTAMessage> msgList = new List<CTAMessage>();

                foreach (DataRow ctarow in dset.Tables[1].Rows)
                {
                    CTAMessage msg = new CTAMessage();
                    msg.Id = Convert.ToInt32(ctarow["Id"]);
                    msg.TokenId = Convert.ToString(ctarow["TokenId"]);
                    msg.Message = Convert.ToString(ctarow["Message"]);
                    msg.CreatedOn = Convert.ToDateTime(ctarow["CreatedOn"]);
                    msgList.Add(msg);
                }

                landingPageTransaction.CTAMsgList = msgList;

            }
            return landingPageTransaction;
        }

        public List<LandingPageTransaction> GetLandingPages(string filter)
        {

            var landingPageTransaction = new List<LandingPageTransaction>();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetLandingPages", QueryCommandType.StoredProcedure,
                            new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@filter", Value = filter, SqlDbType = SqlDbType.VarChar, Size=32 , IsNullable=true} ,
                                    });

            while (rdr.Read())
            {
                landingPageTransaction.Add(new LandingPageTransaction
                {
                    Id = Convert.ToInt32(rdr["Id"]),
                    lob = Convert.ToString(rdr["lob"]),
                    State = Convert.ToString(rdr["State"]),
                    TransactionCounter = Convert.ToInt32(rdr["TransactionCounter"]),
                    logo = Convert.ToString(rdr["logo"]),

                    PageName = Convert.ToString(rdr["PageName"]),
                    Heading = Convert.ToString(rdr["Heading"]),
                    SubHeading = Convert.ToString(rdr["SubHeading"]),
                    ProductName = Convert.ToString(rdr["ProductName"]),
                    ProductHighlight = Convert.ToString(rdr["ProductHighlight"]),
                    BTNText = Convert.ToString(rdr["BTNText"]),
                    CalloutText = Convert.ToString(rdr["CalloutText"]),
                    
                    MainImage = Convert.ToString(rdr["MainImage"]),
                    TemplateId = Convert.ToInt32(rdr["TemplateId"]),
                    Controller = Convert.ToString(rdr["Controller"]),
                    ActionResult = Convert.ToString(rdr["ActionResult"]),
                    TokenId = Convert.ToString(rdr["TokenId"]),
                    CreatedOn = Convert.ToDateTime(rdr["CreatedOn"]),
                    IsDeployed = string.IsNullOrEmpty(rdr["IsDeployed"].ToString()) ? false : Convert.ToBoolean(rdr["IsDeployed"])
                });

            }

            return landingPageTransaction;
        }

        public LandingPageTransaction GetLandingPage(long Id)
        {
            var landingPageTransaction = new LandingPageTransaction();

            //execute stored procedure
            var rdr = GetDbConnector().ExecuteReader("GetLandingPageById", QueryCommandType.StoredProcedure,
                            new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@Id", Value = Id, SqlDbType = SqlDbType.BigInt} ,
                                    });

            while (rdr.Read())
            {
                landingPageTransaction.Id = Convert.ToInt32(rdr["Id"]);
                landingPageTransaction.lob = Convert.ToString(rdr["lob"]);
                landingPageTransaction.State = Convert.ToString(rdr["State"]);
                landingPageTransaction.TransactionCounter = Convert.ToInt32(rdr["TransactionCounter"]);
                landingPageTransaction.logo = Convert.ToString(rdr["logo"]);


                landingPageTransaction.Heading = Convert.ToString(rdr["Heading"]);
                landingPageTransaction.SubHeading = Convert.ToString(rdr["SubHeading"]);
                landingPageTransaction.ProductName = Convert.ToString(rdr["ProductName"]);
                landingPageTransaction.ProductHighlight = Convert.ToString(rdr["ProductHighlight"]);
                landingPageTransaction.BTNText = Convert.ToString(rdr["BTNText"]);
                landingPageTransaction.CalloutText = Convert.ToString(rdr["CTABoxTitle"]);

                landingPageTransaction.MainImage = Convert.ToString(rdr["MainImage"]);
                landingPageTransaction.TemplateId = Convert.ToInt32(rdr["TemplateId"]);
                landingPageTransaction.Controller = Convert.ToString(rdr["Controller"]);
                landingPageTransaction.ActionResult = Convert.ToString(rdr["ActionResult"]);
                landingPageTransaction.TokenId = Convert.ToString(rdr["TokenId"]);
                landingPageTransaction.CreatedOn = Convert.ToDateTime(rdr["CreatedOn"]);
                landingPageTransaction.IsDeployed = string.IsNullOrEmpty(Convert.ToString(rdr["IsDeployed"])) ? false : Convert.ToBoolean(rdr["IsDeployed"]);

            }

            return landingPageTransaction;
        }

        public void UpdateTransactionCounter(string tokenId)
        {
            List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@TokenId", Value = tokenId, SqlDbType = SqlDbType.VarChar,Size=32}, 
                                       
                };
            IBHICDBBase bhicDBBase = GetDbConnector();

            try
            {
                bhicDBBase.OpenConnection();
                bhicDBBase.BeginTransaction();
                //execute stored procedure
                bhicDBBase.ExecuteNonQuery("UpdatelandingPageTransaction", QueryCommandType.StoredProcedure, dbParams);
                bhicDBBase.CommitTransaction();
            }
            catch
            {
                throw;
            }
            finally
            {
                bhicDBBase.Dispose();
            }
        }

        public bool InsertOrUpdateCTAMsgs(List<CTAMessage> listCTAMessages, string tokenId, long userId)
        {

            bool flag = false;
            string existsIds = "";
            IBHICDBBase bhicDBBase = GetDbConnector();

            DataTable CTAList = GetDbConnector().LoadDataSet("GetAllCTAMessagesByTokenId", QueryCommandType.StoredProcedure,
                           new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@tokenId", Value = tokenId, SqlDbType = SqlDbType.VarChar} ,
                                    }).Tables[0];

            var exitsIds = from row in CTAList.AsEnumerable()
                           select row["Id"];

            foreach (CTAMessage ctamsg in listCTAMessages)
            {

                List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@id", Value = ctamsg.Id, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@tokenId", Value =tokenId, SqlDbType = SqlDbType.VarChar,Size=32} ,
                                        new SqlParameter() { ParameterName = "@message", Value = ctamsg.Message , SqlDbType = SqlDbType.VarChar}, 
                                        new SqlParameter() { ParameterName = "@createdBy", Value = userId, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@updatedBy", Value = userId, SqlDbType = SqlDbType.BigInt,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@returnId",Value=0, SqlDbType = SqlDbType.Int,Direction= ParameterDirection.InputOutput } 
                                       
                };

                try
                {
                    flag = false;
                    bhicDBBase.OpenConnection();
                    bhicDBBase.BeginTransaction();
                    //execute stored procedure
                    bhicDBBase.ExecuteNonQuery("InsertOrUpdateCTAMessages", QueryCommandType.StoredProcedure, dbParams);
                    existsIds = (existsIds == "") ? Convert.ToString(dbParams[(dbParams.Count - 1)].Value) : existsIds + "," + Convert.ToString(dbParams[(dbParams.Count - 1)].Value);
                    bhicDBBase.CommitTransaction();
                    flag = true;
                }
                catch
                {
                    flag = false;
                }
                finally
                {
                    bhicDBBase.Dispose();
                }
            }

            // Delete Removed CTA Msgs 
            try
            {
                if (flag)
                {
                    List<IDbDataParameter> dbParams = new List<IDbDataParameter> { 
                        new SqlParameter() { ParameterName = "@existsIds", Value = existsIds, SqlDbType = SqlDbType.VarChar } ,
                        new SqlParameter() { ParameterName = "@tokenId", Value =tokenId, SqlDbType = SqlDbType.VarChar,Size=32} 
                    };
                    bhicDBBase.OpenConnection();
                    bhicDBBase.BeginTransaction();
                    //execute stored procedure
                    bhicDBBase.ExecuteNonQuery("DeleteCTAMessages", QueryCommandType.StoredProcedure, dbParams);
                    bhicDBBase.CommitTransaction();
                    flag = true;
                }
            }
            catch
            {
                flag = false;
            }
            finally
            {
                bhicDBBase.Dispose();
            }


            return (flag) ? true : false;
        }

        /*Use to insert data related to Transacttion Logging*/
        #endregion



        public bool InsertOrUpdateTemplateLogo(string template, string logo)
        {

            List<IDbDataParameter> dbParams = new List<IDbDataParameter>
                {
                                        new SqlParameter() { ParameterName = "@template", Value = template, SqlDbType = SqlDbType.VarChar ,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@logo", Value = "/LandingPage/Images/Logo/" + logo, SqlDbType = SqlDbType.VarChar ,IsNullable=true } ,
                                       
                };
            IBHICDBBase bhicDBBase = GetDbConnector();

            try
            {
                bhicDBBase.OpenConnection();
                bhicDBBase.BeginTransaction();
                //execute stored procedure
                bhicDBBase.ExecuteNonQuery("InsertTemplates", QueryCommandType.StoredProcedure, dbParams);

                bhicDBBase.CommitTransaction();
            }
            catch
            {
                throw;
            }
            finally
            {
                bhicDBBase.Dispose();
            }

            return true;
        }


        public long Authentication(string username, string password)
        {
            Int64 userId = 0;
            try
            {
                //execute stored procedure
                DataTable table = GetDbConnector().LoadDataSet("GetLandingPageUser", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@username", Value = username, SqlDbType = SqlDbType.VarChar ,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@password", Value = password, SqlDbType = SqlDbType.VarChar ,IsNullable=true } ,
                                    }).Tables[0];

                if (table.Rows.Count > 0)
                {
                    userId = Convert.ToInt64(table.Rows[0]["Id"]);
                }
            }
            catch
            {
                throw;
            }


            return userId;
        }


        public bool DeleteLandingPages(string ids, long userId)
        {
            try
            {
                //execute stored procedure
                GetDbConnector().ExecuteNonQuery("DeleteLandingPages", QueryCommandType.StoredProcedure,
                                new List<System.Data.IDbDataParameter> 
                                    { 
                                        new SqlParameter() { ParameterName = "@ids", Value = ids, SqlDbType = SqlDbType.VarChar ,IsNullable=true } ,
                                        new SqlParameter() { ParameterName = "@updatedBy", Value = userId, SqlDbType = SqlDbType.BigInt,IsNullable=true }
                                    });

                return true;
            }
            catch
            {
                throw;
            }
        }


        // Get All, LOB, States      
        public DataSet GetDefaultMasterData()
        {
            //execute stored procedure
            var dataSet = GetDbConnector().LoadDataSet("GetAllLandingPageMasterData", QueryCommandType.StoredProcedure);
            return dataSet;
        }


    }
}

