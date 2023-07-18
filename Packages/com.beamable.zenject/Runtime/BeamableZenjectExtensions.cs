using System;
using System.Reflection;
using Beamable;
using Beamable.Common.Dependencies;
using Zenject;

namespace Beamable.Zenject
{
    public class BindOptions
    {
        public bool IncludeResolvers { get; set; }
    }
    
    public static class BeamableZenjectExtensions
    {

        public static readonly BindOptions DefaultOptions = new BindOptions
        {
            IncludeResolvers = true
        };

        private static MethodInfo _buildResolverMethod;

        static BeamableZenjectExtensions()
        {
            _buildResolverMethod = typeof(BeamableZenjectExtensions).GetMethod(nameof(BuildResolverGeneric), BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static void BindBeamableContext(this DiContainer container, BeamContext beamableContext, BindOptions options=null)
        {
            if (options == null)
            {
                options = DefaultOptions;
            }
            
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

            if (options.IncludeResolvers)
            {
                foreach (var service in scope.SingletonServices)
                {
                    BuildResolver(service.Interface, beamableContext, container, x => x.AsSingle());
                }
                foreach (var service in scope.TransientServices)
                {
                    BuildResolver(service.Interface, beamableContext, container, x => x.AsTransient());

                }
                foreach (var service in scope.ScopedServices)
                {
                    BuildResolver(service.Interface, beamableContext, container, x => x.AsCached());
                }

            }
            
        }

        private static void BuildResolver(Type serviceType, BeamContext ctx, DiContainer container, Func<ScopeConcreteIdArgConditionCopyNonLazyBinder, ConcreteIdArgConditionCopyNonLazyBinder> typeBinder)
        {
            var genMethodInstance = _buildResolverMethod.MakeGenericMethod(serviceType);
            genMethodInstance.Invoke(null, new object[] { ctx, container, typeBinder });
        }

        private static void BuildResolverGeneric<T>(BeamContext ctx, DiContainer container, Func<ScopeConcreteIdArgConditionCopyNonLazyBinder, ConcreteIdArgConditionCopyNonLazyBinder> typeBinder)
        {
            BeamResolver<T> resolver = () => ctx.ServiceProvider.GetService<T>();
            var registration = container.Bind(typeof(BeamResolver<T>)).FromMethodUntyped(_ => resolver);
            typeBinder(registration).Lazy();
        }

    }
}