using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager {

    public enum GameState
    {
        START_UI,
        IN_GAME,
        GAME_OVER
    }

    public static int gameState = (int)GameState.START_UI;
}
