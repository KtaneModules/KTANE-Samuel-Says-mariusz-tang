using System;

public class ColouredSymbol {

    private ButtonColour _colour;
    private char _symbol;

    public ButtonColour Colour { get { return _colour; } }
    public char Symbol { get { return _symbol; } }

    public ColouredSymbol(ButtonColour colour, char symbol) {
        if ((symbol != '.') && (symbol != '-')) {
            throw new ArgumentException("Symbol must be either a dit '.' or a dah '-'.");
        }

        _colour = colour;
        _symbol = symbol;
    }

    public void ToggleSymbol() {
        if (_symbol == '.') {
            _symbol = '-';
        }
        else {
            _symbol = '.';
        }
    }

    public void SetColour(ButtonColour colour) {
        _colour = colour;
    }
}