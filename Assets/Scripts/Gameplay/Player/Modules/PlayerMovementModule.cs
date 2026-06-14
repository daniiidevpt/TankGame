using Core.ModulesSystem;
using Gameplay.Player.Data;
using UnityEngine;

namespace Gameplay.Player.Modules
{
    [System.Serializable]
    public class PlayerMovementModule : ModuleBase
    {
        private const float GroundStickSpeed = 2f;

        private PlayerMovementData m_Data;
        private CharacterController m_Controller;

        private bool m_IsGrounded;
        private Vector3 m_GroundNormal = Vector3.up;
        private float m_VerticalSpeed;

        public override void OnInitialize(ModuleController owner)
        {
            base.OnInitialize(owner);

            m_Data = owner.GetData<PlayerMovementData>();
            m_Controller = owner.GetComponent<CharacterController>();

            if (m_Data == null)
                Debug.LogError($"Missing Data In Module {this} - Data: {typeof(PlayerMovementData)} - Owner: {owner.name}");

            if (m_Controller == null)
                Debug.LogError($"Missing CharacterController Component In {owner.name}");
        }

        public override void OnEnabled() => SetActionEnabled(true);

        public override void OnDisabled() => SetActionEnabled(false);

        public override void OnDispose() => SetActionEnabled(false);

        private void SetActionEnabled(bool enabled)
        {
            if (enabled) m_Data.MoveAction.action.Enable();
            else m_Data.MoveAction.action.Disable();
        }

        public override void OnUpdate()
        {
            if (m_Data == null || m_Controller == null)
                return;

            Vector2 input = m_Data.MoveAction != null ? m_Data.MoveAction.action.ReadValue<Vector2>() : Vector2.zero;
            float deltaTime = Time.deltaTime;

            m_IsGrounded = m_Controller.isGrounded;
            m_GroundNormal = SampleGroundNormal();

            ApplyGravity(deltaTime);
            ApplyOrientation(input, deltaTime);
            ApplyMovement(input, deltaTime);
        }

        private void ApplyGravity(float deltaTime)
        {
            if (m_IsGrounded && m_VerticalSpeed < 0f)
                m_VerticalSpeed = -GroundStickSpeed;
            else
                m_VerticalSpeed += Physics.gravity.y * deltaTime;
        }

        private void ApplyOrientation(Vector2 input, float deltaTime)
        {
            // Match the slope while grounded
            Vector3 targetUp = m_IsGrounded ? m_GroundNormal : Vector3.up;
            Quaternion rotation = Transform.rotation;
            Quaternion aligned = Quaternion.FromToRotation(rotation * Vector3.up, targetUp) * rotation;
            rotation = Quaternion.Slerp(rotation, aligned, m_Data.AlignSpeed * deltaTime);

            // Steer
            float steerInput = input.y < 0f ? -input.x : input.x;
            rotation = Quaternion.AngleAxis(steerInput * m_Data.TurnSpeed * deltaTime, targetUp) * rotation;

            Transform.rotation = rotation;
        }

        private void ApplyMovement(Vector2 input, float deltaTime)
        {
            Vector3 heading = Vector3.ProjectOnPlane(Transform.forward, Vector3.up).normalized;
            Vector3 motion = heading * (input.y * m_Data.MaxSpeed) + Vector3.up * m_VerticalSpeed;
            m_Controller.Move(motion * deltaTime);
        }

        private Vector3 SampleGroundNormal()
        {
            if (!m_IsGrounded)
                return Vector3.up;

            if (Physics.Raycast(Transform.position, Vector3.down, out RaycastHit hit, m_Data.GroundCheckDistance, ~0, QueryTriggerInteraction.Ignore))
                return hit.normal;

            return Vector3.up;
        }

        public override void OnDrawGizmos()
        {
            if (m_Data == null)
                return;

            Gizmos.color = m_IsGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(Transform.position, Transform.position + Vector3.down * m_Data.GroundCheckDistance);
        }
    }
}
