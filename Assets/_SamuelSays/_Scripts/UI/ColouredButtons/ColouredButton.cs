using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ColouredButton : MonoBehaviour {

    [SerializeField] private MeshRenderer _buttonBacking;
    [SerializeField] private MeshRenderer _buttonCover;
    [SerializeField] private Light _light;

    private Animator _animator;
    private KMSelectable _selectable;

    private ButtonColour _colour;

    public ButtonColour Colour { get { return _colour; } }
    public KMSelectable Selectable { get { return _selectable; } }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _selectable = GetComponentInChildren<KMSelectable>();
    }

    public void SetColour(ButtonColour colour) {
        Color[] Colours = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };

        _colour = colour;
        int index = (int)_colour;

        _buttonBacking.material.color = Colours[index] * 88f / 255f;
        _buttonCover.material.color = Colours[index] * 88f / 255f;
        _light.color = Colours[index];
    }

    public void SetVirusColourActive() {
        _buttonBacking.material.color = Color.magenta * 88f / 255f;
        _buttonCover.material.color = Color.magenta * 88f / 255f;
        _light.color = Color.magenta;
    }

    public void SetVirusColourInactive() {
        SetColour(Colour);
    }

    public void PlayPressAnimation() {
        _animator.SetBool("IsPressed", true);
    }

    public void PlayReleaseAnimation() {
        _animator.SetBool("IsPressed", false);
    }

    // Animator event.
    public void SetLightState(int state) {
        _light.enabled = state == 1;
    }

    public void AddInteractionPunch(float intensityModifier = 1.0f) {
        _selectable.AddInteractionPunch(intensityModifier);
    }
}
