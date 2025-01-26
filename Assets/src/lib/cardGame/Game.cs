using UnityEngine;

public class Game {
    public Field field;
    public Opponent opponent;
    public Deck playerDeck = new Deck();
    public Deck opponentDeck = new Deck();
    public Hand playerHand = new Hand();
    public Hand opponentHand = new Hand();
    public int playerBehaviourPoint = 0;
    public int opponentBehaviourPoint = 0;
    public int CountOfturn = 0;
    public Game(Field field, Opponent opponent) {
        this.field = field;
        this.opponent = opponent;        
    }
    public void Init() {
        field.Init();
        playerHand.Init();
        opponentHand.Init();

        foreach(Card card in this.opponent.deck.value) {
            this.opponentDeck.Add(card);
        }
        foreach(Card card in Player.deck.value) {
            this.playerDeck.Add(card);
        }
    }
}