using UnityEngine;

namespace Gameplay.TurnSystem.Interfaces
{
    public interface ITurnParticipant
    {
        Transform CameraTarget { get; }
        void OnTurnBegin();
        void OnTurnEnd();
    }
}
