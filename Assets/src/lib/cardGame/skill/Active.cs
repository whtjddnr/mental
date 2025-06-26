using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public abstract class Active {
    public Active(Card card) {
        this.Card = card;
        this.Name = (string)((JObject)this.Card.spec.active[this.SkillNumber])["name"];
        this.Desc = (string)((JObject)this.Card.spec.active[this.SkillNumber])["desc"];
        this.ConsumableBp = (int)((JObject)this.Card.spec.active[this.SkillNumber])["bp"];
        this.RequiredAdaptation = (int)((JObject)this.Card.spec.active[this.SkillNumber])["adaptation"];
    }
    public Card Card;
    public Card Target;
    public string Name;
    public string Desc;
    public bool IsSameTimeSkill;
    public int ConsumableBp;
    public int RequiredAdaptation;
    public abstract int SkillNumber { get; }
    public abstract int CountOfTarget { get; }
    public abstract void SetTarget(Card target);
    public abstract void Before();
    public abstract Task Act();
    public abstract void After();
}