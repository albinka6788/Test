using System.Web.Mvc;
using StructureMap.Graph;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace BHIC.Infrastructure.DI
{
    public class ControllerConvention : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.CanBeCastTo(typeof(Controller)) && !type.IsAbstract)
            {
                registry.For(type).LifecycleIs(new UniquePerRequestLifecycle());
            }

        }
    }
}
