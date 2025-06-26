public class Poisoning : State
{
    public override bool IsVariableState => true;
    public override bool IsStatic => false;
    public Poisoning(Card card, int count) : base(card, count) {}
    public override string Id => "poisoning";
    public override string Name => "중독";
    public override string Description => "전투 후 추가로 1의 피가 감소한다. (방어력 무시)";
    public override StateActTiming When => StateActTiming.turnEnd;
    public override void Act() {
        this.card.Damage(this.X, false);
    }
}