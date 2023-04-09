using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class GhostQuirk : State {

    private bool _done;

    private string _inputtedSequence;
    private string _expectedSequence;
    private Coroutine _ghostPresses;

    public GhostQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Screen.PlaySequence(_module.DisplayedSequence);
        _inputtedSequence = string.Empty;
        _expectedSequence = string.Empty;
        _done = false;

        GenerateRandomSequence();
        _ghostPresses = _module.StartCoroutine(DoGhostPresses());

        _module.LogQuirk("Ghost");
        _module.Log("The flashing sequence is " + _expectedSequence + " (reading order).");
        yield return null;
    }

    private IEnumerator DoGhostPresses() {
        foreach (char press in _expectedSequence) {
            yield return new WaitForSeconds(0.3f);
            _module.Buttons[press - '1'].PlayPressAnimation();
            yield return new WaitForSeconds(0.2f);
            _module.Buttons[press - '1'].PlayReleaseAnimation();
        }

        yield return new WaitForSeconds(1);
        _ghostPresses = _module.StartCoroutine(DoGhostPresses());
    }

    private void GenerateRandomSequence() {
        int length = Rnd.Range(5, 8);
        for (int i = 0; i < length; i++) {
            _expectedSequence += Rnd.Range(1, 5);
        }
    }

    public override IEnumerator HandlePress(ColouredButton pressedButton) {
        if (_ghostPresses != null) {
            _module.StopCoroutine(_ghostPresses);
            foreach (ColouredButton button in _module.Buttons) {
                button.PlayReleaseAnimation();
            }
        }

        _inputtedSequence += (int)pressedButton.Colour + 1;
        pressedButton.PlayPressAnimation();
        yield return null;
    }

    public override IEnumerator HandleRelease(ColouredButton button) {
        button.PlayReleaseAnimation();
        if (_done) {
            yield break;
        }

        if (_inputtedSequence[_inputtedSequence.Length - 1] != _expectedSequence[_inputtedSequence.Length - 1]) {
            _module.Strike("Incorrectly pressed " + _inputtedSequence + "! Input has been reset.");
            _inputtedSequence = string.Empty;
            _ghostPresses = _module.StartCoroutine(DoGhostPresses());
        }
        else if (_inputtedSequence == _expectedSequence) {
            _module.Log("Pressed the correct sequence!");
            _module.StartCoroutine(FlashAllButtons());
            _done = true;
        }

        yield return null;
    }

    private IEnumerator FlashAllButtons() {
        for (int i = 0; i < 4; i++) {
            foreach (ColouredButton button in _module.Buttons) {
                button.PlayPressAnimation();
            }
            yield return new WaitForSeconds(0.1f);

            foreach (ColouredButton button in _module.Buttons) {
                button.PlayReleaseAnimation();
            }
            yield return new WaitForSeconds(0.1f);
        }

        _module.ChangeState(new RegularStage(_module, true), false);
        yield return null;
    }

    public override IEnumerator HandleSubmitPress() {
        _module.SubmitButtonAnimator.SetBool("IsPressed", true);
        if (_done) {
            yield break;
        }

        if (_ghostPresses != null) {
            _module.StopCoroutine(_ghostPresses);
            foreach (ColouredButton button in _module.Buttons) {
                button.PlayReleaseAnimation();
            }
        }
        else {
            _module.Screen.ToggleMute();
            foreach (ColouredButton button in _module.Buttons) {
                button.ToggleMute();
            }
        }

        _inputtedSequence = string.Empty;
        yield return new WaitForSeconds(1);
        _ghostPresses = _module.StartCoroutine(DoGhostPresses());
        yield return null;
    }

    public override TpAction NextTpAction() {
        if (_done) {
            return new TpAction(TpActionType.Wait);
        }
        
        int position = _inputtedSequence.Length;
        return new TpAction(TpActionType.PressShort, _expectedSequence[position] - '1');
    }
}
