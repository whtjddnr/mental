using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBundle {
    public Card Card;
    public abstract void Init(Card card);
    public List<Active> Actives = new List<Active>();
    public List<Passive> Passives = new List<Passive>();
}