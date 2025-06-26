using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

static class PlayerManager {
    public static PlayerBehaviour playerBehaviour = PlayerBehaviour.nothing;
    public static class BehaviourPoint {
        public static List<GameObject> gameObjects = new List<GameObject>() {};
        public static void Init() {
            Reset(0);
        }
        public static void Reset(int num) {
            int temp = 0;
            temp += CardGameEngine.game.playerBehaviourPoint;
            CardGameEngine.game.playerBehaviourPoint = num;
            for(int i = 0; i < num-temp; i++) {
                GameObject point = GameObject.Instantiate(
                    Resources.Load<GameObject>($"Prefabs/behaviourPoint"), 
                    new Vector3(0, 0, 0), 
                    Quaternion.identity
                );
                float height = point.GetComponent<SpriteRenderer>().bounds.size.y;
                float width = point.GetComponent<SpriteRenderer>().bounds.size.x;
                float x = width*(temp+i+1)-Cam.Instance.Camera.orthographicSize*Cam.Instance.Camera.aspect;
                float y = height/2-Cam.Instance.Camera.orthographicSize;
                point.transform.position = new Vector3(x, y, -1);
                PlayerManager.BehaviourPoint.gameObjects.Add(point);
            }
        }
        public static void Remove(int num) {
            for(int i = 0; i < num; i++) {
                CardGameEngine.game.playerBehaviourPoint -= 1;
                GameObject.Destroy(gameObjects[gameObjects.Count-1]);
                gameObjects.RemoveAt(gameObjects.Count-1);
            }
        }
    }
    public static class DrawManager {
        public static bool isDrew = false;
        private static Sequence shakeSequence;
        private static Transform drewCard;
        public static void Draw(bool skipMotion) {
            if(!isDrew && CardGameEngine.game.playerDeck.value.Count != 0) {
                if(playerBehaviour != PlayerBehaviour.drawing) {
                    playerBehaviour = PlayerBehaviour.drawing;
                    Game game = CardGameEngine.game;
                    int currentCardIndex = game.playerDeck.value.Count - 1;
                    Transform card = GameObject.Find("playerDeckZone").transform.Find($"{currentCardIndex}");
                    Sequence drawSequence = DOTween.Sequence().SetAutoKill(false)
                        .Append(card.transform.DOMoveY(-6, 0.6f)) // 1. 아래로 내림
                        .Join(card.transform.DOMoveZ(-4-0.5f*(game.playerHand.value.Count+1), 0)) // 2. z값이 패에 있는 카드들 보다 높아야 덮히지 않음
                        .Append(card.transform.DOMoveX(0, 0)) // 3. x축을 이동시켜 중앙에 배치
                        .OnComplete(() => {
                            if(skipMotion) {
                                drewCard = card;
                                DoneDraw();
                                isDrew = true;
                            } else {
                                DOTween.Sequence().SetAutoKill(false)
                                .Join(card.gameObject.transform.DOScale(2, 0)) // 4. 커져라!
                                .Append(card.gameObject.transform.Find("display").DORotate(new Vector3(0, 0, 0), 0)) // 5. 앞면으로 뒤집음
                                .Append(card.gameObject.transform.DOMoveY(0, 0.5f)) // 6. 올려버려라!
                                .OnComplete(() => {
                                    shakeSequence = DOTween.Sequence().SetAutoKill(false) // 7. 흔들어주세요~
                                        .Append(card.gameObject.transform.DORotate(new Vector3(0, 0, 3), 20)).SetEase(Ease.OutQuad)
                                        .Append(card.gameObject.transform.DORotate(new Vector3(0, 0, -3), 10)).SetEase(Ease.OutQuad);
                                    drewCard = card;
                                    isDrew = true;
                                });
                            }
                        });
                }
            }
        }
        public static void DoneDraw() {
            if(drewCard != null) { // after player draw card srot to by clicking
                playerBehaviour = PlayerBehaviour.nothing;
                shakeSequence.Pause();
                drewCard.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
                Deck targetrDeck = CardGameEngine.game.playerDeck;
                int index = targetrDeck.value.Count - 1;

                HandManager.Add(targetrDeck.value[index]);
                
                targetrDeck.Remove(index);
                drewCard = null;
            }
        }
    }
    public static class SummonsManager {
        public static Card targetOfSummonsCard;
        public static bool isSacrificingComplete = false;
        public static int countOfSacrifice = 0;
        public static void Summons(Card card, int row, int column, bool isSpecialSummons) {
            if(CardGameEngine.game.field.playerFieldArrangement[column][row] == null) {
                isSacrificingComplete = false;
                targetOfSummonsCard = null;
                
                if(isSpecialSummons) {
                    card.Render(CardGameEngine.game.field.theme, new Vector3());
                    CardGameEngine.game.field.Put(Target.player, card, row, column, false);
                    card.gameObject.transform.DOScale(1, 0.2f);
                    card.location = CardLocation.onField;
                } else {
                    CardGameEngine.game.field.Put(Target.player, card, row, column, false);
                    card.gameObject.transform.DOScale(1, 0.2f);
                    card.location = CardLocation.onField;
                    CardGameEngine.game.playerHand.Remove(card);
                    HandManager.SortHand();
                    PlayerManager.BehaviourPoint.Remove(1);
                }
            }
            
        }
        public static void SacrificeSummons(Card card) {
            PlayerManager.playerBehaviour = PlayerBehaviour.sacrificing;
            targetOfSummonsCard = card;
            for(int i = 0; i < CardGameEngine.game.playerHand.value.Count; i++) { // 선택한 카드를 중앙으로 정렬
                Card _card = CardGameEngine.game.playerHand.value[i];
                if(_card.uid != card.uid) {
                    _card.gameObject.transform.DOMoveY(-Cam.Instance.Camera.orthographicSize-4, 0.5f);
                } else {
                    _card.gameObject.transform.DOMoveX(0, 0.5f);
                }
            }
        }
    }
}