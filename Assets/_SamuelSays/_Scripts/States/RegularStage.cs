using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegularStage : State {

    private static int _stagesLogged;

    private ButtonColour _heldButtonColour;
    private Coroutine _trackingHoldTime;
    private float _timeHeld;

    private ColouredSymbol[] _currentDisplaySequence;
    private List<ColouredSymbol> _submittedSequence = new List<ColouredSymbol>();

    public RegularStage(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _currentDisplaySequence = _module.DisplayedSequences.Peek();
        _module.Screen.PlaySequence(_currentDisplaySequence);

        if (_stagesLogged != _module.StageNumber) {
            DoStageLogging();
        }

        yield return null;
    }

    private void DoStageLogging() {
        string sequenceAsString = string.Join(", ", _module.DisplayedSequences.Peek().Select(c => c.ToString()).ToArray());
        _module.Log("Stage " + _module.StageNumber + ":");
        _module.Log("Displayed sequence is " + sequenceAsString + ".");
        _module.Log("Expected sequence is " + sequenceAsString + ".");
        _stagesLogged = _module.StageNumber;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        _heldButtonColour = button.Colour;
        _timeHeld = 0;
        _trackingHoldTime = _module.StartCoroutine(TrackHoldTime());
        _module.SymbolDisplay.DisplayLetter('•');
        yield return null;
    }

    private IEnumerator TrackHoldTime() {
        while (true) {
            _timeHeld += Time.deltaTime;

            if (_timeHeld >= 0.5f) {
                _module.SymbolDisplay.DisplayLetter('ー');
            }
            yield return null;
        }
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        _module.StopCoroutine(_trackingHoldTime);
        button.PlayReleaseAnimation();
        _module.SymbolDisplay.ClearScreen();

        char inputtedSymbol = _timeHeld >= 0.5f ? '-' : '.';
        _submittedSequence.Add(new ColouredSymbol(_heldButtonColour, inputtedSymbol));

        yield return null;
    }

    public override IEnumerator HandleSubmitPress() {
        string sequenceAsString = string.Join(", ", _submittedSequence.Select(c => c.ToString()).ToArray());
        _module.Log("Inputted " + sequenceAsString + ".");
        _submittedSequence.Clear();
        yield return null;
    }
}
