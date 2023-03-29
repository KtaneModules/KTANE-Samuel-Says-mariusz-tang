using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class GhostQuirk : State {

    public GhostQuirk(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Screen.PlaySequence(_module.DisplayedSequence);
        yield return null;
    }

    public IEnumerator GhostPresses() {
        yield return null;
    }

}
