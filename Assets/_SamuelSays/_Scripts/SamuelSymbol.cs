public class SamuelSymbol {

    public readonly char Symbol;
    public readonly ButtonColour Colour;

    public SamuelSymbol(char symbol, ButtonColour colour) {
        if (symbol != '.' && symbol != '-') {
            throw new System.ArgumentException("SamuelSymbol must be either a dot '.' or a dash '-'.");
        }

        Symbol = symbol;
        Colour = colour;
    }

}
