using System.Collections;

public abstract class State {
    protected SamuelSaysModule _module;

    public State(SamuelSaysModule module) {
        _module = module;
    }

    public virtual IEnumerator ColouredButtonPress(ColouredButton button) {
        yield return null;
    }

    public virtual IEnumerator SubmitPress() {
        yield return null;
    }
}
