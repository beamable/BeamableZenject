using UnityEngine;
using Zenject;

namespace Beamable.Zenject
{
    [CreateAssetMenu(fileName = "BeamableInstaller", menuName = "Installers/BeamableInstaller")]
    public class BeamableInstaller : ScriptableObjectInstaller<BeamableInstaller>
    {
        public string playerCode;
        public bool logNotReadyWarning = true;
    
        public override void InstallBindings()
        {
            var ctx = BeamContext.ForPlayer(playerCode);
            if (logNotReadyWarning && !ctx.OnReady.IsCompleted)
            {
                Debug.LogWarning($"The BeamContext for playerCode=[{playerCode}] is not ready yet! This may mean that dependencies being loaded with Zenject will not be ready by the time they are accessed. " +
                                 $"It is recommended to wait for the BeamContext's OnReady instance promise before running the Beamable Zenject Installer. " +
                                 $"This warning can be disabled with the ${nameof(logNotReadyWarning)} field.");
            }
            Container.BindBeamableContext(ctx);
        }
    }
}