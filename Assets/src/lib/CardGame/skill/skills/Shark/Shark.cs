public class Shark: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new SharkActives.Skill0(card));
        Actives.Add(new SharkActives.Skill1(card));
        Actives.Add(new SharkActives.Skill2(card));
    }
}