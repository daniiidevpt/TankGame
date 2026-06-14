using UnityEngine;

namespace Core.Interfaces
{
    public interface ITurnParticipant
    {
        Transform CameraTarget { get; }
        void OnTurnBegin();
        void OnTurnEnd();
    }
}
