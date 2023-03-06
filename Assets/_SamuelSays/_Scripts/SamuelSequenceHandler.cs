using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using System.Linq;

public class SamuelSequenceHandler {

    private const int SEQUENCE_LENGTH = 4;
    private SamuelSaysModule _module;

    public SamuelSequenceHandler(SamuelSaysModule module) {
        _module = module;
    }

    public ColouredSymbol[] GenerateRandomSequence() {
        var sequence = new ColouredSymbol[SEQUENCE_LENGTH];

        for (int i = 0; i < SEQUENCE_LENGTH; i++) {
            var colour = (ButtonColour)Rnd.Range(0, 4);
            char symbol = ".-"[Rnd.Range(0, 2)];

            sequence[i] = new ColouredSymbol(colour, symbol);
        }

        return sequence;
    }

}