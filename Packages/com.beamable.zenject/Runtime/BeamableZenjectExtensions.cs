using System;
using Beamable;
using Beamable.Common.Dependencies;
using Zenject;

namespace Beamable.Zenject
{
    public static class BeamableZenjectExtensions
    {
        public static void BindBeamableContext(this DiContainer container, BeamContext beamableContext)
        {
            var scope = beamableContext.ServiceProvider as IDependencyProviderScope;

            foreach (var service in scope.SingletonServices)
            {
                container.Bind(service.Interface).FromMethodUntyped(_ =>  scope.GetService(service.Interface)).AsSingle().Lazy();
            }
            foreach (var service in scope.TransientServices)
            {
                container.Bind(service.Interface).FromMethodUntyped(_ =>  scope.GetService(service.Interface)).AsTransient().Lazy();
            }
            foreach (var service in scope.ScopedServices)
            {
                container.Bind(service.Interface).FromMethodUntyped(_ => scope.GetService(service.Interface)).AsCached().Lazy();
            }
        }
    }
}