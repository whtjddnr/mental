using System.Threading.Tasks;

public abstract class Active {
    public Active(Card card) {
        this.Card = card;
    }
    public Card Card;
    public Card Target;
    public bool IsSameTimeSkill;
    public abstract int SkillNumber { get; }
    public abstract int CountOfTarget { get; }
    public abstract int ConsumableBp { get; }
    public abstract void SetTarget(Card target);
    public abstract void Before();
    public abstract Task Act();
    public abstract void After();
}