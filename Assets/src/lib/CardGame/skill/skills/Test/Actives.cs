using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TestActives {
    public class Skill0 : Active {
        public Skill0(Card card) : base(card) {}
        public override int SkillNumber => 0;
        public override int CountOfTarget => 1;
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
}