using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {
	private readonly Color _red = new Color(88f / 255f, 0, 0, 0.75f);
	private readonly Color _yellow = new Color(88f / 255f, 88f / 255f, 0, 0.75f);
	private readonly Color _green = new Color(0, 88f / 255f, 0, 0.75f);
	private readonly Color _blue = new Color(0, 0, 88f / 255f, 0.75f);

	[SerializeField] private MeshRenderer _buttonCover;
	[SerializeField] private MeshRenderer _backing;
	[SerializeField] private Light _light;

	private SamColour _colour;

	public SamColour Colour { get { return _colour; } }
	public bool Lit { set { _light.enabled = value; } }

	void Awake() {
		_light.enabled = false;
		_buttonCover.transform.localPosition = new Vector3();

		if (transform.name == "A") {
			_buttonCover.material.color = _red;
			_backing.material.color = _red;
			_light.color = Color.red;
			_colour = SamColour.Red;
        } else if (transform.name == "B") {
			_buttonCover.material.color = _yellow;
			_backing.material.color = _yellow;
			_light.color = Color.yellow;
			_colour = SamColour.Yellow;
		} else if (transform.name == "C") {
			_buttonCover.material.color = _green;
			_backing.material.color = _green;
			_light.color = Color.green;
			_colour = SamColour.Green;
		} else if (transform.name == "D") {
			_buttonCover.material.color = _blue;
			_backing.material.color = _blue;
			_light.color = Color.blue;
			_colour = SamColour.Blue;
		}
	}
}
