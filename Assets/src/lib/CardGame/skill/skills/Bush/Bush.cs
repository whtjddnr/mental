public class Bush: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Passives.Add(new BushPassives.Skill0(card));
    }
}