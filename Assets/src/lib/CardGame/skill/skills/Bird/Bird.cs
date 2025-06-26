public class Bird: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new BirdActives.Skill0(card));
        Actives.Add(new BirdActives.Skill1(card));
    }
}