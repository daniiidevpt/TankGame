using Core.GameEventsSystem.EventTypes.Events;
using UnityEngine;

namespace Gameplay.TurnSystem.Data
{
    [CreateAssetMenu(fileName = "TurnSystemStaticData", menuName = "StaticData/TurnSystem/TurnSystemStaticData")]
    public class TurnSystemData : ScriptableObject
    {
        [Header("Timing")]
        public float TurnDuration = 30f;

        [Header("Events")]
        public GameEventInt OnTurnStarted;
        public GameEventVoid OnTurnEnded;
        public GameEventFloat OnTurnTimerUpdated;
    }
}
