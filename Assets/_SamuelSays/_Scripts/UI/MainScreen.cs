using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

    [SerializeField] private Material _displayMaterial;

    private const float ColourBrightness = 0.9f;

    private Color[] _colourList = new Color[] {
        Color.red * ColourBrightness,
        Color.yellow * ColourBrightness,
        Color.green * ColourBrightness,
        Color.blue * ColourBrightness
    };

    private MeshRenderer _colourDisplay;
    private Coroutine _displaySequence;

    void Awake() {
        _colourDisplay = GetComponent<MeshRenderer>();
        _colourDisplay.enabled = false;
    }

    //public void DisplayColour(SamColour colour) {
    //    _displayMaterial.color = _colourList[(int)colour];
    //    GetComponent<MeshRenderer>().enabled = true;
    //}

    public void Disable() {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
