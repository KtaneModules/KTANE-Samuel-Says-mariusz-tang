using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

    [SerializeField] private Material _displayMaterial;
    [SerializeField] private AudioSource _beep;

    private const float COLOUR_BRIGHTNESS = 0.9f;
    private const float MORSE_TIME_UNIT = 0.2f;

    private Color[] _colourList = new Color[] {
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };

    private MeshRenderer _colourDisplay;
    private Coroutine _displaySequence;

    void Awake() {
        _colourDisplay = GetComponent<MeshRenderer>();
        _colourDisplay.enabled = false;
    }

    public void SetMute(bool enableMute) {
        _beep.volume = 1;
        
        if (enableMute) {
            _beep.volume = 0;
        }
    }

    private void DisplayColour(ButtonColour colour) {
        _displayMaterial.color = _colourList[(int)colour] * COLOUR_BRIGHTNESS;
        _colourDisplay.enabled = true;
        _beep.Play();
    }

    private void StopDisplayingColour() {
        _colourDisplay.enabled = false;
        _beep.Stop();
    }

    public void PlaySequence(List<ColouredSymbol[]> sequences) {
        if (_displaySequence != null) {
            StopCoroutine(_displaySequence);
        }
        _displaySequence = StartCoroutine(DisplaySequence(sequences));
    }

    public void StopSequence() {
        if (_displaySequence != null) {
            StopCoroutine(_displaySequence);
        }
        StopDisplayingColour();
    }

    private IEnumerator DisplaySequence(List<ColouredSymbol[]> sequences) {
        ColouredSymbol[] currentSequence = sequences[0];
        float elapsedTime;
        float waitTime;

        foreach (ColouredSymbol symbol in currentSequence) {
            int flashLength = (symbol.Symbol == '-') ? 3 : 1;
            DisplayColour(symbol.Colour);

            // Wait for waitTime seconds.
            waitTime = MORSE_TIME_UNIT * flashLength;
            for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
                yield return null;
            }

            StopDisplayingColour();

            waitTime = MORSE_TIME_UNIT;
            for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
                yield return null;
            }
        }

        waitTime = 2 * MORSE_TIME_UNIT;
        for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
            yield return null;
        }

        yield return null;
        sequences.Add(sequences[0]);
        sequences.RemoveAt(0);
        _displaySequence = StartCoroutine(DisplaySequence(sequences));
    }
}
