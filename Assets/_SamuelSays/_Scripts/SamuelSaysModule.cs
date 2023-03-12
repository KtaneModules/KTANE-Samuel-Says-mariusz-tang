using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSaysModule : MonoBehaviour {

    // TODO: Implement input system.
    // TODO: Implement sequence generation and processing.
    // TODO: Add quirks.
    // TODO: Deal with stage 5.
    // TODO: Test everything.
    // TODO: Make manual.
    // TODO: Add TP.
    // TODO: Beta-testing.
    // TODO: Complete(?).

    // ! I am gonna go sleep now, but I think that (^^) is an accurate roadmap of whats gonna happen with Samuel :)

    // Add emote flash sequence to MiniScreen.cs.
    // Add play display sequence to MainScreen.cs.
    // Add button dance.

    [SerializeField] private ColouredButton[] _buttons;
    [SerializeField] private KMSelectable _submitButton;
    [SerializeField] private KMSelectable _miniScreenButton;

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

    private MainScreen _screen;
    private MiniScreen _symbolDisplay;

    private Logger _logging;
    private State _state;
    private List<ColouredSymbol[]> _displayedSequences;
    private SamuelSequenceHandler _sequenceGenerator;

    public ColouredButton[] Buttons { get { return _buttons; } }
    public List<ColouredSymbol[]> DisplayedSequences { get { return _displayedSequences; } }
    public SamuelSequenceHandler SequenceGenerator { get { return _sequenceGenerator; } }
    public MainScreen Screen { get { return _screen; } }
    public MiniScreen SymbolDisplay { get { return _symbolDisplay; } }

    private void Awake() {
        _moduleId = _moduleIdCounter++;

        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Module = GetComponent<KMBombModule>();
        _logging = GetComponent<Logger>();
        _screen = GetComponentInChildren<MainScreen>();
        _symbolDisplay = GetComponentInChildren<MiniScreen>();
        _sequenceGenerator = new SamuelSequenceHandler(this);
    }

    private void Start() {
        AssignInputHandlers();
        _logging.AssignModule(Module.ModuleDisplayName, _moduleId);
        _state = new TestState(this);
    }

    private void AssignInputHandlers() {
        int count = 0;

        foreach (ColouredButton button in _buttons) {
            button.Selectable.OnInteract += delegate () { StartCoroutine(_state.HandlePress(button)); return false; };
            button.Selectable.OnInteractEnded += delegate () { StartCoroutine(_state.HandleRelease(button)); };
            button.SetColour((ButtonColour)count++);
        }

        _submitButton.OnInteract += delegate () { StartCoroutine(_state.HandleSubmitPress()); return false; };
        _miniScreenButton.OnInteract += delegate () { StartCoroutine(_state.HandleMiniScreenPress()); return false; };
        _miniScreenButton.OnInteractEnded += delegate () { StartCoroutine(_state.HandleMiniScreenRelease()); };
    }

    public void ChangeState(State newState) {
        _state = newState;
        StartCoroutine(_state.OnStateEnter());
    }

    public void Strike(string loggingMessage) {
        _logging.Log(loggingMessage);
        Module.HandleStrike();
    }
}
