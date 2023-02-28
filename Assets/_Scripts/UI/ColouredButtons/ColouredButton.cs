using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ColouredButton : MonoBehaviour {

	[SerializeField] private ButtonColour _colour;
    [SerializeField] private ColouredButtonManager _controller;

    [SerializeField] private MeshRenderer _buttonBacking;
	[SerializeField] private MeshRenderer _buttonCover;
	[SerializeField] private Light _light;

    private Animator _animator;

	public ButtonColour Colour { get { return _colour; } }

	void Awake() {
        _animator = GetComponent<Animator>();

        SetColour();
        GetComponentInChildren<KMSelectable>().OnInteract += delegate () { _controller.HandlePress(this); return false; };
        GetComponentInChildren<KMSelectable>().OnInteractEnded += delegate () { _controller.HandleRelease(this); };
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
