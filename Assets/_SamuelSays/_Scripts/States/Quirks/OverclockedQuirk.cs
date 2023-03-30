using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class OverclockedQuirk : State {

    // ! Continue with dealing with what happens after limit is reached.

    private const float DECREASE_RATE = 1f;
    private const float INCREMENT_SIZE = 0.25f;

    private float _spamTotal = 0;
    private bool _hitLimit = false;

    private Coroutine _trackSpam;

    public OverclockedQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Screen.PlaySequence(_module.DisplayedSequence, true);

        _module.LogQuirk("Overclocked");
        _module.Log("Press buttons rapidly until Samuel calms down.");
        _trackSpam = _module.StartCoroutine(TrackSpam());
        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        _spamTotal = Math.Min(1, _spamTotal + INCREMENT_SIZE);
        if (_spamTotal == 1 && !_hitLimit) {
            _hitLimit = true;
            button.AddInteractionPunch(10);
            _module.Log("Limit reached");
        }
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        yield return null;
    }

    private IEnumerator TrackSpam() {
        while (true) {
            _spamTotal = Math.Max(0, _spamTotal - Time.deltaTime * DECREASE_RATE);
            _module.SymbolDisplay.DisplayColour(Color.white * _spamTotal);
            yield return null;
        }
    }
}
