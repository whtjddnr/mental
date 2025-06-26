public class Lion: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new LionActives.Skill0(card));
        Actives.Add(new LionActives.Skill1(card));
        Passives.Add(new LionPassives.Skill0(card));
    }
}