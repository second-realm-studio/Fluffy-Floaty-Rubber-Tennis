using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Runtime;

public class FanSpinner : MonoBehaviour {
    // Update is called once per frame
    public float speed = 100;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        rb.AddRelativeTorque(Vector3.forward * speed, ForceMode.Force);
    }
}