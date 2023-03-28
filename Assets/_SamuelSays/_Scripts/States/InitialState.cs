using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class InitialState : State {

    public InitialState(SamuelSaysModule module) : base(module) { }

    public override IEnumerator HandleSubmitPress() {
        _module.Log("=================================================");
        _module.Log("Conditions and actions are labelled 1-5, top-to-bottom within the given colour table.");

        _module.AdvanceStage();
        yield return null;
    }

}
