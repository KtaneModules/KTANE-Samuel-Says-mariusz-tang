using System.Collections;

public class InitialState : State {

    public InitialState(SamuelSaysModule module) : base(module) { }

    public override IEnumerator HandleSubmitPress() {
        _module.SubmitButtonAnimator.SetBool("IsPressed", true);
        _module.Log("=================== Start ===================");
        _module.Log("Conditions and actions are numbered 1-5, top-to-bottom within the relevant color tables.");

        _module.Buttons[3].AddInteractionPunch();
        _module.AdvanceStage();
        yield return null;
    }

    public override TpAction NextTpAction() {
        return new TpAction(TpActionType.Start);
    }

}
