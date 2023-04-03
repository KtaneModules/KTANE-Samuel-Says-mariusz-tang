using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class UnstableQuirk : State {

    private int _expectedPress;

    public UnstableQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        GenerateAndDisplayUnstableSequences();

        _module.LogQuirk("Unstable");
        _module.Log("Position " + (_expectedPress + 1) + " in the sequence is unstable. Press button " + (_expectedPress + 1) + ".");
        yield return null;
    }

    private void GenerateAndDisplayUnstableSequences() {
        _expectedPress = Rnd.Range(0, _module.DisplayedSequence.Count());

        ColouredSymbol[] unstableSequence = _module.DisplayedSequence.ToArray();
        ButtonColour symbolColour = unstableSequence[_expectedPress].Colour;

        if (unstableSequence[_expectedPress].Symbol == '-') {
            unstableSequence[_expectedPress] = new ColouredSymbol(symbolColour, '.');
        }
        else {
            unstableSequence[_expectedPress] = new ColouredSymbol(symbolColour, '-');
        }

        _module.Screen.PlaySequences(new List<ColouredSymbol[]> { _module.DisplayedSequence, unstableSequence });
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();

        if ((int)button.Colour == _expectedPress) {
            _module.Log("Pressed the correct button!");
            button.AddInteractionPunch();
            _module.ChangeState(new RegularStage(_module));
        }
        else {
            _module.Strike("Pressed an incorrect button!");
        }
        yield return null;
    }

}
