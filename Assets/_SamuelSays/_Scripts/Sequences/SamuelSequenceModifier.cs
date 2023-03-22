using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSequenceModifier {

    // ! Probably rewrite this stuff to work better.

    // Only need 3/4 symbols long.
    private readonly string[] MorseLetters = new string[] {
        "-...",
        "-.-.",
        "-..",
        "..-.",
        "--.",
        "....",
        ".---",
        "-.-",
        ".-..",
        "---",
        ".--.",
        "--.-",
        ".-.",
        "...",
        "..-",
        "...-",
        ".--",
        "-..-",
        "-.--",
        "--.."
    };

    private SamuelSaysModule _module;

    private bool _moduleWithRedInName;
    private bool _shoutsOrSendsPresent;
    private bool _redHasNotFailedToAppear = true;
    private bool _greenHasAppearedBefore = false;

    private int _litIndicatorCount;
    private int _unlitIndicatorCount;
    private int _batteryCount;
    private int _uniquePortTypeCount;
    private int _totalPorts;
    private int _moduleCount;

    private ColouredSymbol[] _displayedSequence;
    private Stack<ColouredSymbol[]> _modifiedSequences;
    private string _displayedSymbols;
    private List<ButtonColour> _displayedColours;
    private ColouredSymbol _removedSymbol;

    public SamuelSequenceModifier(SamuelSaysModule module) {
        _module = module;
        _modifiedSequences = new Stack<ColouredSymbol[]>();
        SetPermanents();
    }

    private void SetPermanents() {
        KMBombInfo bomb = _module.Bomb;
        List<string> moduleNames = bomb.GetModuleNames();

        _moduleWithRedInName = moduleNames.Any(modName => modName.ToLower().Contains("red"));
        _shoutsOrSendsPresent = moduleNames.Contains("Simon Shouts") || moduleNames.Contains("Simon Sends");

        _litIndicatorCount = bomb.GetOnIndicators().Count();
        _unlitIndicatorCount = bomb.GetOffIndicators().Count();
        _batteryCount = bomb.GetBatteryCount();
        _uniquePortTypeCount = bomb.CountUniquePorts();
        _totalPorts = bomb.GetPorts().Count();
        _moduleCount = bomb.GetModuleNames().Count();
    }

    public ColouredSymbol GetExpectedSubmission(ColouredSymbol[] displayedSequence) {
        _displayedSequence = displayedSequence;
        _displayedSymbols = new string(_displayedSequence.Select(symbol => symbol.Symbol).ToArray());
        _displayedColours = _displayedSequence.Select(symbol => symbol.Colour).ToList();
        _modifiedSequences.Clear();

        if (_redHasNotFailedToAppear && !_displayedSequence.Any(symbol => symbol.Colour == ButtonColour.Red)) {
            _redHasNotFailedToAppear = false;
        }

        foreach (ColouredSymbol symbol in displayedSequence) {
            ModifySequence(symbol.Colour);
        }

        return _modifiedSequences.Peek()[GetCorrectPosition()];

        throw new NotImplementedException();
    }

    private void ModifySequence(ButtonColour currentSymbolColour) {
        // Duplicate the modified sequence so far for further modification.
        _modifiedSequences.Push(_modifiedSequences.Peek());

        switch (currentSymbolColour) {
            case ButtonColour.Red: ModifyWithRed(); break;
            case ButtonColour.Yellow: ModifyWithYellow(); break;
            case ButtonColour.Green: ModifyWithGreen(); break;
            case ButtonColour.Blue: ModifyWithBlue(); break;
        }
    }

    private void ModifyWithRed() {
        if (_displayedSymbols == ".-.") {
            foreach (ColouredSymbol symbol in _modifiedSequences.Peek()) {
                symbol.ToggleSymbol();
            }
        }
        else if (_redHasNotFailedToAppear) {
            foreach (ColouredSymbol symbol in _modifiedSequences.Peek()) {
                symbol.Colour = ButtonColour.Red;
            }
        }
        else if (_displayedSequence.Where(symbol => symbol.Symbol == '-').Count() == _litIndicatorCount + _unlitIndicatorCount) {
            // Shift right by lit indicators - unlit indicators.
            int sequenceLength = _modifiedSequences.Peek().Length;
            int rightShiftOffset = _litIndicatorCount - _unlitIndicatorCount;
            ColouredSymbol[] unshiftedList = _modifiedSequences.Peek().ToArray();
            var shiftedList = new ColouredSymbol[unshiftedList.Length];

            for (int i = 0; i < sequenceLength; i++) {
                shiftedList[(i + rightShiftOffset) % sequenceLength] = unshiftedList[i];
            }

            _modifiedSequences.Push(shiftedList);
        }
        else if (_moduleWithRedInName) {
            // Swap reds with blues, and greens with yellows.
            foreach (ColouredSymbol symbol in _modifiedSequences.Peek()) {
                if (symbol.Colour == ButtonColour.Red) {
                    symbol.Colour = ButtonColour.Blue;
                }
                else if (symbol.Colour == ButtonColour.Yellow) {
                    symbol.Colour = ButtonColour.Green;
                }
                else if (symbol.Colour == ButtonColour.Green) {
                    symbol.Colour = ButtonColour.Yellow;
                }
                else if (symbol.Colour == ButtonColour.Blue) {
                    symbol.Colour = ButtonColour.Red;
                }
            }
        }
        else {
            ColouredSymbol[] currentList = _modifiedSequences.Peek();
            currentList[0] = currentList[2].Copy();
            if (currentList.Length == 4) {
                currentList[1] = currentList[3].Copy();
            }
        }
    }

    private void ModifyWithYellow() {
        if (_displayedSymbols == "-.--") {

        }
        else if (_module.StageNumber == _batteryCount) {
            ColouredSymbol[] currentSequence = _modifiedSequences.Pop();
            int length = _displayedSymbols.Length;
            int n = length - (_batteryCount % length);

            if (_removedSymbol != null) {
                _removedSymbol = currentSequence[n - 1];
                _modifiedSequences.Push(currentSequence.Where((symb, index) => index != n - 1).ToArray());
            }
            else {
                List<ColouredSymbol> currentSequenceList = currentSequence.ToList();
                currentSequenceList.Insert(n - 1, _removedSymbol);
                _modifiedSequences.Push(currentSequenceList.ToArray());
            }
        }
        else if (_shoutsOrSendsPresent) {
            _modifiedSequences.Peek()[0].Colour = ButtonColour.Yellow;
            _modifiedSequences.Peek()[1].Colour = ButtonColour.Yellow;
        }
        else if (_displayedColours.Where(col => col == ButtonColour.Yellow).Count() > 1) {
            for (int i = 0; i < _modifiedSequences.Peek().Length; i++) {
                if (i != 2) {
                    _modifiedSequences.Peek()[i].Colour = ButtonColour.Blue;
                }
            }
        }
        else {
            
        }
    }

    private void ModifyWithGreen() {
        if (_displayedSymbols == "--.") {

        }
        else if (_displayedColours[1] == ButtonColour.Green) {

        }
        else if (!_greenHasAppearedBefore) {
            _greenHasAppearedBefore = true;
        }
        else if (_displayedSymbols.Where(character => character == '.').Count() == _uniquePortTypeCount) {

        }
        else {

        }
    }

    private void ModifyWithBlue() {
        if (_displayedSymbols == "-...") {

        }
        else if (_modifiedSequences.Any(seq => seq[2].Colour == ButtonColour.Blue)) {

        }
        else if (!MorseLetters.Contains(_displayedSymbols)) {

        }
        else if (AllColoursAppear()) {

        }
        else {

        }
    }

    private bool AllColoursAppear() {

        // Lazy way of checking all colours appears in the sequence. Works since sequence is always 3 or 4 long.
        if (_displayedColours.Count() == 4 && _displayedColours.Distinct().Count() == 4) {
            return true;
        }

        // Ik this is inefficient but it's never gonna happen more than four times at a time so whatever.
        if (_modifiedSequences.Any()) {
            ButtonColour[] modifiedSequenceColours = _modifiedSequences.Peek().Select(symbol => symbol.Colour).ToArray();
            if (modifiedSequenceColours.Count() == 4 && modifiedSequenceColours.Distinct().Count() == 4) {
                return true;
            }
        }

        return false;
    }

    private int GetCorrectPosition() {
        throw new NotImplementedException();
    }
}
