using UnityEngine;

namespace Core.GameEventsSystem.EventTypes.Events
{
    [CreateAssetMenu(menuName = "Events/Game Event Void")]
    public class GameEventVoid : GameEvent<Void> 
    {
        public void Raise() => Raise(new Void());
    }
}
