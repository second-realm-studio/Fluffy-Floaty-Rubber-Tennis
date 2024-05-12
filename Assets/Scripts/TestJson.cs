using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class TestJson : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        var jsonSetting = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        var myVector3s = new List<Vector3> {
            new Vector3(1, 2, 3),
            new Vector3(4, 5, 6),
            new Vector3(7, 8, 9)
        };
        var json = JsonConvert.SerializeObject(myVector3s, Formatting.Indented, jsonSetting); // To Serialise
        var myVec3s = JsonConvert.DeserializeObject<List<Vector3>>(json); // To Deserialise
        Debug.Log(myVec3s[2]);
        //save /assets/test.json
        System.IO.File.WriteAllText(Application.dataPath + "/test.json", json);
    }

    // Update is called once per frame
    void Update() { }
}