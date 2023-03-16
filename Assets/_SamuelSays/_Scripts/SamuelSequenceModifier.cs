using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSequenceModifier {

    private SamuelSaysModule _module;

    private bool _moduleWithRedInName;
    private bool _shoutsOrSendsPresent;
    private bool _greenHasAppearedBefore = false;

    private int _indicatorCount;
    private int _batteryCount;
    private int _uniquePortTypeCount;
    private int _totalPorts;
    private int _moduleCount;

    private ColouredSymbol[] _displayedSequence;
    private Stack<ColouredSymbol[]> _modifiedSequences;

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
        _modifiedSequences.Clear();

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

    }

    private void ModifyWithYellow() {
        throw new NotImplementedException();
    }

    private void ModifyWithGreen() {
        throw new NotImplementedException();
    }

    private void ModifyWithBlue() {
        throw new NotImplementedException();
    }

    private int GetCorrectPosition() {
        throw new NotImplementedException();
    }
}
