public class Overwhelmed : State
{
    public override bool IsVariableState => false;
    public override bool IsStatic => false;
    public Overwhelmed(Card card, int count) : base(card, count) {}
    public override string Id => "overwhelmed";
    public override string Name => "압도";
    public override string Description => "속도 2 감소, 이 카드에게 가하는 피해 -30%";
    public override StateActTiming When => StateActTiming.turnEnd;
    public override void Act() {
        this.card.Damage(1, false);
    }
}