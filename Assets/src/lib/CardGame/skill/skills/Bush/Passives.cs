namespace BushPassives {
    public class Skill0 : Passive
    {
        public Skill0(Card card) : base(card) {}

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
}