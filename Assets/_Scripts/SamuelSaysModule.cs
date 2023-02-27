using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using System.Linq;

public class SamuelSaysModule : MonoBehaviour {

    // Add emote flash sequence to MiniScreen.cs.
    // Add play display sequence to MainScreen.cs.
    // Add button dance.

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule Module;

    public KMSelectable SubmitButton;
    //public ColouredButton[] Buttons;
    public MainScreen Screen;
    public MiniScreen SmallDisplay;

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

    //void Start() {
    //    foreach (ColouredButton button in Buttons) {
    //        button.GetComponentInChildren<KMSelectable>().OnInteract += delegate () { ButtonHold(button); return false; };
    //        button.GetComponentInChildren<KMSelectable>().OnInteractEnded += delegate () { ButtonRelease(button); };
    //    }

    //    SubmitButton.OnInteract += delegate () { SubmitPress(); return false; };
    //}

    //private void ButtonHold(ColouredButton button) {
    //    button.PlayPressAnimation();
    //}

    //private void ButtonRelease(ColouredButton button) {
    //    button.PlayReleaseAnimation();
    //}

    private void SubmitPress() {

    }

    private void Strike() {

    }
}
