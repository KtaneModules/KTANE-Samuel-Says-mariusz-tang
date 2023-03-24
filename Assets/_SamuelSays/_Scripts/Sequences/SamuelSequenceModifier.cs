using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSequenceModifier {

    // Only need 3/4 symbols long.
    private readonly Dictionary<int, string> MorseLetters = new Dictionary<int, string>() {
        {1, "-..."},
        {2, "-.-."},
        {3, "-.."},
        {5, "..-."},
        {6, "--."},
        {7, "...."},
        {9, ".---"},
        {10, "-.-"},
        {11, ".-.."},
        {14, "---"},
        {15, ".--."},
        {16, "--.-"},
        {17, ".-."},
        {18, "..."},
        {19, "..-"},
        {21, "...-"},
        {22, ".--"},
        {23, "-..-"},
        {24, "-.--"},
        {25, "--.."}
    };

    private SamuelSaysModule _module;

    // Permament values.
    private bool _moduleWithRedInName;
    private bool _shoutsOrSendsPresent;
    private int _batteryCount;
    private int _totalPorts;
    private int _uniquePortTypeCount;
    private int _litIndicatorCount;
    private int _unlitIndicatorCount;
    private int _serialNumberDigitSum;
    private int _moduleCount;

    // Variable values.
    private bool _blueHasBeenInPositionThreeThisStage = false;
    private bool _greenHasAppearedBefore = false;
    private bool _moreThanOneYellowInCurrentDisplay = false;
    private bool _redHasFailedToAppearInDisplay = false;

    private string _displayedSymbols;
    private List<ButtonColour> _displayedColours = new List<ButtonColour>();

    private string _modifiedSymbols;
    private List<ButtonColour> _modifiedColours = new List<ButtonColour>();

    private ColouredSymbol _removedSymbol;

    public SamuelSequenceModifier(SamuelSaysModule module) {
        _module = module;
        SetPermanentValues();
    }

    private void SetPermanentValues() {
        KMBombInfo bomb = _module.Bomb;
        List<string> moduleNames = bomb.GetModuleNames();

        _moduleWithRedInName = moduleNames.Any(modName => modName.ToLower().Contains("red"));
        _shoutsOrSendsPresent = moduleNames.Contains("Simon Shouts") || moduleNames.Contains("Simon Sends");
        _batteryCount = bomb.GetBatteryCount();
        _totalPorts = bomb.GetPorts().Count();
        _uniquePortTypeCount = bomb.CountUniquePorts();
        _litIndicatorCount = bomb.GetOnIndicators().Count();
        _unlitIndicatorCount = bomb.GetOffIndicators().Count();
        _serialNumberDigitSum = bomb.GetSerialNumberNumbers().Sum();
        _moduleCount = bomb.GetModuleNames().Count();
    }

    public ColouredSymbol GetExpectedSubmission(ColouredSymbol[] displayedSequence) {
        DeconstructDisplayedSequence(displayedSequence);
        _modifiedSymbols = _displayedSymbols;
        _modifiedColours = _displayedColours.ToList();

        foreach (ButtonColour colour in _displayedColours) {
            ModifySequence(colour);
        }

        ColouredSymbol[] modifiedSequence = ConstructModifiedSequence(_modifiedSymbols, _modifiedColours);

        return modifiedSequence[GetCorrectPosition()];
    }

    private void DeconstructDisplayedSequence(ColouredSymbol[] sequence) {
        _displayedSymbols = string.Empty;
        _displayedColours.Clear();

        foreach (ColouredSymbol symbol in sequence) {
            _displayedSymbols += symbol.Symbol;
            _displayedColours.Add(symbol.Colour);
        }

        if (!_displayedColours.Contains(ButtonColour.Red)) {
            _redHasFailedToAppearInDisplay = true;
        }

        _moreThanOneYellowInCurrentDisplay = _displayedColours.Count(colour => colour == ButtonColour.Yellow) >= 2;
        _blueHasBeenInPositionThreeThisStage = false;

    }

    private ColouredSymbol[] ConstructModifiedSequence(string symbols, List<ButtonColour> colours) {
        if (symbols.Length != colours.Count()) {
            throw new RankException("Number of symbols and number of colours must match to construct new sequence.");
        }

        ColouredSymbol[] newSequence = new ColouredSymbol[symbols.Length];

        for (int i = 0; i < symbols.Length; i++) {
            newSequence[i] = new ColouredSymbol(colours[i], symbols[i]);
        }

        return newSequence;
    }

    private void ModifySequence(ButtonColour currentSymbolColour) {
        if (!_blueHasBeenInPositionThreeThisStage) {
            if (_modifiedColours.Count() >= 3 && _modifiedColours[2] == ButtonColour.Blue) {
                _blueHasBeenInPositionThreeThisStage = true;
            }
        }

        switch (currentSymbolColour) {
            case ButtonColour.Red: ModifyWithRed(); break;
            case ButtonColour.Yellow: ModifyWithYellow(); break;
            case ButtonColour.Green: ModifyWithGreen(); break;
            case ButtonColour.Blue: ModifyWithBlue(); break;
        }
    }

    private void ModifyWithRed() {
        if (_displayedSymbols == ".-.") {
            // Swap dots and dashes.
            string newSymbols = string.Empty;

            for (int i = 0; i < _modifiedSymbols.Length; i++) {
                if (_modifiedSymbols[i] == '.') {
                    newSymbols += "-";
                }
                else {
                    newSymbols += ".";
                }
            }
            _modifiedSymbols = newSymbols;
        }
        else if (!_redHasFailedToAppearInDisplay) {
            // Make all symbols red.
            int currentLength = _modifiedColours.Count();

            _modifiedColours.Clear();
            for (int i = 0; i < currentLength; i++) {
                _modifiedColours.Add(ButtonColour.Red);
            }
        }
        else if (_modifiedSymbols.Count(symbol => symbol == '-') == _litIndicatorCount + _unlitIndicatorCount) {
            // Shift right by (lit indicators - unlit indicators).
            int rightShiftCount = _litIndicatorCount - _unlitIndicatorCount;
            int length = _modifiedSymbols.Length;
            string newSymbols = string.Empty;
            var newColours = new List<ButtonColour>();

            for (int start = length - (rightShiftCount % length), offset = 0; offset < length; offset++) {
                newSymbols += _modifiedSymbols[(start + offset) % length];
                newColours.Add(_modifiedColours[(start + offset) % length]);
            }
            _modifiedSymbols = newSymbols;
            _modifiedColours = newColours;
        }
        else if (_moduleWithRedInName) {
            // Swap red with blue, and green with yellow.
            var newColours = new List<ButtonColour>();
            foreach (ButtonColour colour in _modifiedColours) {
                switch (colour) {
                    case ButtonColour.Red: newColours.Add(ButtonColour.Blue); break;
                    case ButtonColour.Yellow: newColours.Add(ButtonColour.Green); break;
                    case ButtonColour.Green: newColours.Add(ButtonColour.Yellow); break;
                    case ButtonColour.Blue: newColours.Add(ButtonColour.Red); break;
                }
            }
            _modifiedColours = newColours;
        }
        else {
            // Replace position 1 with position 3.
            _modifiedSymbols.Insert(startIndex: 1, value: _modifiedSymbols[2].ToString());
            _modifiedSymbols.Remove(startIndex: 0, count: 1);
            _modifiedColours.Insert(index: 1, item: _modifiedColours[2]);
            _modifiedColours.RemoveAt(0);

            if (_modifiedSymbols.Length == 4) {
                // Replace position 2 with position 4.
                _modifiedSymbols.Insert(startIndex: 2, value: _modifiedSymbols[3].ToString());
                _modifiedSymbols.Remove(startIndex: 1, count: 1);
                _modifiedColours.Insert(index: 2, item: _modifiedColours[3]);
                _modifiedColours.RemoveAt(1);
            }
        }
    }

    private void ModifyWithYellow() {
        if (_displayedSymbols == "-.--") {
            _modifiedSymbols.Reverse();
            _modifiedColours.Reverse();
        }
        else if (_module.StageNumber == _batteryCount) {
            if (_modifiedSymbols.Length == 4) {
                int n = 4 - (_batteryCount % 4);
                _modifiedColours.RemoveAt(n - 1);
                _modifiedSymbols.Remove(startIndex: n - 1, count: 1);
            }
            else {
                int m = _serialNumberDigitSum % 10 % 3;
                _modifiedColours.Insert(m - 1, ButtonColour.Yellow);
                _modifiedSymbols.Insert(m - 1, "-");
            }
        }
        else if (_shoutsOrSendsPresent) {
            _modifiedColours[0] = ButtonColour.Yellow;
            _modifiedColours[1] = ButtonColour.Yellow;
        }
        else if (_moreThanOneYellowInCurrentDisplay) {
            // Change all except position 3 to blue.
            for (int i = 0; i < _modifiedColours.Count(); i++) {
                if (i != 2) {
                    _modifiedColours[i] = ButtonColour.Blue;
                }
            }
        }
        else {
            // Shift colours right twice.
            int rightShiftCount = 2;
            int length = _modifiedColours.Count();
            var newColours = new List<ButtonColour>();

            for (int start = length - (rightShiftCount % length), offset = 0; offset < length; offset++) {
                newColours.Add(_modifiedColours[(start + offset) % length]);
            }
            _modifiedColours = newColours;
        }
    }

    private void ModifyWithGreen() {
        if (_displayedSymbols == "--.") {
            // Make dashes green THEN change dots to dashes.
            for (int i = 0; i < _modifiedSymbols.Length; i++) {
                if (_modifiedSymbols[i] == '-') {
                    _modifiedColours[i] = ButtonColour.Green;
                }
                else {
                    _modifiedSymbols.Remove(startIndex: i, count: 0);
                    _modifiedSymbols.Insert(startIndex: i, value: "-");
                }
            }
        }
        else if (_displayedColours[1] == ButtonColour.Green) {
            _modifiedColours.Clear();
            _modifiedColours.Add(ButtonColour.Red);
            _modifiedColours.Add(ButtonColour.Blue);
            _modifiedColours.Add(ButtonColour.Yellow);
            if (_modifiedSymbols.Length == 4) {
                _modifiedColours.Add(ButtonColour.Green);
            }
        }
        else if (!_greenHasAppearedBefore) {
            _greenHasAppearedBefore = true;
            for (int i = 0; i < _modifiedColours.Count(); i++) {
                if (i == 0) {
                    _modifiedColours[i] = ButtonColour.Green;
                }
                else {
                    _modifiedColours[i] = ButtonColour.Red;
                }
            }
        }
        else if (_modifiedSymbols.Count(symbol => symbol == '.') == _uniquePortTypeCount) {
            // Move all dots to the front.
            _modifiedSymbols = new string('.', _uniquePortTypeCount);
            while (_modifiedSymbols.Length < _modifiedColours.Count()) {
                _modifiedSymbols += "-";
            }
        }
        else {
            // Remove position 2 and add a green dot to the end.
            _modifiedSymbols.Remove(startIndex: 1, count: 1);
            _modifiedSymbols += ".";
            _modifiedColours.RemoveAt(1);
            _modifiedColours.Add(ButtonColour.Green);
        }
    }

    private void ModifyWithBlue() {
        if (_displayedSymbols == "-...") {
            _modifiedSymbols.Reverse();
        }
        else if (_blueHasBeenInPositionThreeThisStage) {
            // Shift right once.
            int rightShiftCount = 1;
            int length = _modifiedSymbols.Length;
            string newSymbols = string.Empty;
            var newColours = new List<ButtonColour>();

            for (int start = length - (rightShiftCount % length), offset = 0; offset < length; offset++) {
                newSymbols += _modifiedSymbols[(start + offset) % length];
                newColours.Add(_modifiedColours[(start + offset) % length]);
            }
            _modifiedSymbols = newSymbols;
            _modifiedColours = newColours;
        }
        else if (!MorseLetters.ContainsValue(_displayedSymbols)) {
            // Find the first letter, starting from sum of serial digits with a morse representation of
            // the same length as that of the sequence.
            int tryNumber = _serialNumberDigitSum % 26;
            while (!MorseLetters.ContainsKey(tryNumber) || MorseLetters[tryNumber].Length != _modifiedSymbols.Length) {
                tryNumber++;
                tryNumber %= 26;
            }
            _modifiedSymbols = MorseLetters[tryNumber];
        }
        else if (!AllColoursAppear()) {
            // Set positions 1 and 2 to blue, and the rest to red.
            for (int i = 0; i < _modifiedColours.Count(); i++) {
                if (i < 2) {
                    _modifiedColours[i] = ButtonColour.Blue;
                }
                else {
                    _modifiedColours[i] = ButtonColour.Red;
                }
            }
        }
    }

    private bool AllColoursAppear() {
        return _displayedColours.Concat(_modifiedColours).Distinct().Count() == 4;
    }

    private int GetCorrectPosition() {
        int quantityToUse;

        switch (_module.StageNumber) {
            case 1: quantityToUse = _batteryCount; break;
            case 2: quantityToUse = _totalPorts; break;
            case 3: quantityToUse = _litIndicatorCount + _unlitIndicatorCount; break;
            case 4: quantityToUse = _moduleCount; break;
            default: throw new ArgumentOutOfRangeException("WTF STAGE NUMBER ARE WE ON :(");
        }

        return quantityToUse % _modifiedSymbols.Length;
    }
}
