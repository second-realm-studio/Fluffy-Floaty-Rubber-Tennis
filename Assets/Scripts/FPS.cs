using UnityEngine;
using UnityEngine.EventSystems;
using XiheFramework.Runtime;

public class FPS : MonoBehaviour {
    private float delay;
    private float fps;

#if UNITY_EDITOR

    private void OnGUI() {
        delay -= Time.deltaTime;
        if (delay <= 0) {
            delay = 1;
            fps = 1.0f / Time.deltaTime;
        }

        GUI.contentColor = Color.red;
        GUI.color = Color.red;
        GUI.Label(new Rect(10, 10, 100, 20), $"FPS: {fps.ToString("0")}");
        GUI.Label(new Rect(10, 30, 100, 20), $"State: {Game.Fsm.GetCurrentState("MainGameLoop").ToString()}");
        var go = EventSystem.current.currentSelectedGameObject != null ? EventSystem.current.currentSelectedGameObject.name : "null";
        GUI.Label(new Rect(10, 50, 100, 20), $"Select: {go}");
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
#endif
}