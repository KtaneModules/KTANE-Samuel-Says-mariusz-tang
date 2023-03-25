using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSaysModule : MonoBehaviour {

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

    private SamuelSequenceGenerator _sequenceGenerator;
    private SamuelSequenceModifier _sequenceModifier;
    private State _state;

    public ColouredButton[] Buttons { get { return _buttons; } }
    public MainScreen Screen { get; private set; }
    public MiniScreen SymbolDisplay { get; private set; }

    public ColouredSymbol[] DisplayedSequence { get; private set; }
    public ColouredSymbol ExpectedSubmission { get; private set; }
    public int StageNumber { get; private set; }

    private void Awake() {
        _moduleId = _moduleIdCounter++;

        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Module = GetComponent<KMBombModule>();
        Screen = GetComponentInChildren<MainScreen>();
        SymbolDisplay = GetComponentInChildren<MiniScreen>();
        _sequenceGenerator = new SamuelSequenceGenerator();
        _sequenceModifier = new SamuelSequenceModifier(this);
    }

    private void Start() {
        AssignInputHandlers();

        Log("Samuel says hi!");

        StageNumber = 0;
        AdvanceStage();
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
        Log(loggingMessage);
        Module.HandleStrike();
    }

    public void Log(string formattedString) {
        Debug.LogFormat("[Samuel Says #{0}] {1}", _moduleId, formattedString);
    }

    public void AdvanceStage() {
        StageNumber++;
        DisplayedSequence = _sequenceGenerator.GenerateRandomSequence(3 + Rnd.Range(0, 2));
        ExpectedSubmission = _sequenceModifier.GetExpectedSubmission(DisplayedSequence);

        string sequenceAsString = string.Join(", ", DisplayedSequence.Select(c => c.ToString()).ToArray());
        Log("=================================================");
        Log("Stage " + StageNumber + ":");
        Log("Displayed sequence is " + sequenceAsString + ".");
        Log("Expected sequence is " + ExpectedSubmission.ToString() + ".");

        ChangeState(new RegularStage(this));
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} 0-8 in reading order to select/deselect cubes. Use !{0} s1/2/3 to press stage lights. " +
                                                    "Use !{0} screen to press the screen button. Chain commands together with spaces.";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command) {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve() {
        yield return null;
    }
}
