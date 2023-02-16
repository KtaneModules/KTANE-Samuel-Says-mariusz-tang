using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuelSaysModule : MonoBehaviour {

	public ColouredButton[] Buttons;

	// Use this for initialization
	void Start () {
		foreach (ColouredButton button in Buttons) {
			button.GetComponent<KMSelectable>().OnInteract += delegate () { button.Lit = true; return false; };
			button.GetComponent<KMSelectable>().OnInteractEnded += delegate () { button.Lit = false; };
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
