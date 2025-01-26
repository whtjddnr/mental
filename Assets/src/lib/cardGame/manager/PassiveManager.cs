public static class PassiveManager {
    public static void Act(PassiveActTiming when) {
        foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    foreach(Passive state in ((Card)card).skillBundle.Passives) {
                        if(state.When == when) {
                            state.Act();
                        }
                    }
                }
            }
        }
        foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    foreach(Passive state in ((Card)card).skillBundle.Passives) {
                        if(state.When == when) {
                            state.Act();
                        }
                    }
                }
            }
        }
    }
}