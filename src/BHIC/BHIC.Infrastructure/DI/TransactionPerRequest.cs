using System.Web;
using BHIC.Infrastructure.Application;

namespace BHIC.Infrastructure.DI
{
    public class TransactionPerRequest : IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        //private readonly EntityContext _entityContext;
        private readonly HttpContextBase _httpContextBase;

        public TransactionPerRequest(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }

        private const string TRANSACTION = "_Transaction_";
        private const string ERROR = "_Error_";
        private const string ERROR_DETAILS = "_Error_Details_";
        private const string ERRORDUMP = "_ErrorDump_";

        void IRunOnEachRequest.Execute()
        {

            //_httpContextBase.Items[TRANSACTION] =_entityContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        void IRunOnError.Execute()
        {
            _httpContextBase.Items[ERROR] = true;
            _httpContextBase.Items[ERROR_DETAILS] = HttpContext.Current.Server.GetLastError();

        }

        void IRunAfterEachRequest.Execute()
        {
            //var trans = (DbContextTransaction)_httpContextBase.Items[TRANSACTION];
            //if (_httpContextBase.Items[ERROR] != null)
            //{
            //    var ex = (Exception)_httpContextBase.Items[ERROR_DETAILS];
            //    trans.Rollback();
            //    //_logService.Fatal("Last Error", ex);
            //}
            //else
            //{
            //    _entityContext.SaveChanges();
            //    trans.Commit();
            //}

            //trans.Dispose();

            //if (_entityContext != null)
            //    _entityContext.Dispose();
        }
    }
}
