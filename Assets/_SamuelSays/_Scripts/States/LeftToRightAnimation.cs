using System.Collections;
using UnityEngine;

public class LeftToRightAnimation : State {

    private const float FLASH_TIME = 0.1f;
    private const int CYCLE_COUNT = 3;

    private readonly State _nextState;

    public LeftToRightAnimation(SamuelSaysModule module, State nextState) : base(module) {
        _nextState = nextState;
    }

    public override IEnumerator OnStateEnter() {
        for (int i = 0; i < CYCLE_COUNT; i++) {
            foreach (ColouredButton button in _module.Buttons) {
                button.PlayPressAnimation();
                yield return new WaitForSeconds(FLASH_TIME);
                button.PlayReleaseAnimation();
            }
        }

        _module.ChangeState(_nextState);
    }

}