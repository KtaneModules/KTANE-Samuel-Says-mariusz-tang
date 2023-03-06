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

    [SerializeField] private ColouredButton[] _buttons;
    [SerializeField] private KMSelectable _submitButton;

    [HideInInspector] public KMBombInfo Bomb;
    [HideInInspector] public KMAudio Audio;
    [HideInInspector] public KMBombModule Module;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _moduleSolved = false;


    // These need to be moved to their relevant classes.
    private const float SingleMorseUnit = 0.2f;
    private const float EmoticonFlashTime = 0.3f;
    private const int EmoticonFlashCount = 3;
    private const string DotDash = "•ー";

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

    private Logger _logging;
    private State _state;
    private List<ColouredSymbol[]> _displayedSequences;

    public ColouredButton[] Buttons { get { return _buttons; } }
    public List<ColouredSymbol[]> DisplayedSequences { get { return _displayedSequences; } }

    void Awake() {
        _moduleId = _moduleIdCounter++;

        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Module = GetComponent<KMBombModule>();
        _logging = GetComponent<Logger>();
        _state = new TestState(this);
    }

    void Start() {
        int count = 0;

        foreach (ColouredButton button in _buttons) {
            button.Selectable.OnInteract += delegate () { HandlePress(button); return false; };
            button.Selectable.OnInteractEnded += delegate () { HandleRelease(button); };
            button.SetColour((ButtonColour)count++);
        }

        _logging.AssignModule(Module.ModuleDisplayName, _moduleId);
    }

    public void ChangeState(State newState) {
        _state = newState;
    }

    private void HandlePress(ColouredButton button) {
        StartCoroutine(_state.HandlePress(button));
    }

    private void HandleRelease(ColouredButton button) {
        StartCoroutine(_state.HandleRelease(button));
    }
}
