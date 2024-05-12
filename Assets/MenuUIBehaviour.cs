using System;
using Constants;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XiheFramework.Runtime;
using UIBehaviour = XiheFramework.Core.UI.UIBehaviour;

public class MenuUIBehaviour : UIBehaviour {
    public RectTransform pointer;
    public RectTransform startButtonPivot;
    public RectTransform exitButtonPivot;
    public Button startButton;
    public Button exitButton;

    protected override void OnActive() {
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        pointer.position = startButtonPivot.position;
        startButton.onClick.AddListener(() => { Game.Event.Invoke(EventNames.OnStartBtnClicked); });

        exitButton.onClick.AddListener(() => { Game.Event.Invoke(EventNames.OnExitBtnClicked); });
    }

    private void LateUpdate() {
        if (EventSystem.current.currentSelectedGameObject == startButton.gameObject) {
            pointer.anchoredPosition += (startButtonPivot.anchoredPosition - pointer.anchoredPosition) * 0.2f;
        }
        else if (EventSystem.current.currentSelectedGameObject == exitButton.gameObject) {
            pointer.anchoredPosition = Vector2.Lerp(pointer.anchoredPosition, exitButtonPivot.anchoredPosition, 0.2f);
        }
    }

    protected override void OnUnActive() { }
}