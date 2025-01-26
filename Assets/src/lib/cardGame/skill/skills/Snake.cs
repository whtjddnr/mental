using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class Snake: SkillBundle {
    public Snake(Card ownerCard) : base(ownerCard) {}
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
            Target.Damage(2, this.IsSameTimeSkill);
            State bleeding = new Bleeding(Target, 2);
            State poisoning = new Poisoning(Target, 2);
            Target.AddState(bleeding);
            Target.AddState(poisoning);
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
            this.Card.AddDefence(3);
        }
        public override void After() {
            
        }
    }
    public override List<Active> Actives => new List<Active>() { new Skill0(Card), new Skill1(Card) };
    public override List<Passive> Passives => new List<Passive>() {};
}