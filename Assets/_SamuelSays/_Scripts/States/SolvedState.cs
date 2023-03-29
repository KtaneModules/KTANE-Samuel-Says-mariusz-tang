using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SolvedState : State {

    public SolvedState(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Log("================== Solved ==================");
        _module.Log("Samuel thanks you for your time.");
        _module.Module.HandlePass();

        // ! Need to implement solve animation.
        yield return null;
    }
}
