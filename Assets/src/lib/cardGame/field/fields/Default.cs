class DefaultField : Field
{
    public DefaultField(int[] playerFieldConstruction, int[] opponentFieldConstruction, Opponent opponent) : base(playerFieldConstruction, opponentFieldConstruction, opponent)
    {
    }

    public override void Effect()
    {
        
    }

    public override void TurnStart()
    {
        foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    ((Card)card).AddAdaptation(1);
                }
            }
        }
        foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
            foreach(var card in arr) {
                if(card != null) {
                    ((Card)card).AddAdaptation(1);
                }
            }
        }
    }
}