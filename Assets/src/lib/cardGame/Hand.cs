
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Splines;

public class Hand {
    public List<Card> value; 
    public void Add(Card card) {
        value.Add(card);
    }
    public void Remove(Card card) {
        value.Remove(card);
    }
    public void Init() {
        this.value = new List<Card>();
    }
}