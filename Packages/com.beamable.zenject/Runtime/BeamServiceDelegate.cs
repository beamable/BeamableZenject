namespace Beamable.Zenject
{
    /// <summary>
    /// The <see cref="BeamResolver{T}"/> can be used to inject any service registered with Beamable DI, with Zenject.
    ///
    /// Any Beamable service can be injected with the zenject, `[Inject]` attribute. However,
    /// the injected service will be locked to the current user of the BeamContext.
    ///
    /// By injecting a <see cref="BeamResolver{T}"/> instead, it must be resolved every usage,
    /// which means that if the user of the BeamContext changes, the next time the resolver is
    /// invoked, the latest service for the user will be returned.
    /// </summary>
    /// <typeparam name="T">Any service type that has been registered with Beamable DI.</typeparam>
    public delegate T BeamResolver<T>();
}