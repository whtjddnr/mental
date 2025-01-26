using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class CardType {
    public int uid;
    public string id;
    public string name;
    public int defence;
    public int health;
    public int type;
    public JArray sacrifice;
    public JArray skill;
    public int speed;
    public int adaptation;

}
// "speed": 0,
//  "adaptation": 0,
public class Card
{
    public Target target;
    public CardLocation location;
    public int uid;
    public CardType spec;
    public string id;
    public string name;
    public int defence;
    public int health;
    public int type;
    public JArray sacrifice;
    public JArray skill;
    public int speed;
    public int adaptation;
    public GameObject gameObject;
    public States states;
    public SkillBundle skillBundle;
    public Card(Target target, int uid, CardType spec) {
        this.target = target;
        this.uid = uid;
        this.spec = spec;
        this.id = spec.id;
        this.name = spec.name;
        this.defence = spec.defence;
        this.health = spec.health;
        this.type = spec.type;
        this.sacrifice = spec.sacrifice;
        this.skill = spec.skill;
        this.speed = spec.speed;
        this.adaptation = spec.adaptation;
        states = new States();
        skillBundle = SkillManager.CreateSkillBundle(this);
    }
    public void Damage(int damage, bool isSameTime) {
        Sequence sequence = DOTween.Sequence().SetAutoKill(false)
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x + 0.05f, 0.1f))
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x - 0.05f, 0.1f))
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x, 0.05f))
            .OnComplete(() => {
                int calculatedDamage = defence - damage;
                this.defence -= damage;
                if(this.defence < 0) this.defence = 0;
                if(calculatedDamage < 0) this.health += calculatedDamage;
                if(this.health <= 0) {
                    CardGameEngine.Kill(this, isSameTime);
                } else {
                    this.UpdateStats();
                }
            });
    }
    public void AddDefence(int defence) {
        this.defence += defence;
        this.UpdateStats();
    }
    public void DecreaseDefence(int defence) {
        if(this.defence - defence < 0) {
            this.defence = 0;
        } else {
            this.defence -= defence;
        }
        this.UpdateStats();
    }
    public void AddSpeed(int speed) {
        this.speed += speed;
        this.UpdateStats();
    }
    public void DecreaseSpeed(int speed) {
        if(this.speed - speed < 0) {
            this.speed = 0;
        } else {
            this.speed -= speed;
            this.UpdateStats();
        }
        
    }
    public void AddAdaptation(int adaptation) {
        this.adaptation += adaptation;
        this.UpdateStats();
    }
    public void DecreaseAdaptation(int adaptation) {
        if(this.adaptation - adaptation < 0) {
            this.adaptation = 0;
        } else {
            this.adaptation -= adaptation;
            this.UpdateStats();
        }
        
    }
    public void AddState(State state) {
        this.states.Add(state);
        UpdateStats();
    }
    public void UpdateStats() {
        // health
        Transform front = this.gameObject.transform.Find("display").Find("front");
        Transform childTransformHeal = front.Find("heal");
        Transform childTransformHeal1 = front.Find("heal2").Find("1");
        Transform childTransformHeal2 = front.Find("heal2").Find("2");
        childTransformHeal.GetComponent<SpriteRenderer>().sprite = null;
        childTransformHeal1.GetComponent<SpriteRenderer>().sprite = null;
        childTransformHeal2.GetComponent<SpriteRenderer>().sprite = null;
        if(this.health < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.health}");
            childTransformHeal.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.health / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.health%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // defence
        Transform childTransformDef = front.Find("def");
        Transform childTransformDef1 = front.Find("def2").Find("1");
        Transform childTransformDef2 = front.Find("def2").Find("2");
        childTransformDef.GetComponent<SpriteRenderer>().sprite = null;
        childTransformDef1.GetComponent<SpriteRenderer>().sprite = null;
        childTransformDef2.GetComponent<SpriteRenderer>().sprite = null;
        if(this.defence < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.defence}");
            childTransformDef.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.defence / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.defence%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // state
        foreach(Transform child in front.Find("states").transform) {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < this.states.value.Count; i++) {
            State _state = this.states.value[i];
            GameObject stateObj = GameObject.Instantiate(
                Resources.Load<GameObject>($"Prefabs/state"), 
                new Vector3(0, 0, 0), 
                Quaternion.identity
            );
            float height = stateObj.GetComponent<SpriteRenderer>().size.y;
            stateObj.transform.position = new Vector3(stateObj.transform.position.x, -height*i, stateObj.transform.position.z);
            stateObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/state/{_state.Id}");
            stateObj.transform.Find("count").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/number/{_state.Count}");
            stateObj.transform.SetParent(front.Find("states").transform, false);
        }
        // speed
        Transform childTransformSpeed = front.Find("speed");
        Transform childTransformSpeed1 = front.Find("speed2").Find("1");
        Transform childTransformSpeed2 = front.Find("speed2").Find("2");
        childTransformSpeed.GetComponent<SpriteRenderer>().sprite = null;
        childTransformSpeed1.GetComponent<SpriteRenderer>().sprite = null;
        childTransformSpeed2.GetComponent<SpriteRenderer>().sprite = null;
        if(this.speed < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.speed}");
            childTransformSpeed.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.speed / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.speed%10}");
            childTransformSpeed1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformSpeed2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // adapt
        Transform childTransformAdapt = front.Find("adapt");
        Transform childTransformAdapt1 = front.Find("adapt2").Find("1");
        Transform childTransformAdapt2 = front.Find("adapt2").Find("2");
        childTransformAdapt.GetComponent<SpriteRenderer>().sprite = null;
        childTransformAdapt1.GetComponent<SpriteRenderer>().sprite = null;
        childTransformAdapt2.GetComponent<SpriteRenderer>().sprite = null;
        if(this.adaptation < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.adaptation}");
            childTransformAdapt.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.adaptation / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.adaptation%10}");
            childTransformAdapt1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformAdapt2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
    }
    public void Render(string theme, Vector3 vector) {
        var cardObj = GameObject.Instantiate(
            Resources.Load<GameObject>($"Prefabs/card/card"), 
            new Vector3(vector.x, vector.y, vector.z-0.01f), 
            Quaternion.identity
        );
        Transform front = cardObj.transform.Find("display").Find("front");

        Transform childTransformCardImage = front.Find("cardImage");
        Transform childTransformType = front.Find("type");

        front.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/front"); // 카드 프레임
        childTransformCardImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/cardImage/{this.id}"); // 몹 이미지
        childTransformType.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/type/{this.type}"); // 타입
        front.Find("name").GetComponent<TextMeshPro>().text = this.name;

        for(var i = 0; i < this.sacrifice.Count; i++) {
            Transform childTransformsacrifice = front.Find($"sacrifice{i}");
            var sacrificeImage = Resources.Load<Sprite>($"image/card/{theme}/sacrifice/{this.sacrifice[i]}");
            childTransformsacrifice.GetComponent<SpriteRenderer>().sprite = sacrificeImage;
        }
        //defence
        if(this.defence < 10) {
            Transform childTransformDef = front.Find("def");
            var numberImage = Resources.Load<Sprite>($"image/number/{this.defence}");
            childTransformDef.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            Transform childTransformHeal1 = front.Find("def2").Find("1");
            Transform childTransformHeal2 = front.Find("def2").Find("2");
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.defence / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.defence%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // health
        if(this.health < 10) {
            Transform childTransformHeal = front.Find("heal");
            var numberImage = Resources.Load<Sprite>($"image/number/{this.health}");
            childTransformHeal.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            Transform childTransformHeal1 = front.Find("heal2").Find("1");
            Transform childTransformHeal2 = front.Find("heal2").Find("2");
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.health / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.health%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // speed
        if(this.speed < 10) {
            Transform childTransformSpeed = front.Find("speed");
            var numberImage = Resources.Load<Sprite>($"image/number/{this.speed}");
            childTransformSpeed.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            Transform childTransformSpeed1 = front.Find("speed2").Find("1");
            Transform childTransformSpeed2 = front.Find("speed2").Find("2");
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.speed / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.speed%10}");
            childTransformSpeed1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformSpeed2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        // adapt
        if(this.adaptation < 10) {
            Transform childTransformDef = front.Find("adapt");
            var numberImage = Resources.Load<Sprite>($"image/number/{this.adaptation}");
            childTransformDef.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            Transform childTransformHeal1 = front.Find("adapt2").Find("1");
            Transform childTransformHeal2 = front.Find("adapt2").Find("2");
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.adaptation / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.adaptation%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        var cardComponent = cardObj.GetComponent<CardComponent>();
        cardComponent.card = this;
        this.gameObject = cardObj;
        
    }
}
