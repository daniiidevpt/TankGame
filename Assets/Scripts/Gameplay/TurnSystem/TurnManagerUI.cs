using Core.GameEventsSystem.EventTypes.Events;
using TMPro;
using UnityEngine;
using Void = Core.GameEventsSystem.Void;

namespace Gameplay.TurnSystem
{
    public class TurnManagerUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI m_TurnTimer;
        [SerializeField] private TextMeshProUGUI m_TurnTitle;
        [SerializeField] private TextMeshProUGUI m_TurnSubtitle;

        [Header("Event Listening")]
        [SerializeField] private GameEventInt OnTurnStarted;
        [SerializeField] private GameEventVoid OnTurnEnded;
        [SerializeField] private GameEventFloat OnTurnTimerUpdated;

        private void OnEnable()
        {
            OnTurnStarted.Register(this, ShowTurnStart);
            OnTurnEnded.Register(this, ShowTurnEnd);
            OnTurnTimerUpdated.Register(this, UpdateTurnTimer);
        }

        private void OnDisable()
        {
            OnTurnStarted.Unregister(this);
            OnTurnEnded.Unregister(this);
            OnTurnTimerUpdated.Unregister(this);
        }

        private void ShowTurnStart(int value)
        {
            m_TurnTitle.text = "TURN START";
            m_TurnSubtitle.text = TurnManager.Instance?.Participants[value].CameraTarget.name;
        }

        private void ShowTurnEnd(Void _)
        {
            m_TurnTitle.text = "TURN ENDED";
            m_TurnSubtitle.text = "";
            m_TurnTimer.text = "";
        }

        private void UpdateTurnTimer(float value)
        {
            m_TurnTitle.text = "";
            m_TurnSubtitle.text = "";
            m_TurnTimer.text = $"Turn Timer: {value:0}";
        }
    }
}

