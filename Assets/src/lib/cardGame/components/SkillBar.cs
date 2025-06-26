using DG.Tweening;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
public class SkillBar : MonoBehaviour {
    public Card card;
    public int skillNumber;
    public void Select() {
        this.transform.Find("selected").gameObject.SetActive(true);
    }
    void OnMouseEnter() {
        
    }
    void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)) {
            if(card.spec.adaptation >= card.skillBundle.Actives[this.skillNumber].RequiredAdaptation) {
                card.gameObject.GetComponent<CardComponent>().SetSkill(this.skillNumber);
            }
        } else if(Input.GetMouseButtonUp(1)) {
            SkillManager.RemoveBlock(card.gameObject.GetComponent<CardComponent>().SkillBlock);
            card.gameObject.GetComponent<CardComponent>().SkillBlock = null;
            SkillBarManager.Remove();
        }
    }
}