using System;
using System.Collections;
using System.Collections.Generic;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Runtime;

public class SpawnPlayer : MonoBehaviour {
    private void Start() {
        var entity = Game.Entity.InstantiateEntity<TennisPlayerEntity>("TennisPlayerEntity_Goat");
        entity.transform.position = new Vector3(0, -2, 0);
    }

    // Update is called once per frame
    void Update() { }
}