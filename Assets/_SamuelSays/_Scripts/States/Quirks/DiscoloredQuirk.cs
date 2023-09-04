using System;
using System.Collections;
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

    private bool _done;

    private string _displayedColour;
    private string _expectedSequenceWords;
    private string _expectedSequenceNumbers;
    private string _inputtedSequence;

    public DiscoloredQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _done = false;
        GenerateResponse();
        _inputtedSequence = string.Empty;

        _module.Screen.PlaySequence(_module.DisplayedSequence);
        _module.LogQuirk("Discolored");
        _module.Log("The displayed colour is " + _displayedColour + ". Press " + _expectedSequenceWords + ".");
        yield return null;
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

        _inputtedSequence += (int)button.Colour;

        if (_inputtedSequence[_inputtedSequence.Length - 1] != _expectedSequenceNumbers[_inputtedSequence.Length - 1]) {
            string inputtedNames = string.Join(" ", _inputtedSequence.Select(num => _colourNames[num - '0']).ToArray());
            _module.Strike("Incorrectly pressed " + inputtedNames + ". Input has been reset.");
            _inputtedSequence = string.Empty;
        }
        else if (_inputtedSequence == _expectedSequenceNumbers) {
            _module.Log("Pressed the correct sequence!");
            _done = true;
            _module.SymbolDisplay.DisplayEmoticon(_module.HappyFaces[Rnd.Range(0, _module.HappyFaces.Length)], Color.green);
            yield return new WaitForSeconds(0.5f);
            _module.ChangeState(new RegularStage(_module, true), false);
            button.AddInteractionPunch();
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

        switch (colourIndex) {
            case 0: _displayedColour = "Red"; _expectedSequenceNumbers = "0012"; break;
            case 1: _displayedColour = "Yellow"; _expectedSequenceNumbers = "2233"; break;
            case 2: _displayedColour = "Green"; _expectedSequenceNumbers = "0132"; break;
            case 3: _displayedColour = "Blue"; _expectedSequenceNumbers = "2330"; break;
            case 4: _displayedColour = "Magenta"; _expectedSequenceNumbers = "0303"; break;
            case 5: _displayedColour = "Cyan"; _expectedSequenceNumbers = "3112"; break;
            default: throw new ArgumentException("How the fuck has that happened then.");
        }

        _module.SymbolDisplay.DisplayColour(colours[colourIndex], _displayedColour);
        _displayedColour = _displayedColour.ToLower();

        _expectedSequenceWords = string.Join(" ", _expectedSequenceNumbers.Select(num => _colourNames[num - '0']).ToArray());
    }

    public override TpAction NextTpAction() {
        if (_done) {
            return new TpAction(TpActionType.Wait);
        }

        int position = _inputtedSequence.Length;
        return new TpAction(TpActionType.PressShort, _expectedSequenceNumbers[position] - '0');
    }

}
