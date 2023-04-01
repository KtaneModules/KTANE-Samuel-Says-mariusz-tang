using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class FinalStage : State {

    private Dictionary<char, string> _letterToNato = new Dictionary<char, string> {
        {'A', "ALFA"},
        {'B', "BRAVO"},
        {'C', "CHARLIE"},
        {'D', "DELTA"},
        {'E', "ECHO"},
        {'F', "FOXTROT"},
        {'G', "GOLF"},
        {'H', "HOTEL"},
        {'I', "INDIA"},
        {'J', "JULIETT"},
        {'K', "KILO"},
        {'L', "LIMA"},
        {'M', "MIKE"},
        {'N', "NOVEMBER"},
        {'O', "OSCAR"},
        {'P', "PAPA"},
        {'Q', "QUEBEC"},
        {'R', "ROMEO"},
        {'S', "SIERRA"},
        {'T', "TANGO"},
        {'U', "UNIFORM"},
        {'V', "VICTOR"},
        {'W', "WHISKEY"},
        {'X', "XRAY"},
        {'Y', "YANKEE"},
        {'Z', "ZULU"}
    };
    private Dictionary<char, string> _letterToMorse = new Dictionary<char, string> {
        {'A', ".-"},
        {'B', "-..."},
        {'C', "-.-."},
        {'D', "-.."},
        {'E', "."},
        {'F', "..-."},
        {'G', "--."},
        {'H', "...."},
        {'I', ".."},
        {'J', ".---"},
        {'K', "-.-"},
        {'L', ".-.."},
        {'M', "--"},
        {'N', "-."},
        {'O', "---"},
        {'P', ".--."},
        {'Q', "--.-"},
        {'R', ".-."},
        {'S', "..."},
        {'T', "-"},
        {'U', "..-"},
        {'V', "...-"},
        {'W', ".--"},
        {'X', "-..-"},
        {'Y', "-.--"},
        {'Z', "--.."}
    };
    private Dictionary<string, char> _morseToLetter;

    private List<ColouredSymbol> _expectedSubmission = new List<ColouredSymbol>();
    private List<int> _letterEndings = new List<int>();

    public FinalStage(SamuelSaysModule module) : base(module) {
        // Inverse dictionary.
        _morseToLetter = _letterToMorse.ToDictionary((i) => i.Value, (i) => i.Key);
    }

    public override IEnumerator OnStateEnter() {
        GenerateExpectedSubmission();
        // ! Continue here.
        return base.OnStateEnter();
    }

    private void GenerateExpectedSubmission() {
        List<int> quirks = _module.QuirkAppearances;
        int quirksCount = quirks.Count();
        int quirksDistinct = quirks.Distinct().Count();
        string displayedSymbols = new string(_module.DisplayedSequence.Select(symbol => symbol.Symbol).ToArray());
        string submitWord = string.Empty;

        if (!_morseToLetter.ContainsKey(displayedSymbols)) {
            submitWord = "SAMUEL";
        }
        else if (quirksCount == 1) {
            displayedSymbols = displayedSymbols.Remove(0, 1);
        }
        else if (quirksCount == 2) {
            if (quirksDistinct == 2) {
                displayedSymbols = displayedSymbols.Substring(displayedSymbols.Length - 2) + displayedSymbols.Substring(0, displayedSymbols.Length - 2);
            }
            else {
                displayedSymbols = new string(displayedSymbols.Reverse().ToArray());
            }
        }
        else if (quirksCount == 3) {
            if (quirksDistinct == 3) {
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
            }
            else if (quirksDistinct == 2) {
                displayedSymbols.Remove(2, 1);
            }
            else {
                displayedSymbols.Remove(0, 1);
                displayedSymbols.Remove(0, 1);
            }
        }

        if (submitWord == string.Empty) {
            if (displayedSymbols == string.Empty) {
                submitWord = "MORSE";
            }
            else {
                submitWord = _letterToNato[_morseToLetter[displayedSymbols]];
            }
        }

        int counter = 0;
        foreach (char letter in submitWord) {
            foreach (char symbol in _letterToNato[letter]) {
                _expectedSubmission.Add(new ColouredSymbol(_module.SubmittedColours[counter % 4], symbol));
                counter++;
            }
            _letterEndings.Add(counter);
        }
    }
}
