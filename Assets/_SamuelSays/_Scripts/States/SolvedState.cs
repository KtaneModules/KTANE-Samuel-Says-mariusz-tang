using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SolvedState : State {

    private readonly int[][] _buttonPressSequence = new int[][] {
        new int[] { 0, 1, 2, 3 },
        new int[] { 0, 1, 2, 3 },
        new int[] { 0, 1 },
        new int[] { 2, 3 },
        new int[] { 0, 1 },
        new int[] { 2, 3 },
        new int[] { 0, 2 },
        new int[] { 1, 3 },
        new int[] { 0, 2 },
        new int[] { 1, 3 },
        new int[] { 0 },
        new int[] { 1 },
        new int[] { 2 },
        new int[] { 3 }
    };
    private readonly string[] _solveSounds = new string[] {
        "Solve 1",
        "Solve 2"
    };

    public SolvedState(SamuelSaysModule module) : base(module) { }

    public override IEnumerator OnStateEnter() {
        _module.Log("================== Solved ==================");
        _module.Log("Samuel thanks you for your time.");
        _module.Module.HandlePass();

        _module.StartCoroutine(EnterSolveStateAnimation());
        yield return null;
    }

    private IEnumerator EnterSolveStateAnimation() {
        _module.Audio.PlaySoundAtTransform(_solveSounds[Rnd.Range(0, _solveSounds.Length)], _module.transform);

        foreach (int[] pressSet in _buttonPressSequence) {
            foreach (int press in pressSet) {
                _module.Buttons[press].PlayPressAnimation();
            }
            _module.SymbolDisplay.DisplayEmoticon(_module.HappyFaces[Rnd.Range(0, _module.HappyFaces.Length)], Color.green);
            yield return new WaitForSeconds(0.3f);

            foreach (int press in pressSet) {
                _module.Buttons[press].PlayReleaseAnimation();
            }

            _module.SymbolDisplay.ClearScreen();
            yield return new WaitForSeconds(0.05f);
        }

        foreach (ColouredButton button in _module.Buttons) {
            button.SetMute(true);
        }
        _module.StartCoroutine(ContinuousSolveAnimation());
    }

    private IEnumerator ContinuousSolveAnimation() {
        foreach (int[] pressSet in _buttonPressSequence) {
            foreach (int press in pressSet) {
                _module.Buttons[press].PlayPressAnimation();
            }
            _module.SymbolDisplay.DisplaySolveSmile();
            yield return new WaitForSeconds(1.5f);

            foreach (int press in pressSet) {
                _module.Buttons[press].PlayReleaseAnimation();
            }

            _module.SymbolDisplay.ClearScreen();
            yield return new WaitForSeconds(0.2f);
        }

        _module.StartCoroutine(ContinuousSolveAnimation());
    }
}
