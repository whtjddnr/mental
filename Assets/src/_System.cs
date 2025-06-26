using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _System : MonoBehaviour
{
    public PlayMode PlayMode = PlayMode.CardGame;
    private void CardGameMode() {
        Opponent opponent = new Opponent();
        int[] fieldConstruction = {5};
        string[] deckRecipe = new string[] {
            "fish",
            "fish",
            "fish",
            "shark",
        };

        var card = new Card(Target.player, 100, Util.getJson<CardType>($"cardData/fish"));
        Player.deck.Add(card);

        CardGameEngine.Init(new Game(new DefaultField(fieldConstruction, fieldConstruction, opponent), opponent));
        CardGameEngine.Start();

        var test = new Card(Target.opponent, 1000, Util.getJson<CardType>("cardData/test"));
        
        OpponentManager.SummonsManager.Summons(test, 2, 0, true);
        PlayerManager.SummonsManager.Summons(new Card(Target.player, 19, Util.getJson<CardType>("cardData/bird")), 0, 0, true);
    }
    private void ExplorationMode() {
        
    }
    void Start()
    {
        CardGameMode();
        // ExplorationMode();
    }

    
    void Update()
    {
        if(PlayMode == PlayMode.CardGame) {
        }
        CardGameEngine.Loop();
    }
}
