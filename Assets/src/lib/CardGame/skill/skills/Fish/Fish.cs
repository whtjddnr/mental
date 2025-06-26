public class Fish: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new FishActives.Skill0(card));
        Actives.Add(new FishActives.Skill1(card));
    }
}