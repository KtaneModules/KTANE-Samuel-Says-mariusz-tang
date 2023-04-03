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
        DisplayLetter(letter.ToString());
    }

    public void DisplayLetter(string letter) {
        ClearScreen();
        LetterDisplay.text = letter;
    }

    public void DisplayColour(Color colour) {
        ClearScreen();
        _displayMaterial.color = colour;
        _colourDisplay.enabled = true;
    }

    public void DisplaySolveSmile() {
        ClearScreen();
        LetterDisplay.characterSize = 0.045f;
        LetterDisplay.text = "Ü";
        LetterDisplay.color = Color.green;
    }
}
