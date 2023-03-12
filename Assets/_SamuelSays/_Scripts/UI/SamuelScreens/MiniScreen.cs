using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScreen : MonoBehaviour {
    [SerializeField] private Material _displayMaterial;
    [SerializeField] private TextMesh LetterDisplay;
    [SerializeField] private TextMesh EmoticonDisplay;
    [SerializeField] private MeshRenderer _colourDisplay;

    private void Awake() {
        ClearScreen();
    }

    public void ClearScreen() {
        LetterDisplay.text = "";
        EmoticonDisplay.text = "";
        _colourDisplay.enabled = false;
    }

    public void DisplayEmoticon(string faceText, Color colour) {
        ClearScreen();
        EmoticonDisplay.text = faceText;
        EmoticonDisplay.color = colour;
    }

    public void DisplayLetter(char letter) {
        ClearScreen();
        LetterDisplay.text = letter.ToString();
    }

    public void DisplayColour(Color colour) {
        ClearScreen();
        _displayMaterial.color = colour;
        _colourDisplay.enabled = true;
    }
}
