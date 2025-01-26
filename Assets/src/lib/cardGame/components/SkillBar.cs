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
    void OnMouseUp() {
        card.gameObject.GetComponent<CardComponent>().SetSkill(this.skillNumber);
    }
}