using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class CardType {
    public string id;
    public string name;
    public int defence;
    public int health;
    public int type;
    public JArray sacrifice;
    public int speed;
    public int adaptation;
    public JArray active;
    public JArray passive;
}
// "speed": 0,
//  "adaptation": 0,
public class Card
{
    public Target target;
    public CardLocation location;
    public int uid;
    public CardType spec;
    public GameObject gameObject;
    public States states;
    public SkillBundle skillBundle;
    public Card(Target target, int uid, CardType spec) {
        this.location = CardLocation.unknown;
        this.target = target;
        this.uid = uid;
        this.spec = spec;
        states = new States();
        skillBundle = SkillManager.CreateSkillBundle(this);
    }
    public void Damage(int damage, bool isSameTime) {
        Sequence sequence = DOTween.Sequence().SetAutoKill(false)
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x + 0.05f, 0.1f))
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x - 0.05f, 0.1f))
            .Append(this.gameObject.transform.DOMoveX(this.gameObject.transform.position.x, 0.05f))
            .OnComplete(() => {
                int calculatedDamage = spec.defence - damage;
                this.spec.defence -= damage;
                if(this.spec.defence < 0) this.spec.defence = 0;
                if(calculatedDamage < 0) this.spec.health += calculatedDamage;
                if(this.spec.health <= 0) {
                    CardGameEngine.Kill(this, isSameTime);
                } else {
                    this.UpdateStats();
                }
            });
    }
    public void AddDefence(int defence) {
        this.spec.defence += defence;
        this.UpdateStats();
    }
    public void DecreaseDefence(int defence) {
        if(this.spec.defence - defence < 0) {
            this.spec.defence = 0;
        } else {
            this.spec.defence -= defence;
        }
        this.UpdateStats();
    }
    public void AddSpeed(int speed) {
        this.spec.speed += speed;
        this.UpdateStats();
    }
    public void DecreaseSpeed(int speed) {
        if(this.spec.speed - speed < 0) {
            this.spec.speed = 0;
        } else {
            this.spec.speed -= speed;
            this.UpdateStats();
        }
        
    }
    public void AddAdaptation(int adaptation) {
        this.spec.adaptation += adaptation;
        this.UpdateStats();
    }
    public void DecreaseAdaptation(int adaptation) {
        if(this.spec.adaptation - adaptation < 0) {
            this.spec.adaptation = 0;
        } else {
            this.spec.adaptation -= adaptation;
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
        if(this.spec.health < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.spec.health}");
            childTransformHeal.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.spec.health / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.spec.health%10}");
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
        if(this.spec.defence < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.spec.defence}");
            childTransformDef.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.spec.defence / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.spec.defence%10}");
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
        if(this.spec.speed < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.spec.speed}");
            childTransformSpeed.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.spec.speed / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.spec.speed%10}");
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
        if(this.spec.adaptation < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.spec.adaptation}");
            childTransformAdapt.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.spec.adaptation / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.spec.adaptation%10}");
            childTransformAdapt1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformAdapt2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
    }
    public void Render(string theme, Vector3 vector) {
        this.gameObject = GameObject.Instantiate(
            Resources.Load<GameObject>($"Prefabs/card/card"), 
            new Vector3(vector.x, vector.y, vector.z-0.01f), 
            Quaternion.identity
        );
        // ref (TODO: have do not use find)
        Transform front = this.gameObject.transform.Find("display").Find("front");
        Transform childTransformCardImage = front.Find("cardImage");
        Transform childTransformType = front.Find("type");

        front.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/front"); // 카드 프레임
        childTransformCardImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/cardImage/{this.spec.id}"); // 몹 이미지
        childTransformType.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/{theme}/type/{this.spec.type}"); // 타입
        front.Find("name").GetComponent<TextMeshPro>().text = this.spec.name;
        // sacrifice
        for(var i = 0; i < this.spec.sacrifice.Count; i++) {
            Transform childTransformsacrifice = front.Find($"sacrifice{i}");
            var sacrificeImage = Resources.Load<Sprite>($"image/card/{theme}/sacrifice/{this.spec.sacrifice[i]}");
            childTransformsacrifice.GetComponent<SpriteRenderer>().sprite = sacrificeImage;
        }
        this.gameObject.GetComponent<CardComponent>().card = this;
        // stats
        UpdateStats();
        
        
    }
}
