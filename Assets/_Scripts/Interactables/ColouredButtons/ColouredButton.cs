using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {

	[SerializeField] private ButtonColour _colour;

	[SerializeField] private MeshRenderer _buttonBacking;
	[SerializeField] private MeshRenderer _buttonCover;
	[SerializeField] private Light _light;
	[SerializeField] private Animator _animator;

	[SerializeField] private KMSelectable _selectable;
	[SerializeField] private ColouredButtonController _controller;

	public ButtonColour Colour { get { return _colour; } }

	void Awake() {
		SetColour();
		_selectable.OnInteract += delegate () { _controller.HandlePress(this); return false; };
	}

	private void SetColour() {
		int index = (int)_colour;
		// This is me trying to do this "nicely" with different alpha values.
		float redComponent = index < 2 ? 0.34f : 0;
		float greenComponent = (index == 1 || index == 2) ? 0.34f : 0;
		float blueComponent = index == 3 ? 0.34f : 0;
		Color[] LightColours = new Color[] {
			Color.red,
			Color.yellow,
			Color.green,
			Color.blue
		};

		_buttonBacking.material.color = new Color(redComponent, greenComponent, blueComponent);
		_buttonCover.material.color = new Color(redComponent, greenComponent, blueComponent, 0.75f);
		_light.color = LightColours[index];
	}

	public void PlayPressAnimation() {
		_animator.SetBool("IsPressed", true);
	}

	public void PlayReleaseAnimation() {
		_animator.SetBool("IsPressed", false);
	}
}
