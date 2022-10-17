using Zero.Game.Server;
using Zero.Game.Shared;

namespace Zero.Demo.World.Systems
{
    public class CapacityLogSystem : ComponentSystem
    {
        private static long MsFrequency = System.Diagnostics.Stopwatch.Frequency / 1000;

        protected override void OnUpdate()
        {
            if (Time.LastUpdateDuration > Time.TargetDelta)
            {
                Debug.Log("Update lagged! {0}ms", Time.LastUpdateDuration);
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.SynchronizationContext, nameof(Time.LastUpdateMethods.SynchronizationContext));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.AddRemoveWorlds, nameof(Time.LastUpdateMethods.AddRemoveWorlds));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.AddRemoveConnections, nameof(Time.LastUpdateMethods.AddRemoveConnections));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.UpdateTasks, nameof(Time.LastUpdateMethods.UpdateTasks));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.ReceiveData, nameof(Time.LastUpdateMethods.ReceiveData));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.UpdateWorlds, nameof(Time.LastUpdateMethods.UpdateWorlds));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.UpdateViews, nameof(Time.LastUpdateMethods.UpdateViews));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.SendData, nameof(Time.LastUpdateMethods.SendData));
                Debug.Log("\t{0}% {1}", Time.LastUpdateMethods.WaitNext, nameof(Time.LastUpdateMethods.WaitNext));
            }
        }
    }
}
