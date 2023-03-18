using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSequenceModifier {

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

    private int _indicatorCount;
    private int _batteryCount;
    private int _uniquePortTypeCount;
    private int _totalPorts;
    private int _moduleCount;

    private ColouredSymbol[] _displayedSequence;
    private Stack<ColouredSymbol[]> _modifiedSequences;
    private string _displayedSymbols;
    private List<ButtonColour> _displayedColours;

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

        _indicatorCount = bomb.GetIndicators().Count();
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

        }
        else if (_redHasNotFailedToAppear) {

        }
        else if (_displayedSequence.Where(symbol => symbol.Symbol == '-').Count() == _indicatorCount) {

        }
        else if (_moduleWithRedInName) {

        }
        else {

        }
    }

    private void ModifyWithYellow() {
        if (_displayedSymbols == "-.--") {

        }
        else if (_module.StageNumber == _batteryCount) {

        }
        else if (_shoutsOrSendsPresent) {

        }
        else if (_displayedColours.Where(col => col == ButtonColour.Yellow).Count() > 1) {

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
