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

    public SamColour Colour { get;
        private set;
    }
    public bool Lit {
        set {
            _light.enabled = value;
        }
    }

    void Awake() {
        SetButtonColourFromName();
        _light.enabled = false;

        _buttonCover.GetComponent<KMSelectable>().OnInteract += delegate () {
            _isBeingHeld = true;
            StartCoroutine(PushAnimation());
            return false;
        };

        _buttonCover.GetComponent<KMSelectable>().OnInteractEnded += delegate () {
            _isBeingHeld = false;
            StartCoroutine(ReleaseAnimation());
        };
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

    private IEnumerator PushAnimation() {
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
            StartCoroutine(PushAnimation());
        }
    }

    private float ButtonYPosiionFromTime(float progressPercentage) {
        float buttonFreedom = ReleasedYValue - PushedYValue;

        return PushedYValue + buttonFreedom * Mathf.Pow(1 - progressPercentage, 3);
    }
}
