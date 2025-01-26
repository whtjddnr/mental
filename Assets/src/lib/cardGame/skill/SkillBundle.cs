using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBundle {
    public SkillBundle(Card ownerCard) {
        this.Card = ownerCard;
        foreach(Active active in Actives) {
            active.Card = ownerCard;
        }
        foreach(Passive passive in Passives) {
            passive.Card = ownerCard;
        }
    }
    public Card Card;
    public abstract List<Active> Actives { get; }
    public abstract List<Passive> Passives { get; }
}