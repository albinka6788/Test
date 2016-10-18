using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using BHIC.Infrastructure.Application;
using StructureMap;
using StructureMap.Graph;

namespace BHIC.Infrastructure.DI
{
    public static class IoC
    {
        public static IContainer Container;
        public static IContainer Initialize()
        {

            Container = new Container(cfg =>
            {
                cfg.Scan(t =>
                {
                    //t.TheCallingAssembly();
                    //t.AssembliesFromApplicationBaseDirectory();
                    t.AssembliesFromApplicationBaseDirectory(assembly => !assembly.FullName.StartsWith("System.Web"));
                    t.WithDefaultConventions();
                    t.With(new ControllerConvention());
                    t.AddAllTypesOf<IRunAfterEachRequest>();
                    t.AddAllTypesOf<IRunAtInit>();
                    t.AddAllTypesOf<IRunAtStartup>();
                    t.AddAllTypesOf<IRunOnEachRequest>();
                    t.AddAllTypesOf<IRunOnError>();
                    t.AddAllTypesOf<IRunForEachRequestBusinessStart>();
                    t.AddAllTypesOf<IRunForEachRequestBusinessEnd>();
                });
                //cfg.For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
                //cfg.For<IRoleStore<ApplicationRole>>().Use<RoleStore<ApplicationRole>>();

                cfg.For<BundleCollection>().Use(BundleTable.Bundles);
                cfg.For<RouteCollection>().Use(RouteTable.Routes);
                cfg.For<HttpSessionStateBase>().Use(() => new HttpSessionStateWrapper(HttpContext.Current.Session));
                cfg.For<HttpServerUtilityBase>().Use(() => new HttpServerUtilityWrapper(HttpContext.Current.Server));
                cfg.For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));

            }
    );



            return Container;
        }

    }
}
