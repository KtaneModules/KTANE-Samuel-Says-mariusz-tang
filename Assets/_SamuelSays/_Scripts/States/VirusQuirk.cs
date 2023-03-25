using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirusQuirk : State {

    private readonly string[] _hackedFaces = new string[] {
        ">:(",
        ">:[",
        ">:<",
        ":'(",
        ">:x",
        ":|",
        ">:|",
        ":s",
        ":o",
        ":0",
        ":O"
    };

    private string _inputtedSequence;

    public VirusQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        switch (_module.StageNumber) {
            case 2: _inputtedSequence = "1234"; break;
            case 3: _inputtedSequence = "3412"; break;
            case 4: _inputtedSequence = "2413"; break;
            default: throw new ArgumentOutOfRangeException("WTF STAGE NUMBER ARE WE ON :(");
        }

        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        return base.HandlePress(button);
    }

    private IEnumerator ConvertButtons() {
        yield return null;
    }

    private IEnumerator FlashVirusFace() {
        yield return null;
    }
}
