using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillBlock : IComparable {
    public Card card;
    public Active skill;
    public SkillBlock(Card card, Active skill) {
        this.card = card;
        this.skill = skill;
    }
    public int CompareTo(object obj)
    {
        // 비교 대상의 속도 보다 크면 -1을 반환	
        if (this.card.speed > (obj as SkillBlock).card.speed)
            return -1;
        // 비교 대상의 속도와 같으면 0을 반환
        else if (this.card.speed == (obj as SkillBlock).card.speed)
            return 0;
        // 비교 대상의 속도 보다 작으면 1을 반환
        else
            return 1;
    }
}

public static class SkillManager {
    private static List<SkillBlock> skillBlocks = new List<SkillBlock>() {};
    private static int totalConsumableBp = 0;
    public static bool IsAbleToAddBlock(int bp) {
        Debug.Log($"{CardGameEngine.game.playerBehaviourPoint}, {totalConsumableBp}+{bp}");
        if(CardGameEngine.game.playerBehaviourPoint >= totalConsumableBp+bp) {
            return true;
        } else {
            return false;
        }
    }
    public static void AddBlock(SkillBlock block) {
        skillBlocks.Add(block);
        totalConsumableBp += block.skill.ConsumableBp;
        Debug.Log("Add skill block: " + $"{(int)skillBlocks.Count}");
        // if(IsAbleToAddBlock(block.skill.ConsumableBp)) {
            
        // } else {
        //     block.card.gameObject.GetComponent<CardComponent>().SkillBlock = null;
        // }
    }
    public static void RemoveBlock(SkillBlock block) {
        skillBlocks.Remove(block);
        Debug.Log("Remove skill block: " + $"{(int)skillBlocks.Count}");
    }
    public static async Task Active() {
        skillBlocks.Sort();
        for(int i = 0; i < skillBlocks.Count; i++) {
            if(!skillBlocks[i].skill.IsSameTimeSkill) SetSameTime(i);
            skillBlocks[i].skill.Before();
            await skillBlocks[i].skill.Act();
            skillBlocks[i].skill.After();
            skillBlocks[i].card.gameObject.GetComponent<CardComponent>().SkillBlock = null;
        }
        skillBlocks.RemoveAll(a => a != null);
        PlayerManager.BehaviourPoint.Remove(totalConsumableBp);
        totalConsumableBp = 0;
    }
    public static void SetSameTime(int index) {
        if(skillBlocks.Count >= index+1+1) {
            if(skillBlocks[index].card.speed == skillBlocks[index+1].card.speed) {
                skillBlocks[index].skill.IsSameTimeSkill = true;
                skillBlocks[index+1].skill.IsSameTimeSkill = true;
                SetSameTime(index+1);
            }
        }
    }
    public static SkillBundle CreateSkillBundle(Card ownerCard) {
        SkillBundle result;
        string cardId = ownerCard.id;
        if(cardId == "bird") result = new Bird(ownerCard);
        else if(cardId == "fish") result = new Fish(ownerCard);
        else if(cardId == "shark") result = new Shark(ownerCard);
        else if(cardId == "test") result = new Test(ownerCard);
        else if(cardId == "bush") result = new Bush(ownerCard);
        else if(cardId == "oldTree") result = new OldTree(ownerCard);
        else if(cardId == "snake") result = new Snake(ownerCard);
        else if(cardId == "lion") result = new Lion(ownerCard);
        else result = new Bird(ownerCard);
        return result;
    }
}