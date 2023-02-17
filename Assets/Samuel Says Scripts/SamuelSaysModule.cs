using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.CodeDom;

public class SamuelSaysModule : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;
    public KMSelectable SubmitButton;
    public ColouredButton[] Buttons;
    public MainScreen Screen;

    private const float SingleTimeUnit = 0.25f;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _moduleSolved = false;

    private bool _playingButtonDance = false;

    void Awake() {
        _moduleId = _moduleIdCounter++;
    }

    void Start() {
        foreach (ColouredButton button in Buttons) {
            button.GetComponentInChildren<KMSelectable>().OnInteract += delegate () { button.Lit = true; return false; };
            button.GetComponentInChildren<KMSelectable>().OnInteractEnded += delegate () { button.Lit = false; };
        }

        SubmitButton.OnInteract += delegate () {
            if (!_playingButtonDance) {
                StartCoroutine(PlayButtonDance());
            }
            return false;
        };

        StartCoroutine(PlayDisplaySequence("-..-", new SamColour[] { SamColour.Red, SamColour.Green, SamColour.Yellow, SamColour.Blue }));
    }

    private IEnumerator PlayDisplaySequence(string morse, SamColour[] colours) {
        float elapsedTime;
        float symbolFlashTime;

        if (morse.Length != colours.Length) {
            throw new ArgumentException("Length of morse sequence did not match length of colour sequence.");
        }

        for (int i = 0; i < morse.Length; i++) {
            if (!".-".Contains(morse[i].ToString())) {
                throw new ArgumentException("Morse sequence should contain only dots and dashes.");
            }

            elapsedTime = 0;
            symbolFlashTime = (morse[i] == '.') ? SingleTimeUnit : 3 * SingleTimeUnit;
            Screen.DisplayColour(colours[i]);

            while (elapsedTime / symbolFlashTime < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0;
            Screen.Disable();

            while (elapsedTime / SingleTimeUnit < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        elapsedTime = 0;
        while (elapsedTime / (2 * SingleTimeUnit) < 1) {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(PlayDisplaySequence(morse, colours));
    }

    private IEnumerator PlayButtonDance() {
        float elapsedTime;
        float buttonHoldTime = 0.1f;
        int cycleCount = 3;

        _playingButtonDance = true;

        for (int i = 0; i < 4 * cycleCount; i++) {
            elapsedTime = 0;
            Buttons[i % 4].GetComponentInChildren<KMSelectable>().OnInteract();

            while (elapsedTime / buttonHoldTime < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Buttons[i % 4].GetComponentInChildren<KMSelectable>().OnInteractEnded();
        }

        _playingButtonDance = false;
    }
}
