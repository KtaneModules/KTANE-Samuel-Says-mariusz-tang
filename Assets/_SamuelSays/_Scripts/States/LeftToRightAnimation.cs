using System.Collections;

public class LeftToRightAnimation : State {

    private readonly State _nextState;

    public LeftToRightAnimation(SamuelSaysModule module, State nextState) : base(module) {
        _nextState = nextState;
    }

    public override IEnumerator OnStateEnter() {
        // ! Put animation stuff here.
        yield return null;
        _module.ChangeState(_nextState);
    }

}