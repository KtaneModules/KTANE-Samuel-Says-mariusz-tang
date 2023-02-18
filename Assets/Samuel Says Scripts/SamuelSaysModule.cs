using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System.CodeDom;
using Rnd = UnityEngine.Random;

public class SamuelSaysModule : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    public KMSelectable SubmitButton;
    public ColouredButton[] Buttons;
    public MainScreen Screen;
    public AudioSource Beep;
    public TextMesh LetterDisplay;

    private const float SingleTimeUnit = 0.2f;
    private const string ExampleMorseSequence = "-..-";
    private readonly SamColour[] ExampleColourSequence = new SamColour[] {
        SamColour.Red,
        SamColour.Green,
        SamColour.Yellow,
        SamColour.Blue
    };

    private Coroutine DisplaySequence;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _moduleSolved = false;

    private bool _playingButtonDance = false;
    private bool _muted = false;

    void Awake() {
        _moduleId = _moduleIdCounter++;
    }

    void Start() {
        foreach (ColouredButton button in Buttons) {
            button.GetComponentInChildren<KMSelectable>().OnInteract += delegate () { ButtonHold(button); return false; };
            button.GetComponentInChildren<KMSelectable>().OnInteractEnded += delegate () { ButtonRelease(button); };
        }

        SubmitButton.OnInteract += delegate () { SubmitPress(); return false; };
        DisplaySequence = StartCoroutine(PlayDisplaySequence(ExampleMorseSequence, ExampleColourSequence));
    }

    private void ButtonHold(ColouredButton button) {
        if (_playingButtonDance) {
            return;
        }

        button.PlayHoldAnimation();
        StopFlashingDisplaySequence();
        LetterDisplay.text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[Rnd.Range(0, 26)].ToString();
    }

    private void ButtonRelease(ColouredButton button) {
        if (_playingButtonDance) {
            return;
        }

        button.PlayReleaseAnimation();
        DisplaySequence = StartCoroutine(PlayDisplaySequence(ExampleMorseSequence, ExampleColourSequence));
        LetterDisplay.text = "";
    }

    private void SubmitPress() {
        _muted = !_muted;
        Beep.volume = _muted ? 0 : 1;
        StartCoroutine(PlayButtonDance());
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
            Beep.Play();

            while (elapsedTime / symbolFlashTime < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0;
            Screen.Disable();
            Beep.Stop();

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

        DisplaySequence = StartCoroutine(PlayDisplaySequence(morse, colours));
    }

    private IEnumerator PlayButtonDance() {
        float elapsedTime;
        float buttonHoldTime = 0.1f;
        int cycleCount = 3;

        _playingButtonDance = true;

        for (int i = 0; i < 4 * cycleCount; i++) {
            elapsedTime = 0;
            Buttons[i % 4].PlayHoldAnimation();

            while (elapsedTime / buttonHoldTime < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Buttons[i % 4].PlayReleaseAnimation();
        }

        _playingButtonDance = false;
    }

    private void StopFlashingDisplaySequence() {
        StopCoroutine(DisplaySequence);
        Screen.Disable();
        Beep.Stop();
    }
}
