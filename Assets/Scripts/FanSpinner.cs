using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using XiheFramework.Runtime;

public class FanSpinner : MonoBehaviour {
    // Update is called once per frame
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.AddRelativeTorque(Vector3.forward * Game.Config.FetchConfig<float>(ConfigNames.FanSpinSpeed), ForceMode.Force);
    }
}