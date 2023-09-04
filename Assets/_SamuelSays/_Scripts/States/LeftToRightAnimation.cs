using System.Collections;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class LeftToRightAnimation : State {

    private const float FLASH_TIME = 0.1f;
    private const int CYCLE_COUNT = 3;

    private readonly State _nextState;

    private bool _faceOn = false;

    public LeftToRightAnimation(SamuelSaysModule module, State nextState) : base(module) {
        _nextState = nextState;
    }

    public override IEnumerator OnStateEnter() {
        for (int i = 0; i < CYCLE_COUNT; i++) {
            foreach (ColouredButton button in _module.Buttons) {
                button.PlayPressAnimation();
                ToggleFace();
                yield return new WaitForSeconds(FLASH_TIME);
                button.PlayReleaseAnimation();
            }
        }

        _module.ChangeState(_nextState);
    }

    private void ToggleFace() {
        if (_faceOn) {
            _module.SymbolDisplay.ClearScreen();
        }
        else {
            _module.SymbolDisplay.DisplayEmoticon(_module.HappyFaces[Rnd.Range(0, _module.HappyFaces.Length)], Color.green);
        }
        _faceOn = !_faceOn;
    }

    public override TpAction NextTpAction() {
        return new TpAction(TpActionType.Wait);
    }
}
