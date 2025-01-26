using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _System : MonoBehaviour
{
    public Camera cam;
    void Awake(){
    	cam = Camera.main;
    }
    
    void Start()
    {
        Opponent opponent = new Opponent();
        int[] fieldConstruction = {5};
        string[] deckRecipe = new string[] {
            "fish",
            "fish",
            "fish",
            "shark",
        };
        for(int i = 0; i < deckRecipe.Length; i++) {
            var playerCard = new Card(Target.player, Player.deck.value.Count, Util.getJson<CardType>($"cardData/{deckRecipe[i]}"));
            var opponentCard = new Card(Target.opponent, opponent.deck.value.Count, Util.getJson<CardType>($"cardData/{deckRecipe[i]}"));
            Player.deck.Add(playerCard);
            opponent.deck.Add(opponentCard);
        }
        Player.deck.Shuffle();
        opponent.deck.Shuffle();

        CardGameEngine.Init(new Game(new DefaultField(fieldConstruction, fieldConstruction, opponent), opponent));
        CardGameEngine.Start();

        var card = new Card(Target.player, 100, Util.getJson<CardType>($"cardData/fish"));
        HandManager.Add(card);

        var test = new Card(Target.opponent, 1000, Util.getJson<CardType>("cardData/test"));
        var test2 = new Card(Target.opponent, 110, Util.getJson<CardType>("cardData/test"));
        
        OpponentManager.SummonsManager.Summons(test, 2, 0, true);
        OpponentManager.SummonsManager.Summons(test2, 0, 0, true);
        PlayerManager.SummonsManager.Summons(new Card(Target.player, 10, Util.getJson<CardType>("cardData/silentMask")), 0, 0, true);
        PlayerManager.SummonsManager.Summons(new Card(Target.player, 11, Util.getJson<CardType>("cardData/bird")), 1, 0, true);
        // CardGameEngine.SummonsManager.Summons(Target.player, new Card(Target.player, 12, Util.getJson<CardType>("cardData/shark")), 2, 0, true);
    }

    
    void Update()
    {
        CardGameEngine.Loop();
    }
}
