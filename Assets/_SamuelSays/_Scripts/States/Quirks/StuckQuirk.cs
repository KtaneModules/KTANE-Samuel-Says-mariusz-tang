using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class StuckQuirk : State {

    private int _stuckButton;

    public StuckQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _stuckButton = Rnd.Range(0, 4);
        _module.Buttons[_stuckButton].PlayPressAnimation(false);
        _module.Screen.PlaySequence(_module.DisplayedSequence);

        _module.LogQuirk("Stuck");
        _module.Log("Button " + (_stuckButton + 1) + " is stuck. Touch this button at the right time.");
        yield return null;
    }

    public override IEnumerator HandlePress(ColouredButton button) {
        if ((int)button.Colour != _stuckButton) {
            _module.Strike("Touched an incorrect button!");
        }
        else if ((Math.Floor(_module.Bomb.GetTime()) - _module.Bomb.GetSolvedModuleNames().Count()) % 10 != 0) {
            _module.Strike("Incorrectly touched the button at " + _module.Bomb.GetFormattedTime()
                 + " with " + _module.Bomb.GetSolvedModuleNames().Count() + " solved modules!", "StuckQuirk Zap");
        }
        else {
            _module.Log("Touched the correct button!");
            _module.Buttons[_stuckButton].PlayReleaseAnimation();
            _module.Audio.PlaySoundAtTransform("StuckQuirk Zap", _module.transform);
            button.AddInteractionPunch();
            _module.ChangeState(new RegularStage(_module, true), false);
        }

        yield return null;
    }

    public override TpAction NextTpAction() {
        return new TpAction(TpActionType.PressStuck, _stuckButton);
    }
}
