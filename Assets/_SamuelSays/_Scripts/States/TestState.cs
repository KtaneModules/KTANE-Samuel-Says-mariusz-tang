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
        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        yield return null;
    }
}
