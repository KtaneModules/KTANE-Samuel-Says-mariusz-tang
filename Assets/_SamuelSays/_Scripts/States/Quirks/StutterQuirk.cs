using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class StutterQuirk : State {

    private List<ColouredSymbol> _stutterSequence = new List<ColouredSymbol>();
    private int[] _colourCounts;

    private bool _done;

    public StutterQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _done = false;
        _colourCounts = new int[4];
        GenerateStutterSequence();
        _module.Screen.PlaySequence(_stutterSequence.ToArray());

        string logString = string.Join(" | ", _colourCounts.Select((count, index) => (ButtonColour)index + ": " + count).ToArray());

        _module.LogQuirk("Stutter");
        _module.Log("Number of flashes per colour: " + logString + ".");
        _module.Log("Press a colour that flashes " + _colourCounts.Max() + " times.");

        yield return null;
    }

    private void GenerateStutterSequence() {
        foreach (ColouredSymbol symbol in _module.DisplayedSequence) {
            int repeats = Rnd.Range(2, 5);

            for (int i = 0; i < repeats; i++) {
                _stutterSequence.Add(symbol);
                _colourCounts[(int)symbol.Colour]++;
            }
        }
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        if (_done) {
            yield break;
        }

        if (_colourCounts[(int)button.Colour] == _colourCounts.Max()) {
            _module.Log("Pressed a valid colour!");
            _done = true;
            _module.Screen.StopSequence();
            button.AddInteractionPunch();
            _module.SymbolDisplay.DisplayEmoticon(_module.HappyFaces[Rnd.Range(0, _module.HappyFaces.Length)], Color.green);
            yield return new WaitForSeconds(0.2f);
            _module.ChangeState(new RegularStage(_module));
        }
        else {
            _module.Strike("Incorrectly pressed " + button.Colour.ToString().ToLower() + "!");
        }
        yield return null;
    }
}
