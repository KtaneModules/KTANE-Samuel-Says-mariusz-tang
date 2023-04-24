using System;

public struct TpAction {

    public TpAction(TpActionType actionType, int position = -1) {
        if (actionType != TpActionType.Wait && actionType != TpActionType.Solved && actionType != TpActionType.Start && (position > 3 || position < 0)) {
            throw new ArgumentOutOfRangeException("Position must be between 0 and 3");
        }

        ActionType = actionType;
        Position = position;
    }

    public TpActionType ActionType { get; private set; }
    public int Position { get; private set; }
}

public enum TpActionType {
    PressLong,
    PressShort,
    PressStuck,
    Solved,
    Start,
    Wait,
}
