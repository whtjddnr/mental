namespace LionPassives {
    public class Skill0 : Passive {
        public Skill0(Card card) : base(card) {}

        public override int SkillNumber => 0;
        public override PassiveActTiming When => PassiveActTiming.startbattle;
        public override void Act()
        {
            if(Card.target == global::Target.player) {
                foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
                    foreach(var card in arr) {
                        if(card != null) {
                            if(this.Card.spec.speed > ((Card)card).spec.speed) {
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
}