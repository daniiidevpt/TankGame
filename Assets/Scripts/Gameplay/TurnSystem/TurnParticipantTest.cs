using Gameplay.TurnSystem.Interfaces;
using UnityEngine;

namespace Gameplay.TurnSystem
{
    public class TurnParticipantTest : MonoBehaviour, ITurnParticipant
    {
        public Transform CameraTarget => transform;

        void Start()
        {
            TurnManager.Instance?.RegisterParticipant(this);
        }

        public void OnTurnBegin()
        {
            Debug.Log("Participant Turn Begin");
        }

        public void OnTurnEnd()
        {
            Debug.Log("Participant Turn End");
        }
    }
}

