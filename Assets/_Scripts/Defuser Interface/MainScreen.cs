using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

    [SerializeField] private Material _displayMaterial;

    private Color[] _colourList = new Color[] {
        new Color(225f / 255f, 0, 0),
        new Color(225f / 255f, 225f / 255f, 0),
        new Color(0, 225f / 255f, 0),
        new Color(0, 0, 225f / 255f)
    };

    void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void DisplayColour(SamColour colour) {
        _displayMaterial.color = _colourList[(int)colour];
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Disable() {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
