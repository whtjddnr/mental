using DG.Tweening;
using UnityEngine;

public static class OpponentManager {
    public static class BehaviourPoint {
        public static void Init() {
            Reset(0);
        }
        public static void Reset(int num) {
            int temp = 0;
            temp += CardGameEngine.game.opponentBehaviourPoint;
            CardGameEngine.game.opponentBehaviourPoint = num;
        }
        public static void Remove(int num) {
            for(int i = 0; i < num; i++) { CardGameEngine.game.opponentBehaviourPoint -= 1; }
        }
    }
    public static class DrawManager {
        public static void Draw() {
            Deck targetrDeck = CardGameEngine.game.opponentDeck;
            Hand targetHand = CardGameEngine.game.opponentHand;
            if(targetrDeck.value.Count == 0) return;
            int index = targetrDeck.value.Count - 1;
            targetHand.Add(targetrDeck.value[index]);
            targetrDeck.Remove(index);
        }
    }
    public static class SummonsManager {
        public static void Summons(Card card, int row, int column, bool isSpecialSummons) {
            if(CardGameEngine.game.opponentBehaviourPoint == 0) return; 
            if(CardGameEngine.game.field.opponentFieldArrangement[column][row] == null) {
                card.Render(CardGameEngine.game.field.theme, new Vector3());
                CardGameEngine.game.field.Put(Target.opponent, card, row, column, false);
                card.gameObject.transform.DOScale(1, 0.2f);
                card.location = CardLocation.onField;
                if(!isSpecialSummons) CardGameEngine.game.opponentHand.Remove(card);
                OpponentManager.BehaviourPoint.Remove(1);
            }
        }
    }
    public static class Algorithm {
        public static void Summons() {
            bool Summoned = false;
            if(CardGameEngine.game.opponentHand.value.Count == 0) return;
            Card card = CardGameEngine.game.opponentHand.value[0];
            Debug.Log(CardGameEngine.game.opponentHand.value.Count);
            if(card.spec.sacrifice.Count == 0) {
                for(int n = 0; n < CardGameEngine.game.field.opponentFieldConstruction.Length; n++) {
                    for(int t = 0; t < CardGameEngine.game.field.opponentFieldConstruction[n]; t++) {
                        if(CardGameEngine.game.field.opponentFieldArrangement[n][t] == null) {
                            if(!Summoned) {
                                OpponentManager.SummonsManager.Summons(card, t, n, false);
                                Summoned = true;
                            }
                        }
                    }
                }
            }
        }
    }
}