using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class SilentMask: SkillBundle {
    public SilentMask(Card ownerCard) : base(ownerCard) {}
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
        }
        public override void After() {
            
        }
    }
    public override List<Active> Actives => new List<Active>() { new Skill0(Card) };
    public override List<Passive> Passives => new List<Passive>() {};
}