using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {

	[SerializeField] private Light _light;

	public bool Lit { set { _light.enabled = value; } }

	void Awake() {
		_light.enabled = false;
    }
}
