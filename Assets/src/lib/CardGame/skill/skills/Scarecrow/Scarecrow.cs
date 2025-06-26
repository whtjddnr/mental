public class Scarecrow: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Passives.Add(new ScarecrowPassives.Skill0(card));
    }
}