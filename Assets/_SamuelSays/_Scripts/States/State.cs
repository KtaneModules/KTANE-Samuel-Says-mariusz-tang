using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is my first time trying to implement a state machine, and now looking back at
// it I see things that I would love to improve but I don't think it is worth it right now :')
public abstract class State {

    protected readonly SamuelSaysModule _module;

    protected State(SamuelSaysModule module) {
        _module = module;
        _module.StartCoroutine(OnStateEnter());
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
        _module.Screen.ToggleMute();
        yield return null;
    }
}
