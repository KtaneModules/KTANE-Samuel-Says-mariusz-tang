using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	[SerializeField] private SamuelSaysModule _module;
	[SerializeField] private ColouredButton[] _buttons;
	[SerializeField] private KMSelectable _submitButton;

	void Awake() {
		foreach (ColouredButton button in _buttons) {
			button.Selectable.OnInteract += delegate () { HandlePress(button); return false; };
			button.Selectable.OnInteractEnded += delegate () { HandleRelease(button); };
		}
	}

	public void HandlePress(ColouredButton button) {
		button.PlayPressAnimation();
	}

	public void HandleRelease(ColouredButton button) {
		button.PlayReleaseAnimation();
	}
}
