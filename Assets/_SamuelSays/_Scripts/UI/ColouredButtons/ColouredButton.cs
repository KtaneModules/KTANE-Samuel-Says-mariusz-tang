using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ColouredButton : MonoBehaviour {

    [SerializeField] private MeshRenderer _buttonBacking;
    [SerializeField] private MeshRenderer _buttonCover;
    [SerializeField] private Light _light;
    [SerializeField] private AudioSource _beep;

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

    public void PlayPressAnimation(bool playSound = true) {
        _animator.SetBool("IsPressed", true);
        if (playSound) {
            _beep.Play();
        }
    }

    public void PlayReleaseAnimation() {
        _animator.SetBool("IsPressed", false);
        _beep.Stop();
    }

    public void ToggleMute() {
        // Toggle between 0 and 0.25.
        _beep.volume = 0.25f - _beep.volume;
    }

    public void SetMute(bool isEnabled) {
        _beep.volume = isEnabled ? 0 : 0.25f;
    }

    // Animator event.
    public void SetLightState(int state) {
        _light.enabled = state == 1;
    }

    public void AddInteractionPunch(float intensityModifier = 1.0f) {
        _selectable.AddInteractionPunch(intensityModifier);
    }
}
