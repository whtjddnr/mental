public enum PassiveActTiming {
    
    turnBegin,
    skillSetting,
    summoned,
    startbattle,
    sameTimeAttack,
    attack,
    defence,
    finishBattle,
    turnEnd
}
public abstract class Passive {
    public Passive(Card card) {
        this.Card = card;
    }
    public Card Card;
    public Card Target;
    public bool IsSameTimeSkill;
    public abstract int SkillNumber { get; }
    public abstract void SetTarget(Card target);
    abstract public PassiveActTiming When { get; }
    abstract public void Act();
}