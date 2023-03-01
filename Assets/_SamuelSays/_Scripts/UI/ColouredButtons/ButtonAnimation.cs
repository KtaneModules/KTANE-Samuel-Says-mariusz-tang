using System.Collections;

public abstract class ButtonAnimation {

    private ColouredButton[] _buttons;

    protected ButtonAnimation(ColouredButton[] buttons) {
        _buttons = buttons;
    }

    public abstract IEnumerator PlayAnimation();
}
