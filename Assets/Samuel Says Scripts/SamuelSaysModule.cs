using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;

public class SamuelSaysModule : MonoBehaviour {

	public KMBombInfo Bomb;
	public KMAudio Audio;
	public KMBombModule Module;
	public ColouredButton[] Buttons;
	public MainScreen Screen;

	private static int _moduleIdCounter = 1;
	private int _moduleId;
	private bool _moduleSolved = false;

	// Use this for initialization
	void Start () {
		foreach (ColouredButton button in Buttons) {
			button.GetComponentInChildren<KMSelectable>().OnInteract += delegate () { button.Lit = true; Screen.ShowColour(button.Colour); return false; };
			button.GetComponentInChildren<KMSelectable>().OnInteractEnded += delegate () { button.Lit = false; Screen.Disable(); };
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
