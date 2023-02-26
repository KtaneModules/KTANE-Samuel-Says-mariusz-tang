using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {
    public Event StartedButtonHold;
    public Event EndedButtonHold;

    [SerializeField] private GameObject _buttonCover;
    [SerializeField] private GameObject _buttonBacking;
    [SerializeField] private Light _light;

    private Animator _animator;

    public SamColour Colour { get; private set; }

    void Awake() {
        _animator = GetComponentInChildren<Animator>();
        _light.enabled = false;
        SetButtonColourFromName();

    }

    private void SetButtonColourFromName() {
        // Dimmed colours (RYGB).
        Color[] buttonColourList = new Color[] {
            new Color(88f / 255f, 0, 0, 0.75f),
            new Color(88f / 255f, 88f / 255f, 0, 0.75f),
            new Color(0, 88f / 255f, 0, 0.75f),
            new Color(0, 0, 88f / 255f, 0.75f)
        };
        Color[] lightColourList = new Color[] {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };
        int colourIndex = "ABCD".IndexOf(transform.name);
        Color buttonColour = buttonColourList[colourIndex];
        Color lightColour = lightColourList[colourIndex];

        _buttonCover.GetComponent<MeshRenderer>().material.color = buttonColour;
        _buttonBacking.GetComponent<MeshRenderer>().material.color = buttonColour;
        _light.color = lightColour;
        Colour = (SamColour)colourIndex;
    }

    public void PlayPressAnimation() {
        _animator.SetBool("IsPressed", true);
    }

    public void PlayReleaseAnimation() {
        _animator.SetBool("IsPressed", false);
    }

}
