using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class DiscoloredQuirk : State {

    private readonly string[] _colourNames = new string[] {
            "red",
            "yellow",
            "green",
            "blue"
        };

    private string _displayedColour;
    private string _expectedSequenceWords;
    private string _expectedSequenceNumbers;
    private string _inputtedSequence = string.Empty;

    public DiscoloredQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        GenerateResponse();

        _module.Screen.PlaySequence(_module.DisplayedSequence);
        _module.LogQuirk("Discolored");
        _module.Log("Displayed colour is " + _displayedColour + ". Press " + _expectedSequenceWords + ".");
        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        button.PlayPressAnimation();
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        _inputtedSequence += (int)button.Colour;

        if (_inputtedSequence[_inputtedSequence.Length - 1] != _expectedSequenceNumbers[_inputtedSequence.Length - 1]) {
            string inputtedNames = string.Join(" ", _inputtedSequence.Select(num => _colourNames[num - '0']).ToArray());
            _module.Strike("Incorrectly pressed " + inputtedNames + ". Input has been reset.");
            _inputtedSequence = string.Empty;
        }
        else if (_inputtedSequence == _expectedSequenceNumbers) {
            _module.Log("Pressed the correct sequence!");
            _module.ChangeState(new RegularStage(_module, true), false);
        }
        yield return null;
    }

    private void GenerateResponse() {
        Color[] colours = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue,
            Color.magenta,
            Color.cyan
        };
        int colourIndex = Rnd.Range(0, 6);

        _module.SymbolDisplay.DisplayColour(colours[colourIndex]);
        switch (colourIndex) {
            case 0: _displayedColour = "red"; _expectedSequenceNumbers = "0012"; break;
            case 1: _displayedColour = "yellow"; _expectedSequenceNumbers = "2233"; break;
            case 2: _displayedColour = "green"; _expectedSequenceNumbers = "0132"; break;
            case 3: _displayedColour = "blue"; _expectedSequenceNumbers = "2330"; break;
            case 4: _displayedColour = "magenta"; _expectedSequenceNumbers = "0303"; break;
            case 5: _displayedColour = "cyan"; _expectedSequenceNumbers = "3112"; break;
            default: throw new ArgumentException("How the fuck has that happened then.");
        }

        _expectedSequenceWords = string.Join(" ", _expectedSequenceNumbers.Select(num => _colourNames[num - '0']).ToArray());
    }

}
