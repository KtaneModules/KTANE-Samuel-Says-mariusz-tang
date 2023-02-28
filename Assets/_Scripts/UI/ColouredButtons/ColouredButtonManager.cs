using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButtonManager : MonoBehaviour {

	[SerializeField] private SamuelSaysModule _module;
	[SerializeField] private ColouredButton[] _buttons;

	public void HandlePress(ColouredButton button) {
		button.PlayPressAnimation();
	}

	public void HandleRelease(ColouredButton button) {
		button.PlayReleaseAnimation();
	}
}
