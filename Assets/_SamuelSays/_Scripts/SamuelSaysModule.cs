using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSaysModule : MonoBehaviour {

    // TODO: Implement sequence generation and processing.
    // TODO: Add quirks.
    // TODO: Deal with stage 5.
    // TODO: Test everything.
    // TODO: Make manual.
    // TODO: Add TP.
    // TODO: Beta-testing.
    // TODO: Complete(?).
    
    [SerializeField] private ColouredButton[] _buttons;
    [SerializeField] private KMSelectable _submitButton;

    [HideInInspector] public KMBombInfo Bomb;
    [HideInInspector] public KMAudio Audio;
    [HideInInspector] public KMBombModule Module;

    private static int _moduleIdCounter = 1;
    private int _moduleId;

    // ! Move to relevant class.
    // private readonly string[] _happyFaces = new string[] {
    //     ":)",
    //     ": )",
    //     ":-)",
    //     "=)",
    //     "= )",
    //     "=-)",
    //     ":]" ,
    //     ": ]",
    //     ":-]",
    //     "=]",
    //     "= ]",
    //     "=-]"
    // };
    // private readonly string[] _hackedFaces = new string[] {
    //     ">:(",
    //     ">:[",
    //     ">:<",
    //     ":'(",
    //     ">:x",
    //     ":|",
    //     ">:|",
    //     ":s",
    //     ":o",
    //     ":0",
    //     ":O"
    // };
    // private readonly string[] _strikeFaces = new string[] {
    //     ">:(",
    //     ">:[",
    //     ">:<",
    //     ":'(",
    //     ">:x",
    //     ":|",
    //     ">:|",
    //     ":s",
    //     ":o",
    //     ":0",
    //     ":O"
    // };

    private SamuelSequenceHandler _sequenceGenerator;
    private Logger _logging;
    private State _state;

    public ColouredButton[] Buttons { get { return _buttons; } }
    public MainScreen Screen { get; private set; }
    public MiniScreen SymbolDisplay { get; private set; }

    public Stack<ColouredSymbol[]> DisplayedSequences { get; private set; }
    public int StageNumber { get; private set; }

    private void Awake() {
        _moduleId = _moduleIdCounter++;

        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Module = GetComponent<KMBombModule>();
        _logging = GetComponent<Logger>();
        Screen = GetComponentInChildren<MainScreen>();
        SymbolDisplay = GetComponentInChildren<MiniScreen>();
        _sequenceGenerator = new SamuelSequenceHandler(this);
        DisplayedSequences = new Stack<ColouredSymbol[]>();
    }

    private void Start() {
        AssignInputHandlers();
        _logging.AssignModule(Module.ModuleDisplayName, _moduleId);

        StageNumber = 0;
        AdvanceStage();
        _state = new RegularStage(this);
    }

    private void AssignInputHandlers() {
        int count = 0;

        foreach (ColouredButton button in _buttons) {
            button.Selectable.OnInteract += delegate () { StartCoroutine(_state.HandlePress(button)); return false; };
            button.Selectable.OnInteractEnded += delegate () { StartCoroutine(_state.HandleRelease(button)); };
            button.SetColour((ButtonColour)count++);
        }

        _submitButton.OnInteract += delegate () { StartCoroutine(_state.HandleSubmitPress()); return false; };
    }

    public void ChangeState(State newState) {
        _state = newState;
        StartCoroutine(_state.OnStateEnter());
    }

    public void Strike(string loggingMessage) {
        _logging.Log(loggingMessage);
        Module.HandleStrike();
    }

    public void Log(string formattedString) {
        _logging.Log(formattedString);
    }

    public void AdvanceStage() {
        DisplayedSequences.Push(_sequenceGenerator.GenerateRandomSequence());
        StageNumber++;
    }
}
