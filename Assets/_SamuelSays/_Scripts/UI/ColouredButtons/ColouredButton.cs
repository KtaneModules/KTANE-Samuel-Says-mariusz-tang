using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ColouredButton : MonoBehaviour {

	[SerializeField] private ButtonColour _colour;

    [SerializeField] private MeshRenderer _buttonBacking;
	[SerializeField] private MeshRenderer _buttonCover;
	[SerializeField] private Light _light;

    private Animator _animator;
    private KMSelectable _selectable;

	public ButtonColour Colour { get { return _colour; } }
    public KMSelectable Selectable { get { return _selectable; } }

	void Awake() {
        _animator = GetComponent<Animator>();
        _selectable = GetComponentInChildren<KMSelectable>();

        SetColour();
    }

    private void SetColour() {
        int index = (int)_colour;
        Color[] Colours = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };

        _buttonBacking.material.color = Colours[index] * new Color(88f / 255f, 88f / 255f, 88f / 255f);
        _buttonCover.material.color = Colours[index] * new Color(88f / 255f, 88f / 255f, 88f / 255f, 0.75f);
        _light.color = Colours[index];
    }

    public void PlayPressAnimation() {
		_animator.SetBool("IsPressed", true);
	}

	public void PlayReleaseAnimation() {
		_animator.SetBool("IsPressed", false);
	}

    // Animator event.
    public void SetLightState(int state) {
        _light.enabled = (state == 1);
    }
}
