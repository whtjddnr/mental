using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace FishActives {
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
            Vector3 cardPos = Card.gameObject.transform.position;
            Debug.Log(Card.uid);
            Task Sequence1 = DOTween.Sequence().SetAutoKill(false)
                .Append(Card.gameObject.transform.DOMove(new Vector3(cardPos.x, cardPos.y+0.1f, cardPos.z), 0.1f))
                .AsyncWaitForCompletion();
            await Sequence1;
            Card.gameObject.transform.DOMove(new Vector3(cardPos.x, cardPos.y, cardPos.z), 0.1f);
            Target.Damage(1, this.IsSameTimeSkill);
        }
        public override void After() {
            
        }
    }
    public class Skill1: Active {
        public Skill1(Card card) : base(card) {}
        public override int SkillNumber => 1;
        public override int CountOfTarget => 0;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            await Task.Delay(0);
            Card.AddDefence(2);
        }
        public override void After() {
            
        }
    }
}