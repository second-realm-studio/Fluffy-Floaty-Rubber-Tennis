using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using XiheFramework.Core.Utility.Extension;
using XiheFramework.Runtime;

public class CourtTrigger : MonoBehaviour {
    public LayerMask ballLayer;
    public bool isRightSide;

    private void OnTriggerEnter(Collider other) {
        if (ballLayer.Includes(other.gameObject.layer)) {
            var currentHitCount = Game.Blackboard.GetData<int>(BlackboardDataNames.HitCount);
            if (currentHitCount == 1 && !isRightSide) {
                currentHitCount++;
            }
            else if (currentHitCount == -1 && isRightSide) {
                currentHitCount--;
            }

            Game.Blackboard.SetData(BlackboardDataNames.HitCount, currentHitCount);
            // Game.Event.Invoke(EventNames.OnCourtChange, isRightSide, null);

#if UNITY_EDITOR
            GetComponent<Renderer>().enabled = false;
#endif
        }
    }

    private void OnTriggerExit(Collider other) {
#if UNITY_EDITOR
        GetComponent<Renderer>().enabled = true;
        GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.7f);
#endif
    }
}