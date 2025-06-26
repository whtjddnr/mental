namespace ScarecrowPassives {
    public class Skill0 : Passive
    {
        public Skill0(Card card) : base(card) {}

        public override int SkillNumber => 0;
        public override PassiveActTiming When => PassiveActTiming.startbattle;
        public override void Act()
        {
            
        }

        public override void SetTarget(Card target) {}
    }
}