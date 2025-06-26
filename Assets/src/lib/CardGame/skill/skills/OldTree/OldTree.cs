public class OldTree: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new OldTreeActives.Skill0(card));
    }
}