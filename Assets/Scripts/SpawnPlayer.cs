using System;
using System.Collections;
using System.Collections.Generic;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Runtime;

public class SpawnPlayer : MonoBehaviour {
    private void Start() {
        var left = Game.Entity.InstantiateEntity<TennisPlayerEntity>("TennisPlayerEntity_Goat");
        left.transform.position = new Vector3(-10, -2, 0);
        left.inputId = 0;

        var right = Game.Entity.InstantiateEntity<TennisPlayerEntity>("TennisPlayerEntity_Rabbit");
        right.transform.position = new Vector3(10, -2, 0);
        right.inputId = 1;
    }

    // Update is called once per frame
    void Update() { }
}