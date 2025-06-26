public class Test: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new TestActives.Skill0(card));
    }
}