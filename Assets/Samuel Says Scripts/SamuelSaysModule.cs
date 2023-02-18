using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using System.Linq;

public class SamuelSaysModule : MonoBehaviour {

    // Remove when not needed.
    private const string ExampleMorseSequence = "-..-";
    private readonly SamColour[] ExampleColourSequence = new SamColour[] {
        SamColour.Red,
        SamColour.Green,
        SamColour.Yellow,
        SamColour.Blue
    };

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    public KMSelectable SubmitButton;
    public ColouredButton[] Buttons;
    public MainScreen Screen;
    public MiniScreen SmallDisplay;
    public AudioSource Beep;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _moduleSolved = false;

    private const float SingleMorseUnit = 0.2f;
    private const float EmoticonFlashTime = 0.3f;
    private const int EmoticonFlashCount = 3;

    private readonly string[] HappyFaces = new string[] {
        ":)",
        ": )",
        ":-)",
        "=)",
        "= )",
        "=-)",
        ":]" ,
        ": ]",
        ":-]",
        "=]",
        "= ]",
        "=-]"
    };
    private readonly string[] HackedFaces = new string[] {
        ">:(",
        ">:[",
        ">:<",
        ":'(",
        ">:x",
        ":|",
        ">:|",
        ":s",
        ":o",
        ":0",
        ":O"
    };
    private readonly string[] StrikeFaces = new string[] {
        ">:(",
        ">:[",
        ">:<",
        ":'(",
        ">:x",
        ":|",
        ">:|",
        ":s",
        ":o",
        ":0",
        ":O"
    };

    private Coroutine DisplaySequence;

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
        SmallDisplay.DisplayLetter("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[Rnd.Range(0, 26)]);
    }

    private void ButtonRelease(ColouredButton button) {
        if (_playingButtonDance) {
            return;
        }

        button.PlayReleaseAnimation();
        DisplaySequence = StartCoroutine(PlayDisplaySequence(ExampleMorseSequence, ExampleColourSequence));
        SmallDisplay.ClearScreen();
    }

    private void SubmitPress() {
        _muted = !_muted;
        Beep.volume = _muted ? 0 : 1;
        StartCoroutine(PlayButtonDance());
        StartCoroutine(PlayEmoticonFlashAnimation(HappyFaces[Rnd.Range(0, HappyFaces.Length)], Color.green));
    }

    private void Strike() {
        StartCoroutine(PlayEmoticonFlashAnimation(StrikeFaces[Rnd.Range(0, StrikeFaces.Length)], Color.red));
        Module.HandleStrike();
    }

    private IEnumerator PlayEmoticonFlashAnimation(string faceText, Color colour) {
        float elapsedTime;

        for (int i = 0; i < EmoticonFlashCount; i++) {
            elapsedTime = 0;
            SmallDisplay.DisplayEmoticon(faceText, colour);
            while (elapsedTime < EmoticonFlashTime) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0;
            SmallDisplay.ClearScreen();
            while (elapsedTime < EmoticonFlashTime) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
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
            symbolFlashTime = (morse[i] == '.') ? SingleMorseUnit : 3 * SingleMorseUnit;
            Screen.DisplayColour(colours[i]);
            Beep.Play();

            while (elapsedTime / symbolFlashTime < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0;
            Screen.Disable();
            Beep.Stop();

            while (elapsedTime / SingleMorseUnit < 1) {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        elapsedTime = 0;
        while (elapsedTime / (2 * SingleMorseUnit) < 1) {
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
