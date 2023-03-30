using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour {

    private const float MorseTimeUnit = 0.2f;
    private const float ColourBrightness = 0.9f;

    private bool _skipPause = false;

    [SerializeField] private MeshRenderer _display;
    [SerializeField] private AudioSource _beep;

    private readonly Color[] _colourList = new Color[] {
        Color.red,
        Color.yellow,
        Color.green,
        Color.blue
    };
    private Coroutine _displaySequence;

    private void Awake() {
        _display.enabled = false;
    }

    public void ToggleMute() {
        // Toggle between 0 and 1.
        _beep.volume = 1 - Math.Abs(_beep.volume);
    }

    private void DisplayColour(ButtonColour colour) {
        _display.enabled = true;
        _display.material.color = _colourList[(int)colour] * ColourBrightness;
        _beep.Play();
    }

    private void StopDisplayingColour() {
        _display.enabled = false;
        _beep.Stop();
    }

    public void PlaySequence(ColouredSymbol[] sequence, bool skipPause = false) {
        PlaySequences(new List<ColouredSymbol[]>() { sequence }, skipPause);
    }

    public void PlaySequences(List<ColouredSymbol[]> sequences, bool skipPause = false) {
        if (_displaySequence != null) {
            StopCoroutine(_displaySequence);
        }
        _skipPause = skipPause;
        _displaySequence = StartCoroutine(DisplaySequences(sequences));
    }

    public void StopSequence() {
        if (_displaySequence != null) {
            StopCoroutine(_displaySequence);
        }
        StopDisplayingColour();
    }

    private IEnumerator DisplaySequences(List<ColouredSymbol[]> sequences) {
        ColouredSymbol[] currentSequence = sequences[0];
        float elapsedTime;
        float waitTime;

        foreach (ColouredSymbol symbol in currentSequence) {
            int flashLength = (symbol.Symbol == '-') ? 3 : 1;
            DisplayColour(symbol.Colour);

            // Wait for waitTime seconds.
            waitTime = MorseTimeUnit * flashLength;
            for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
                yield return null;
            }

            StopDisplayingColour();

            waitTime = MorseTimeUnit;
            for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
                yield return null;
            }
        }

        if (!_skipPause) {
            waitTime = 2 * MorseTimeUnit;
            for (elapsedTime = 0; elapsedTime < waitTime; elapsedTime += Time.deltaTime) {
                yield return null;
            }
        }

        yield return null;
        sequences.Add(sequences[0]);
        sequences.RemoveAt(0);
        _displaySequence = StartCoroutine(DisplaySequences(sequences));
    }
}
