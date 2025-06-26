using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public static class SkillBarManager {
    static private Card TargetCard;
    static public GameObject skillBars;
    static public void Show(Card card) {
        if(card.target == Target.player) {
            if(skillBars == null) {
                TargetCard = card;
                SkillBarManager.Remove();
                GameObject skillBarsContainer = new GameObject("skillBars"); // create container
                skillBarsContainer.transform.position = new Vector3(
                    card.gameObject.transform.position.x, 
                    card.gameObject.transform.position.y, 
                    card.gameObject.transform.position.z-1f
                );
                card.gameObject.GetComponent<CardVisual>().visualShadow.position = new Vector3(card.gameObject.transform.position.x, card.gameObject.transform.position.y, card.gameObject.transform.position.z-0.5f);
                for(int i = 0; i < card.spec.active.Count; i++) {
                    GameObject skillBar = GameObject.Instantiate( // create skill bar elements
                        Resources.Load<GameObject>($"Prefabs/skill"), 
                        new Vector3(
                            0, -card.gameObject.GetComponent<SpriteRenderer>().size.y*(i-1)/2, 0), 
                        Quaternion.identity
                    );
                    if(card.gameObject.GetComponent<CardComponent>().SkillBlock != null)
                        if(card.gameObject.GetComponent<CardComponent>().SkillBlock.skill.SkillNumber == i)
                            skillBar.GetComponent<SkillBar>().Select();
                    
                    skillBar.transform.SetParent(skillBarsContainer.transform, false); // set parent that container
                    skillBar.GetComponent<SkillBar>().card = card; // init card
                    skillBar.GetComponent<SkillBar>().skillNumber = i; // init skill number
                    string skillName = card.skillBundle.Actives[i].Name; // get name
                    int bp = card.skillBundle.Actives[i].ConsumableBp; // get adaptation
                    int requiredAdaptation = card.skillBundle.Actives[i].RequiredAdaptation; // get adaptation
                    skillBar.transform.Find("text").GetComponent<TextMeshPro>().text = $"{skillName}[{requiredAdaptation}]({bp})";
                }
                skillBars = skillBarsContainer;
            }
        }
    }
    static public void Remove() {
        if(skillBars != null && TargetCard != null) {
            GameObject.Destroy(skillBars);
            TargetCard.gameObject.GetComponent<CardVisual>().ReturnPosShadow();
            TargetCard = null;
        }
    }
}