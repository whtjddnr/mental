using System.Collections;
using System.Linq;
using UnityEngine;

public static class StateManager {
    public static void DecreaseCountAll() {
        foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    foreach(State state in ((Card)card).states.value.ToList()) {
                        state.DecreaseCount(1);
                    }
                }
            }
        }
        foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    foreach(State state in ((Card)card).states.value.ToList()) {
                        state.DecreaseCount(1);
                    }
                }
            }
        }
    }
    public static void Act(StateActTiming when) {
        foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
            foreach(var card in new ArrayList(arr)) {
                if(card != null) {
                    foreach(State state in ((Card)card).states.value) {
                        if(state.When == when) {
                            state.Act();
                        }
                    }
                }
            }
        }
        foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
            foreach(var card in new ArrayList(arr)) {
                if(card != null) {
                    foreach(State state in ((Card)card).states.value) {
                        if(state.When == when) {
                            state.Act();
                        }
                    }
                }
            }
        }
    }
}