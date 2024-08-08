using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Constants;
using PlayerEntities;
using UnityEngine;
using UnityEngine.Serialization;
using XiheFramework.Core.Entity;
using XiheFramework.Runtime;

public class FollowCharacterMiddle : MonoBehaviour {
    [SerializeField]
    CinemachineVirtualCamera virtualCamera;

    private uint m_LeftId;
    private uint m_RightId;
    private Transform m_LeftTransform;
    private Transform m_RightTransform;

    void LateUpdate() {
        if (m_LeftId == 0) {
            m_LeftId = Game.Blackboard.GetData<uint>(BlackboardDataNames.EntityIdP1);
        }
        else {
            if (m_LeftTransform == null && Game.Entity.IsEntityExisted(m_LeftId)) {
                m_LeftTransform = Game.Entity.GetEntity<GameEntity>(m_LeftId).transform;
            }
        }

        if (m_RightId == 0) {
            m_RightId = Game.Blackboard.GetData<uint>(BlackboardDataNames.EntityIdP2);
        }
        else {
            if (m_RightTransform == null && Game.Entity.IsEntityExisted(m_RightId)) {
                m_RightTransform = Game.Entity.GetEntity<GameEntity>(m_RightId).transform;
            }
        }

        Vector3 leftPos;
        Vector3 rightPos;
        if (m_LeftTransform == null && m_RightTransform == null) {
            leftPos = Vector3.zero;
            rightPos = Vector3.zero;
        }
        else if (m_LeftTransform == null && m_RightTransform != null) {
            leftPos = m_RightTransform.position;
            rightPos = m_RightTransform.position;
        }
        else if (m_LeftTransform != null && m_RightTransform == null) {
            leftPos = m_LeftTransform.position;
            rightPos = m_LeftTransform.position;
        }
        else {
            leftPos = m_LeftTransform.position;
            rightPos = m_RightTransform.position;
        }

        transform.position = (leftPos + rightPos) / 2f;
        var delta = leftPos - rightPos;
        var zOffset = Mathf.Lerp(-750, -1900, (delta.magnitude) / 300f);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = zOffset;
    }
}