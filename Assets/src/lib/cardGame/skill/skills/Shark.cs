using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
public class Shark: SkillBundle {
    public Shark(Card ownerCard) : base(ownerCard) {}
    private class Skill0 : Active {
        public Skill0(Card card) : base(card) {}
        public override int SkillNumber => 0;
        public override int CountOfTarget => 1;
        public override int ConsumableBp => 1;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            Vector3 TempTargetPos = Target.gameObject.transform.position;

            Task Sequence1 = DOTween.Sequence().SetAutoKill(false)
                .Append(this.Card.gameObject.transform.DOScale(new Vector3(0, 0, 0), .5f))
                .Append(this.Target.gameObject.transform.DOMoveZ(Target.gameObject.transform.position.z-2, .5f))
                .AsyncWaitForCompletion();
            await Sequence1;
            Camera cam = Cam.Instance.Camera;
            Vector3 TargetPos = Target.gameObject.transform.position;
            
            GameObject shark = GameObject.Instantiate(
                Resources.Load<GameObject>($"Prefabs/skill/shark"), 
                new Vector3(0, 0, 0), 
                Quaternion.identity
            );
            GameObject inSide = shark.transform.Find("a").gameObject;
            GameObject bg = new GameObject("bg");
            bg.AddComponent<SpriteRenderer>();
            bg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("image/skill/shark/bg");
            Task Sequence2 = DOTween.Sequence().SetAutoKill(false)
                // 투명화
                .Append(shark.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), 0))
                .Join(inSide.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), 0))
                .Join(bg.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), 0))

                // 위치로!
                .Join(shark.transform.DOMove(new Vector3(TargetPos.x, TargetPos.y, TargetPos.z+0.1f), 0))
                .Join(bg.transform.DOMove(new Vector3(TargetPos.x, TargetPos.y, TargetPos.z+1f), 0))

                // 카메라 이동
                .Append(cam.transform.DOMove(new Vector3(TargetPos.x, TargetPos.y+shark.GetComponent<SpriteRenderer>().size.y/2, cam.transform.position.z), 0.5f))
                .Join(DOTween.To(() => cam.orthographicSize, b => cam.orthographicSize = b, 2, 0.5f))
                
                // 커져라!
                .Append(bg.transform.DOScale(new Vector3(6.5f, 6.5f, 6.5f), 0))
                // 배경 서서히 보임!
                .Append(bg.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 1), 0.7f))

                // 상어 서서히 보임!
                .Append(shark.transform.DOScale(new Vector3(3.5f, 3.5f, 3.5f), 0.7f))
                .Join(shark.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0.8f), 0.7f))
                .Join(inSide.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0.6f), 0.7f))

                // 왕!
                .Append(shark.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 1), 0))
                .Join(cam.transform.DOShakePosition(2, 0.1f, 2))
                .Join(inSide.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 1), 0))
                .Join(shark.transform.DOMoveZ(TargetPos.z-0.08f, 0))

                .AsyncWaitForCompletion();
            await Sequence2;
            Target.Damage(3, this.IsSameTimeSkill);
            State bleeding = new Bleeding(Target, 2);
            Target.AddState(bleeding);
            await Task.Delay(1000);
            Task Sequence3 = DOTween.Sequence().SetAutoKill(false)
                .Append(shark.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), .3f))
                .Join(inSide.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), .3f))
                .Join(bg.GetComponent<SpriteRenderer>().DOColor(new Color(0, 0, 0, 0), .3f))
                .Append(cam.transform.DOMove(new Vector3(0, 0, cam.transform.position.z), 1))
                .Join(DOTween.To(() => cam.orthographicSize, b => cam.orthographicSize = b, 3, 0.5f))
                .Append(this.Card.gameObject.transform.DOScale(new Vector3(1, 1, 1), .5f))
                .Append(this.Target.gameObject.transform.DOMoveZ(TempTargetPos.z, .5f))
                .AsyncWaitForCompletion();
            await Sequence3;
            GameObject.Destroy(shark);
            GameObject.Destroy(bg);
        }
        public override void After() {
            
        }
    }
    private class Skill1: Active {
        public Skill1(Card card) : base(card) {}
        public override int SkillNumber => 1;
        public override int CountOfTarget => 0;
        public override int ConsumableBp => 2;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            await Task.Delay(0);
            Target.Damage(3, this.IsSameTimeSkill);
            State bleeding = new Bleeding(Target, 2);
            Target.AddState(bleeding);

        }
        public override void After() {
            
        }
    }
    private class Skill2: Active {
        public Skill2(Card card) : base(card) {}
        public override int SkillNumber => 2;
        public override int CountOfTarget => 1;
        public override int ConsumableBp => 3;
        public override void SetTarget(Card target) {
            this.Target = target;
        }
        public override void Before() {
            
        }
        public override async Task Act() {
            await Task.Delay(0);
            Target.Damage(3, this.IsSameTimeSkill);
            State bleeding = new Bleeding(Target, 2);
            Target.AddState(bleeding);

        }
        public override void After() {
            if(this.Target.defence == 0 && this.Target.type != 3) {
                CardGameEngine.Kill(Target, false);
            }
        }
    }
    public override List<Active> Actives => new List<Active>() { new Skill0(Card), new Skill1(Card), new Skill2(Card) };
    public override List<Passive> Passives => new List<Passive>() {};
}