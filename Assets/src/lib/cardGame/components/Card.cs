using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardComponent : MonoBehaviour
{
    [HideInInspector] public Card card;

    [Header("Field")]
    public int row;
    public int column;

    [HideInInspector] public Tween tween;
    [HideInInspector] public Vector3 tempPos;
    [HideInInspector] public Quaternion tempRotation;

    public bool isDragging;
    public SkillBlock SkillBlock;

    void Start() {
        if(card.location == CardLocation.onHand) {
            this.transform.position = new Vector3();
        }
        tempPos = this.transform.position;
    }
    void OnMouseEnter() {
        if(!isDragging && PlayerManager.playerBehaviour != PlayerBehaviour.sacrificing) {
            if(card.location == CardLocation.onHand) {
                if(PlayerManager.playerBehaviour != PlayerBehaviour.draggingCard) {
                    tween?.Pause();
                    this.transform.DOScale(2, 0);
                    this.transform.DOLocalRotateQuaternion(new Quaternion(), 0);
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.GetComponent<SpriteRenderer>().size.y, this.transform.position.z-2);
                }
            } else if(card.location == CardLocation.onField) {
                // if(!this.IsSelected()) this.DrawArrow();
            }
        }
    }
    void OnMouseExit() {
        if(!isDragging && PlayerManager.playerBehaviour != PlayerBehaviour.sacrificing) {
            if(card.location == CardLocation.onHand) {
                tween?.Pause();
                this.transform.DOLocalRotateQuaternion(tempRotation, .2f);
                this.transform.DOScale(1.5f, .2f);
                this.transform.DOMoveY(tempPos.y, .2f);
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, tempPos.z);
            } else if(card.location == CardLocation.onField) {
                // if(!this.IsSelected()) this.DestroyArrow();
            }
        }
    }
    void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)) {
            if(PlayerManager.playerBehaviour != PlayerBehaviour.sacrificing) {
                if(card.location == CardLocation.onHand) {
                    tween?.Pause();
                    PlayerManager.SummonsManager.targetOfSummonsCard = card;
                    if(!IsCardNeedSacrifice()) tween = this.transform.DOScale(1, 0.1f);
                } else if(card.location == CardLocation.onDeck) {
                    if(card.target == Target.player) PlayerManager.DrawManager.Draw(false);
                }
            }
        } else if(Input.GetMouseButtonDown(1)) {
            
        }

        if(Input.GetMouseButtonUp(0)) {
            tween?.Pause();
            if(card.location == CardLocation.onHand) {
                if(PlayerManager.playerBehaviour != PlayerBehaviour.sacrificing) {
                    if(IsCardNeedSacrifice()) {
                        if(SkillManager.IsAbleToAddBlock(1)) {
                            tween = DOTween.Sequence().SetAutoKill(false)
                                .Append(card.gameObject.transform.DORotate(new Vector3(0, 0, 1), 0.02f))
                                .Append(card.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.02f))
                                .Append(card.gameObject.transform.DORotate(new Vector3(0, 0, -1), 0.02f));
                            tween.SetLoops(1000);
                            PlayerManager.SummonsManager.SacrificeSummons(card);
                        }
                    } 
                    if(isDragging) {
                        isDragging = false;
                        PlayerManager.playerBehaviour = PlayerBehaviour.nothing;
                        if(!IsCardNeedSacrifice()) {
                            LayerMask ZoneLayer = 1<<6;
                            Vector2 pos = Cam.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 1f, ZoneLayer);
                            if(hit.collider != null) {
                                int zoneRow = hit.transform.GetComponent<Zone>().row;
                                int zoneColumn = hit.transform.GetComponent<Zone>().column;
                                Target zoneTarget = hit.transform.GetComponent<Zone>().target;
                                if(zoneTarget == Target.player && CardGameEngine.game.field.playerFieldArrangement[zoneColumn][zoneRow] == null) {
                                    if(SkillManager.IsAbleToAddBlock(1)) {
                                        PlayerManager.SummonsManager.Summons(this.card, zoneRow, zoneColumn, false);
                                    } else {
                                        ReturnToTempPos();
                                    }
                                } else {
                                    ReturnToTempPos();
                                }
                            } else {
                                ReturnToTempPos();
                            }
                        }
                    }
                }
            } else if(card.location == CardLocation.onField) {
                if(PlayerManager.playerBehaviour == PlayerBehaviour.sacrificing) {// Sacrifice The Card
                    this.Sacrificing();
                } else if(PlayerManager.playerBehaviour == PlayerBehaviour.selectSkillTarget) {
                    this.SetSkillTarget();
                    PlayerManager.playerBehaviour = PlayerBehaviour.nothing;
                    this.UnSelectThisCard();
                } else {
                    if(this.card.target == Target.player) this.SelectThisCard();
                }
            }
        } else if(Input.GetMouseButtonUp(1)) {
            PopDescBook();
        }
    }
    void OnMouseDrag(){
        if(card.location == CardLocation.onHand && PlayerManager.playerBehaviour != PlayerBehaviour.sacrificing) {
            if(!IsCardNeedSacrifice()) {
                isDragging = true;
                PlayerManager.playerBehaviour = PlayerBehaviour.draggingCard;
            }
        }
    }
    void Update() {
        if(isDragging) {
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                new Vector3(CardGameEngine.Utill.GetMousePos().x, CardGameEngine.Utill.GetMousePos().y, this.transform.position.z),
                Time.deltaTime * 20
            );
            if(Input.GetMouseButtonUp(0)) {
                
            }
        }
    }
    // ------------------------------------->>>>>>>>>>>>>>>>>>>>>> Functions
    private void ReturnToTempPos() {
        this.transform.DOScale(1.5f, 0);
        this.transform.DOMove(tempPos, 0);
        this.transform.eulerAngles = new Vector3();
    }
    private bool IsSelected() {
        if(CardGameEngine.SelectedCard == this.card) {
            return true;
        } else {
            return false;
        }
    }
    public void SelectThisCard() {
        if(PlayerManager.playerBehaviour == PlayerBehaviour.nothing) {
            if(card.location == CardLocation.onField) {
                CardGameEngine.SelectedCard?.gameObject.GetComponent<CardComponent>().UnSelectThisCard();
                CardGameEngine.SelectedCard = this.card;
                SkillBarManager.Show(this.card);
            }
        } 
    }
    public void UnSelectThisCard() {
        if(PlayerManager.playerBehaviour == PlayerBehaviour.nothing)
            if(card.location == CardLocation.onField)
                if(CardGameEngine.SelectedCard != null)
                    CardGameEngine.SelectedCard = null;
                    SkillBarManager.Remove();
    }
    private void Sacrificing() {
        PlayerManager.SummonsManager.countOfSacrifice += 1;
        if(PlayerManager.SummonsManager.targetOfSummonsCard.spec.sacrifice.Count == PlayerManager.SummonsManager.countOfSacrifice) {
            PlayerManager.playerBehaviour = PlayerBehaviour.nothing;
            PlayerManager.SummonsManager.isSacrificingComplete = true;
        }
        CardGameEngine.KillAt(card.target, row, column);
    }
    private void PopDescBook() {
        GameObject.Destroy(CardGameEngine.PoppedBook);
        GameObject book = GameObject.Instantiate( // create skill bar elements
            Resources.Load<GameObject>($"Prefabs/book"), 
            new Vector3(0, 0, -3), 
            Quaternion.identity
        );
        Book.Instance._name.GetComponent<TextMeshPro>().text = this.card.spec.name;
        Book.Instance.cardImage.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/default/cardImage/{this.card.spec.id}");

        GameObject[] sacrificeArr = new GameObject[] {Book.Instance.sacrifice0, Book.Instance.sacrifice1, Book.Instance.sacrifice2, Book.Instance.sacrifice3};
        for(int i = 0; i < this.card.spec.sacrifice.Count; i++) {
            sacrificeArr[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/default/sacrifice/{this.card.spec.sacrifice[0]}");
        }

        Book.Instance.type.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"image/card/default/type/{this.card.spec.type}"); // 타입
        // health
        if(this.card.spec.health < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.card.spec.health}");
            Book.Instance.heal.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            Transform childTransformHeal1 = Book.Instance.heal2.transform.Find("1");
            Transform childTransformHeal2 = Book.Instance.heal2.transform.Find("2");
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.card.spec.health / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.card.spec.health%10}");
            childTransformHeal1.GetComponent<SpriteRenderer>().sprite = numberImage1;
            childTransformHeal2.GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        if(this.card.spec.speed < 10) {
            var numberImage = Resources.Load<Sprite>($"image/number/{this.card.spec.speed}");
            Book.Instance.speed.GetComponent<SpriteRenderer>().sprite = numberImage;
        } else {
            var numberImage1 = Resources.Load<Sprite>($"image/number/{System.Math.Truncate((double)(this.card.spec.speed / 10))}");
            var numberImage2 = Resources.Load<Sprite>($"image/number/{this.card.spec.speed%10}");
            Book.Instance.speed2.transform.Find("1").GetComponent<SpriteRenderer>().sprite = numberImage1;
            Book.Instance.speed2.transform.Find("2").GetComponent<SpriteRenderer>().sprite = numberImage2;
        }
        GameObject[] skillNameArr = new GameObject[] {Book.Instance.skill1, Book.Instance.skill2, Book.Instance.skill3};
        GameObject[] skillDescArr = new GameObject[] {Book.Instance.skill1desc, Book.Instance.skill2desc, Book.Instance.skill3desc};
        for(int i = 0; i < this.card.spec.active.Count; i++) {
            JObject skill = (JObject)this.card.spec.active[i];
            skillNameArr[i].GetComponent<TextMeshPro>().text = (string)skill["name"];
            skillDescArr[i].GetComponent<TextMeshPro>().text = (string)skill["desc"];
        }
        
        CardGameEngine.PoppedBook = book;
    }
    public void SetSkill(int skillNumber) {
        SkillManager.RemoveBlock(SkillBlock); // 이 카드의 스킬이 이미 선택되어 있다면 지워버림
        this.SkillBlock = null;
        Active selectedSkill = this.card.skillBundle.Actives[skillNumber];
        if(SkillManager.IsAbleToAddBlock(selectedSkill.ConsumableBp)) {
            SkillBlock block = new SkillBlock(this.card, selectedSkill);
            SkillBlock = block;
            if(selectedSkill.CountOfTarget == 0) {
                SkillManager.AddBlock(block);
                this.UnSelectThisCard();
            } else {
                PlayerManager.playerBehaviour = PlayerBehaviour.selectSkillTarget;
                ArrowObj.Instance.Arrow.GetComponent<Arrow>().Show(this.transform.position);
            }
            SkillBarManager.Remove();
        }
    }
    private void SetSkillTarget() {
        if(CardGameEngine.SelectedCard != null) {
            ArrowObj.Instance.Arrow.GetComponent<Arrow>().Hide();
            CardGameEngine.SelectedCard.gameObject.GetComponent<CardComponent>().SkillBlock.skill.SetTarget(this.card);
            SkillManager.AddBlock(CardGameEngine.SelectedCard.gameObject.GetComponent<CardComponent>().SkillBlock);
        }
    }
    private bool IsCardNeedSacrifice() {
        return this.card.spec.sacrifice.Count > 0;
    }

}
