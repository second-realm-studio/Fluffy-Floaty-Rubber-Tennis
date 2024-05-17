using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiheFramework.Core.Utility.Extension;

public class LightFlicker : MonoBehaviour {
    public float flickSpeed = 1f;
    public float initialIntensity = 2;

    Renderer lightRenderer;
    private Color initColor;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private void Start() {
        lightRenderer = GetComponent<Renderer>();
        initColor = lightRenderer.material.GetColor(EmissionColor);
    }

    void Update() {
        var intensity = Mathf.Sin(Time.time * flickSpeed) * initialIntensity / 2 + initialIntensity / 2;
        float factor = Mathf.Pow(2, intensity);
        var color= new Color(initColor.r * factor, initColor.g * factor, initColor.b * factor);
        lightRenderer.material.SetColor(EmissionColor, color);
    }
}