using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace BHIC.Infrastructure.DI
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        private readonly Func<IContainer> _container;

        public StructureMapControllerFactory(Func<IContainer> containerFactory)
        {
            _container = containerFactory;
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return (IController)_container().GetNestedContainer().GetInstance(controllerType);
        }
    }
}
