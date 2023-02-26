using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniScreen : MonoBehaviour {
    [SerializeField] private TextMesh LetterDisplay;
    [SerializeField] private TextMesh EmoticonDisplay;

    void Awake() {
        ClearScreen();
    }

    public void ClearScreen() {
        LetterDisplay.text = "";
        EmoticonDisplay.text = "";
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
}
