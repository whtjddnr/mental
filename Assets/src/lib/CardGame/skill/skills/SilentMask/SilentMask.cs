public class SilentMask: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new SlientMaskSActives.Skill0(card));
    }
}