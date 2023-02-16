using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

    [SerializeField] private Material _displayMaterial;

    private Color[] _colourList = new Color[] { Color.red, Color.yellow, Color.green, Color.blue };

	void Awake() {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void ShowColour(SamColour colour) {
        _displayMaterial.color = _colourList[(int)colour];
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Disable() {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
