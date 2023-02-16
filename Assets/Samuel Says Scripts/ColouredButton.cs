using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {
    [SerializeField] private MeshRenderer _buttonCover;
    [SerializeField] private MeshRenderer _buttonBacking;
    [SerializeField] private Light _light;

    public SamColour Colour { get;
        private set;
    }
    public bool Lit {
        set {
            _light.enabled = value;
        }
    }

    void Awake() {
        Color[] buttonColourList = new Color[] {
            new Color(88f / 255f, 0, 0, 0.75f),
            new Color(88f / 255f, 88f / 255f, 0, 0.75f),
            new Color(0, 88f / 255f, 0, 0.75f),
            new Color(0, 0, 88f / 255f, 0.75f)
        };
        Color[] lightColourList = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };
        int colourIndex = "ABCD".IndexOf(transform.name);
        Color buttonColour = buttonColourList[colourIndex];
        Color lightColour = lightColourList[colourIndex];

        _buttonCover.material.color = buttonColour;
        _buttonBacking.material.color = buttonColour;
        _light.color = lightColour;
        Colour = (SamColour)colourIndex;

        _light.enabled = false;
    }
}
