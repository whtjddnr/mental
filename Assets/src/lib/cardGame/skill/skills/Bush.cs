using System;
using System.Collections.Generic;
using UnityEngine;
public class Bush: SkillBundle {
    public Bush(Card ownerCard) : base(ownerCard) {}
    private class Passive1 : Passive
    {
        public Passive1(Card card) : base(card) {}

        public override int SkillNumber => 0;
        public override PassiveActTiming When => PassiveActTiming.startbattle;
        public override void Act()
        {
            if(this.Card.gameObject.GetComponent<CardComponent>().column == 0) {
                int row = this.Card.gameObject.GetComponent<CardComponent>().row;
                if(CardGameEngine.game.field.playerFieldArrangement.Length == 1) {
                    if(CardGameEngine.game.field.playerFieldArrangement[1][row] != null) {
                        ((Card)CardGameEngine.game.field.playerFieldArrangement[1][row]).AddDefence(5);
                    }
                }
            }
        }

        public override void SetTarget(Card target) {}
    }
    public override List<Active> Actives => new List<Active>() {};
    public override List<Passive> Passives => new List<Passive>() {new Passive1(Card)};

}