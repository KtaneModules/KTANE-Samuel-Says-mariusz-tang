using System;

public struct TpAction {

    public TpAction(TpActionType actionType, int position = -1, int time = -1) {
        if (actionType != TpActionType.Wait && actionType != TpActionType.Solved && actionType != TpActionType.Start && (position > 3 || position < 0)) {
            throw new ArgumentOutOfRangeException("Position must be between 0 and 3");
        }
        if (actionType == TpActionType.PressTimed && (time < 0 || time > 9)) {
            throw new ArgumentOutOfRangeException("Time must be between 0 and 9");
        }

        ActionType = actionType;
        Position = position;
        Time = time;
    }

    public TpActionType ActionType { get; private set; }
    public int Position { get; private set; }
    public int Time { get; private set; }
}

public enum TpActionType {
    PressLong,
    PressShort,
    PressTimed,
    Solved,
    Start,
    Wait,
}
