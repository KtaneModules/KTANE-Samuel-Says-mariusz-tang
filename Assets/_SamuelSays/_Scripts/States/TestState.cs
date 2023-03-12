using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TestState : State {

    public TestState(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Screen.PlaySequence(new List<ColouredSymbol[]> { _module.SequenceGenerator.GenerateRandomSequence(), _module.SequenceGenerator.GenerateRandomSequence() });
        _module.SymbolDisplay.DisplayColour(Color.white);
        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        _module.SymbolDisplay.DisplayEmoticon(":)", Color.green);
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        _module.SymbolDisplay.DisplayColour(Color.white);
        yield return null;
    }

    public override IEnumerator HandleMiniScreenPress() {
        _module.SymbolDisplay.DisplayLetter('Ü');
        yield return null;
    }

    public override IEnumerator HandleMiniScreenRelease() {
        _module.SymbolDisplay.DisplayColour(Color.white);
        yield return null;
    }

    public override IEnumerator HandleSubmitPress() {
        _module.Screen.StopSequence();
        _module.SymbolDisplay.ClearScreen();
        _module.ChangeState(new LeftToRightAnimation(_module, this));
        yield return null;
    }
}
