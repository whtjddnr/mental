using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Deck {
    public List<Card> value = new List<Card>();
    public void Add(Card card) {
        Card _card = new(card.target, card.uid, card.spec) {
            gameObject = card.gameObject
        };
        value.Add(card);
    }
    public void Remove(int index) {
        value.RemoveAt(index);
    }
    public void Shuffle() {
        Random rand = new Random();
        var shuffled = value.OrderBy(_ => rand.Next()).ToList();
        value = shuffled;
    }
}