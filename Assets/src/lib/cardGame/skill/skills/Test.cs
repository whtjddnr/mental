using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class Test: SkillBundle {
    public Test(Card ownerCard) : base(ownerCard) {}
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
            Task a = DOTween.Sequence().SetAutoKill(false).Join(this.Card.gameObject.transform.DORotate(new Vector3(180, 20, 20), 1f)).AsyncWaitForCompletion();
            await a;
            Target.Damage(3, this.IsSameTimeSkill);
            State bleeding = new Bleeding(Target, 2);
            Target.AddState(bleeding);

        }
        public override void After() {
            
        }
    }
    public override List<Active> Actives => new List<Active>() { new Skill0(Card) };
    public override List<Passive> Passives => new List<Passive>() {};
}