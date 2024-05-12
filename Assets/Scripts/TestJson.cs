using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using XiheFramework.Core.Config;

public class TestJson : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        var jsonSetting = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        ConfigData newData = new ConfigData {
            entries = new List<ConfigEntry> {
                new ConfigEntry { path = "exampleKey1", type = ConfigType.String, value = "Hello" },
                new ConfigEntry { path = "examplePath2", type = ConfigType.Vector3, value = JsonConvert.SerializeObject(new Vector3(1, 2, 3), Formatting.Indented, jsonSetting) },
            }
        };

        string newJson = JsonUtility.ToJson(newData, true);

        //save /assets/test.json
        System.IO.File.WriteAllText(Application.dataPath + "/test.json", newJson);
    }

    // Update is called once per frame
    void Update() { }
}