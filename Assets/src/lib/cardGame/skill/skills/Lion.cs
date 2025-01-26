using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class Lion: SkillBundle {
    public Lion(Card ownerCard) : base(ownerCard) {}
    private class Skill0 : Active {
        public Skill0(Card card) : base(card) {}
        public override int SkillNumber => 0;
        public override int CountOfTarget => 1;
        public override int ConsumableBp => 1;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            await Task.Delay(0);
            Target.Damage(1, this.IsSameTimeSkill);
        }
        public override void After() {
            
        }
    }
    private class Skill1: Active {
        public Skill1(Card card) : base(card) {}
        public override int SkillNumber => 1;
        public override int CountOfTarget => 0;
        public override int ConsumableBp => 1;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            await Task.Delay(0);
            Card.AddDefence(1);
        }
        public override void After() {
            
        }
    }
    private class Passive0 : Passive
    {
        public Passive0(Card card) : base(card) {}

        public override int SkillNumber => 0;
        public override PassiveActTiming When => PassiveActTiming.startbattle;
        public override void Act()
        {
            if(Card.target == global::Target.player) {
                foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
                    foreach(var card in arr) {
                        if(card != null) {
                            if(this.Card.speed > ((Card)card).speed) {
                                ((Card)card).DecreaseSpeed(2);
                            }
                        }
                    }
                }
            } else {
                foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
                    foreach(var card in arr) {
                        if(card != null) {
                            ((Card)card).DecreaseSpeed(2);
                        }
                    }
                }
            }
        }

        public override void SetTarget(Card target) {}
    }
    public override List<Active> Actives => new List<Active>() { new Skill0(Card), new Skill1(Card) };
    public override List<Passive> Passives => new List<Passive>() {new Passive0(Card)};
}