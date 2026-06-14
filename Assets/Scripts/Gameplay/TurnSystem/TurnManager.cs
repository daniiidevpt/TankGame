using System.Collections.Generic;
using Core.Interfaces;
using Core.SingletonSystem;
using Gameplay.TurnSystem.Data;
using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay.TurnSystem
{
    public class TurnManager : PersistentSingleton<TurnManager>
    {
        [SerializeField] private TurnSystemData m_Data;
        [SerializeField] private CinemachineCamera m_Camera;

        [SerializeField] private readonly List<ITurnParticipant> m_Participants = new();
        private int m_CurrentIndex = -1;
        private float m_ElapsedThisTurn;
        private int m_LastWholeSecondNotified;
        private bool m_IsRunning;

        protected override void OnInitialization()
        {
            if (m_Data == null)
                Debug.LogError("[TurnManager] TurnSystemData is not assigned");

            if (m_Camera == null)
                Debug.LogError("[TurnManager] CinemachineCamera is not assigned");
        }

        public void RegisterParticipant(ITurnParticipant participant)
        {
            if (!m_Participants.Contains(participant))
                m_Participants.Add(participant);
        }

        [ContextMenu("Start Combat")]
        public void StartCombat()
        {
            if (m_IsRunning)
            {
                Debug.LogError("[TurnManager] StartCombat called but combat is already running");
                return;
            }

            if (m_Participants.Count == 0)
            {
                Debug.LogError("[TurnManager] StartCombat called with no registered participants");
                return;
            }

            m_IsRunning = true;
            BeginTurn(0);
        }

        private void BeginTurn(int index)
        {
            m_CurrentIndex = index;
            m_ElapsedThisTurn = 0f;
            m_LastWholeSecondNotified = 0;

            ITurnParticipant participant = m_Participants[index];

            if (participant.CameraTarget == null)
            {
                Debug.LogError("[TurnManager] Participant CameraTarget is not assigned");
                return;
            }

            m_Camera.Follow = participant.CameraTarget;
            m_Camera.LookAt = participant.CameraTarget;

            participant.OnTurnBegin();
            m_Data.OnTurnStarted?.Raise(index);
        }

        private void EndCurrentTurn()
        {
            m_Participants[m_CurrentIndex].OnTurnEnd();
            m_Data.OnTurnEnded?.Raise();
            BeginTurn((m_CurrentIndex + 1) % m_Participants.Count);
        }

        private void Update()
        {
            if (!m_IsRunning || m_Participants.Count == 0)
                return;

            m_ElapsedThisTurn += Time.deltaTime;

            int elapsed = Mathf.FloorToInt(m_ElapsedThisTurn);
            if (elapsed > m_LastWholeSecondNotified)
            {
                m_LastWholeSecondNotified = elapsed;
                m_Data.OnTurnTimerUpdated?.Raise(Mathf.Max(0f, m_Data.TurnDuration - m_ElapsedThisTurn));
            }

            if (m_ElapsedThisTurn >= m_Data.TurnDuration)
                EndCurrentTurn();
        }
    }
}
