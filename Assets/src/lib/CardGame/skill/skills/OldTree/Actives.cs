using System.Threading.Tasks;

namespace OldTreeActives {
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
            await Task.Delay(0);
            Target.Damage(1, this.IsSameTimeSkill);
        }
        public override void After() {
            
        }
    }

}