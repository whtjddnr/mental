public class Snake: SkillBundle {
    public override void Init(Card card) {
        this.Card = card;
        Actives.Add(new SnakeActives.Skill0(card));
        Actives.Add(new SnakeActives.Skill1(card));
    }
}