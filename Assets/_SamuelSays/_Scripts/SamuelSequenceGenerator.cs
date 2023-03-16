using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SamuelSequenceGenerator {

    public SamuelSequenceGenerator() { }

    public ColouredSymbol[] GenerateRandomSequence(int sequenceLength) {
        var sequence = new ColouredSymbol[sequenceLength];

        for (int i = 0; i < sequenceLength; i++) {
            var colour = (ButtonColour)Rnd.Range(0, 4);
            char symbol = ".-"[Rnd.Range(0, 2)];

            sequence[i] = new ColouredSymbol(colour, symbol);
        }

        return sequence;
    }

}
