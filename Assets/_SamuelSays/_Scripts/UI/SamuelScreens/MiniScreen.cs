using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScreen : MonoBehaviour {

    [SerializeField] private TextMesh LetterDisplay;
    [SerializeField] private TextMesh EmoticonDisplay;
    [SerializeField] private TextMesh _colourblindText;
    [SerializeField] private MeshRenderer _colourDisplay;

    private MeshRenderer _colourblindRenderer;

    private void Awake() {
        ClearScreen();
        _colourblindRenderer = _colourblindText.GetComponent<MeshRenderer>();
        _colourblindRenderer.enabled = false;
    }

    public void ClearScreen() {
        LetterDisplay.text = string.Empty;
        EmoticonDisplay.text = string.Empty;
        _colourblindText.text = string.Empty;
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

    public void DisplayColour(Color colour, string colourName) {
        ClearScreen();
        _colourDisplay.enabled = true;
        _colourDisplay.material.color = colour;
        _colourblindText.text = colourName;
    }

    public void DisplaySolveSmile() {
        ClearScreen();
        LetterDisplay.characterSize = 0.045f;
        LetterDisplay.text = "Ü";
        LetterDisplay.color = Color.green;
    }

    public void ToggleColourblindMode() {
        _colourblindRenderer.enabled = !_colourblindRenderer.enabled;
    }
}
