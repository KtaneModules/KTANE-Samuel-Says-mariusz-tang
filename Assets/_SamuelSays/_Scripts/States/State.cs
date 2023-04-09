using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is my first time trying to implement a state machine, and now looking back at
// it I see things that I would love to improve but I don't think it is worth it right now :')
public abstract class State {

    protected readonly SamuelSaysModule _module;

    protected State(SamuelSaysModule module) {
        _module = module;
    }

    public virtual IEnumerator OnStateEnter() {
        yield return null;
    }

    public virtual IEnumerator HandlePress(ColouredButton button) {
        yield return null;
    }

    public virtual IEnumerator HandleRelease(ColouredButton button) {
        yield return null;
    }

    public virtual IEnumerator HandleSubmitPress() {
        _module.SubmitButtonAnimator.SetBool("IsPressed", true);
        _module.Screen.ToggleMute();
        foreach (ColouredButton button in _module.Buttons) {
            button.ToggleMute();
        }
        yield return null;
    }

    public virtual IEnumerator HandleSubmitRelease() {
        _module.SubmitButtonAnimator.SetBool("IsPressed", false);
        yield return null;
    }

    public abstract TpAction NextTpAction();
}
