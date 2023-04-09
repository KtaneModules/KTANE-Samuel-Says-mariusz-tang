using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegularStage : State {

    private const float DashTime = 0.3f;

    private ButtonColour _heldButtonColour;
    private float _timeHeld;
    private bool _isHolding;
    private bool _startDisplaySequence;

    private ColouredSymbol[] _currentDisplaySequence;

    public RegularStage(SamuelSaysModule module) : this(module, false) { }

    public RegularStage(SamuelSaysModule module, bool sequenceAlreadyPlaying) : base(module) {
        _startDisplaySequence = !sequenceAlreadyPlaying;
    }

    public override IEnumerator OnStateEnter() {
        _currentDisplaySequence = _module.DisplayedSequence;

        if (_startDisplaySequence) {
            _module.Screen.PlaySequence(_currentDisplaySequence);
        }
        _module.DoStageLogging();

        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
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
        var submittedSymbol = new ColouredSymbol(_heldButtonColour, inputtedSymbol);

        CheckSubmission(submittedSymbol);
        yield return null;
    }

    private void CheckSubmission(ColouredSymbol submittedSymbol) {
        if (submittedSymbol.Equals(_module.ExpectedSubmission)) {
            _module.Log("Transmitted the correct symbol!");
            _module.AdvanceStage();
        }
        else {
            _module.Strike("Incorrectly submitted a " + submittedSymbol.ToString() + "!");
        }
    }

    public override TpAction NextTpAction() {
        if (_module.ExpectedSubmission.Symbol == '.') {
            return new TpAction(TpActionType.PressShort, (int)_module.ExpectedSubmission.Colour);
        }
        else {
            return new TpAction(TpActionType.PressLong, (int)_module.ExpectedSubmission.Colour);
        }
    }
}
