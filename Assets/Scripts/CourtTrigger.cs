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
            Game.Event.Invoke(EventNames.OnCourtChange, isRightSide, null);

            GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.4f);
        }
    }

    private void OnTriggerExit(Collider other) {
        GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.4f);
    }
}