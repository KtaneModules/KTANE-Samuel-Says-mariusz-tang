using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class FinalStage : State {

    private const float DashTime = 0.3f;

    private Dictionary<char, string> _letterToNato = new Dictionary<char, string> {
        {'A', "ALFA"},
        {'B', "BRAVO"},
        {'C', "CHARLIE"},
        {'D', "DELTA"},
        {'E', "ECHO"},
        {'F', "FOXTROT"},
        {'G', "GOLF"},
        {'H', "HOTEL"},
        {'I', "INDIA"},
        {'J', "JULIETT"},
        {'K', "KILO"},
        {'L', "LIMA"},
        {'M', "MIKE"},
        {'N', "NOVEMBER"},
        {'O', "OSCAR"},
        {'P', "PAPA"},
        {'Q', "QUEBEC"},
        {'R', "ROMEO"},
        {'S', "SIERRA"},
        {'T', "TANGO"},
        {'U', "UNIFORM"},
        {'V', "VICTOR"},
        {'W', "WHISKEY"},
        {'X', "XRAY"},
        {'Y', "YANKEE"},
        {'Z', "ZULU"}
    };
    private Dictionary<char, string> _letterToMorse = new Dictionary<char, string> {
        {'A', ".-"},
        {'B', "-..."},
        {'C', "-.-."},
        {'D', "-.."},
        {'E', "."},
        {'F', "..-."},
        {'G', "--."},
        {'H', "...."},
        {'I', ".."},
        {'J', ".---"},
        {'K', "-.-"},
        {'L', ".-.."},
        {'M', "--"},
        {'N', "-."},
        {'O', "---"},
        {'P', ".--."},
        {'Q', "--.-"},
        {'R', ".-."},
        {'S', "..."},
        {'T', "-"},
        {'U', "..-"},
        {'V', "...-"},
        {'W', ".--"},
        {'X', "-..-"},
        {'Y', "-.--"},
        {'Z', "--.."}
    };
    private Dictionary<string, char> _morseToLetter;

    private List<ColouredSymbol> _expectedSubmission = new List<ColouredSymbol>();
    private List<ColouredSymbol> _inputtedSubmission = new List<ColouredSymbol>();
    private List<int> _letterEndings = new List<int>();

    private string _submitWord = string.Empty;

    private ButtonColour _heldButtonColour;
    private float _timeHeld;
    private bool _isHolding;
    private bool _isDisplayingSequence = true;

    private Coroutine _flashingRecoverySequence;

    public FinalStage(SamuelSaysModule module) : base(module) {
        // Inverse dictionary.
        _morseToLetter = _letterToMorse.ToDictionary((i) => i.Value, (i) => i.Key);
    }

    public override IEnumerator OnStateEnter() {
        _module.Screen.PlaySequence(_module.DisplayedSequence);

        string sequenceAsString = string.Join(", ", _module.DisplayedSequence.Select(c => c.ToString()).ToArray());
        _module.Log("================== Stage 5 ==================");
        _module.Log("The displayed sequence is " + sequenceAsString + ".");
        GenerateExpectedSubmission();

        return base.OnStateEnter();
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        if (_flashingRecoverySequence != null) {
            _module.StopCoroutine(_flashingRecoverySequence);
            foreach (ColouredButton buttonToBeReleased in _module.Buttons) {
                buttonToBeReleased.PlayReleaseAnimation();
            }
        }

        button.PlayPressAnimation();

        if (_isDisplayingSequence) {
            _module.Screen.StopSequence();
        }

        _heldButtonColour = button.Colour;
        _timeHeld = 0;
        _module.StartCoroutine(TrackHoldTime());
        _module.SymbolDisplay.DisplayLetter('•');
        yield return null;
    }

    private IEnumerator TrackHoldTime() {
        _isHolding = true;

        while (_isHolding) {
            _timeHeld += Time.deltaTime;

            if (_timeHeld >= DashTime) {
                _module.SymbolDisplay.DisplayLetter('ー');
            }
            yield return null;
        }
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        if (!_isHolding) {
            yield break;
        }

        _isHolding = false;
        button.PlayReleaseAnimation();
        _module.SymbolDisplay.ClearScreen();

        char inputtedSymbol = _timeHeld >= DashTime ? '-' : '.';
        _inputtedSubmission.Add(new ColouredSymbol(_heldButtonColour, inputtedSymbol));
        CheckSubmission();
        yield return null;
    }

    private void CheckSubmission() {
        int submissionLength = _inputtedSubmission.Count();

        if (!_inputtedSubmission[submissionLength - 1].Equals(_expectedSubmission[submissionLength - 1])) {
            _module.Strike("Incorrectly transmitted a " + _inputtedSubmission[submissionLength - 1].ToString()
                 + " at position " + submissionLength + "! Input has been reset.");
            _inputtedSubmission.Clear();
            _module.Screen.PlaySequence(_module.DisplayedSequence);
            _flashingRecoverySequence = _module.StartCoroutine(FlashRecoverySequence());
        }
        else if (_letterEndings.Contains(submissionLength)) {
            _module.SymbolDisplay.DisplayLetter(_submitWord[_letterEndings.IndexOf(submissionLength)]);
        }

        if (submissionLength == _expectedSubmission.Count()) {
            _module.Log("Transmitted the correct sequence!");
            _module.ChangeState(new SolvedState(_module));
        }
    }

    private IEnumerator FlashRecoverySequence() {
        foreach (ButtonColour colour in _module.SubmittedColours) {
            _module.Buttons[(int)colour].PlayPressAnimation();
            yield return new WaitForSeconds(0.8f);
            _module.Buttons[(int)colour].PlayReleaseAnimation();
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    private void GenerateExpectedSubmission() {
        List<int> quirks = _module.QuirkAppearances;
        int quirksCount = quirks.Count();
        int quirksDistinct = quirks.Distinct().Count();
        string displayedSymbols = new string(_module.DisplayedSequence.Select(symbol => symbol.Symbol).ToArray());

        if (!_morseToLetter.ContainsKey(displayedSymbols)) {
            _module.Log("The displayed sequence does not form a letter in the alphabet.");
            _submitWord = "SAMUEL";
        }
        else if (quirksCount == 1) {
            _module.Log("There was one quirk. Removing the first symbol.");
            displayedSymbols = displayedSymbols.Remove(0, 1);
        }
        else if (quirksCount == 2) {
            if (quirksDistinct == 2) {
                _module.Log("There were two distinct quirks. Shifting right twice.");
                displayedSymbols = displayedSymbols.Substring(displayedSymbols.Length - 2) + displayedSymbols.Substring(0, displayedSymbols.Length - 2);
            }
            else {
                _module.Log("There were two of the same quirk. Reversing the sequence.");
                displayedSymbols = new string(displayedSymbols.Reverse().ToArray());
            }
        }
        else if (quirksCount == 3) {
            if (quirksDistinct == 3) {
                _module.Log("There were three distinct quirks. Removing the first three symbols.");
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
            }
            else if (quirksDistinct == 2) {
                _module.Log("There were three quirks, two of which were the same. Removing the third symbol.");
                displayedSymbols.Remove(2, 1);
            }
            else {
                _module.Log("There were three of the same quirk. Removing the first two symbols.");
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
            }
        }

        if (_submitWord == string.Empty) {
            if (displayedSymbols == string.Empty) {
                _module.Log("The sequence is now empty.");
                _submitWord = "MORSE";
            }
            else if (!_morseToLetter.ContainsKey(displayedSymbols)) {
                _module.Log("The displayed sequence now does not form a letter in the alphabet.");
                _submitWord = "MORSE";
            }
            else {
                _submitWord = _letterToNato[_morseToLetter[displayedSymbols]];
            }
        }

        _module.Log("The expected word to submit is " + _submitWord + ".");
        _module.Log("The expected colour sequence is " + string.Join(" ", _module.SubmittedColours.Select(s => s.ToString().ToLower()).ToArray()) + ".");

        int counter = 0;
        foreach (char letter in _submitWord) {
            foreach (char symbol in _letterToMorse[letter]) {

                _expectedSubmission.Add(new ColouredSymbol(_module.SubmittedColours[counter % 4], symbol));
                counter++;
            }
            _letterEndings.Add(counter);
        }
    }

    public override TpAction NextTpAction() {
        int position = _inputtedSubmission.Count();

        if (_expectedSubmission[position].Symbol == '.') {
            return new TpAction(TpActionType.PressShort, (int)_expectedSubmission[position].Colour);
        }
        else {
            return new TpAction(TpActionType.PressLong, (int)_expectedSubmission[position].Colour);
        }
    }
}
