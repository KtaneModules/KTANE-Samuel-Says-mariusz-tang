using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredButton : MonoBehaviour {
    [SerializeField] private GameObject _buttonCover;
    [SerializeField] private GameObject _buttonBacking;
    [SerializeField] private Light _light;

    private const float ReleasedYValue = 0;
    private const float PushedYValue = -0.42f;
    private const float ButtonAnimationTime = 0.15f;

    private bool _isBeingHeld = false;
    private bool _isMoving = false;

    public SamColour Colour { get; private set; }

    void Awake() {
        SetButtonColourFromName();
        _light.enabled = false;
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

    public void PlayHoldAnimation() {
        _isBeingHeld = true;
        _light.enabled = true;
        StartCoroutine(HoldAnimation());
    }

    public void PlayReleaseAnimation() {
        _isBeingHeld = false;
        _light.enabled = false;
        StartCoroutine(ReleaseAnimation());
    }

    private IEnumerator HoldAnimation() {
        float elapsedTime = 0;
        float transitionProgress = 0;
        Vector3 oldPos = _buttonCover.transform.localPosition;

        if (_isMoving) {
            yield break;
        }
        _isMoving = true;

        yield return null;

        while (transitionProgress < 1) {
            elapsedTime += Time.deltaTime;
            transitionProgress = Mathf.Min(elapsedTime / ButtonAnimationTime, 1);
            _buttonCover.transform.localPosition = new Vector3(oldPos.x, ButtonYPosiionFromTime(transitionProgress), oldPos.z);
            yield return null;
        }

        _isMoving = false;
        if (!_isBeingHeld) {
            StartCoroutine(ReleaseAnimation());
        }
    }

    private IEnumerator ReleaseAnimation() {
        float elapsedTime = 0;
        float transitionProgress = 0;
        Vector3 oldPos = _buttonCover.transform.localPosition;

        if (_isMoving) {
            yield break;
        }
        _isMoving = true;

        yield return null;

        while (transitionProgress < 1) {
            elapsedTime += Time.deltaTime;
            transitionProgress = Mathf.Min(elapsedTime / ButtonAnimationTime, 1);
            _buttonCover.transform.localPosition = new Vector3(oldPos.x, ButtonYPosiionFromTime(1 - transitionProgress), oldPos.z);
            yield return null;
        }

        _isMoving = false;
        if (_isBeingHeld) {
            StartCoroutine(HoldAnimation());
        }
    }

    private float ButtonYPosiionFromTime(float progressPercentage) {
        float buttonFreedom = ReleasedYValue - PushedYValue;

        return PushedYValue + buttonFreedom * Mathf.Pow(1 - progressPercentage, 3);
    }
}
