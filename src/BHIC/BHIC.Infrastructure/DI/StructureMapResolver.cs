using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace BHIC.Infrastructure.DI
{
    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator, System.Web.Mvc.IDependencyResolver
    {
        //private readonly IContainer container;

        private readonly Func<IContainer> _containerFactory;

        public StructureMapResolver(Func<IContainer> controllerFactory)
            : base(controllerFactory())
        {
            _containerFactory = controllerFactory;
        }
        
        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(_containerFactory().GetNestedContainer());
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return _containerFactory().GetNestedContainer().GetInstance(controllerType) as IHttpController;
        }
    }
}
