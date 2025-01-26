using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;


public static class CardGameEngine
{
    public static Game game;
    public static GamePhase phase;
    public static void Start()
    {
        CardGameEngine.game.Init();

        PhaseManager.TurnBegin();
    }
    public static void Init(Game game) {
        CardGameEngine.game = game;
        PlayerManager.BehaviourPoint.Init();
        OpponentManager.BehaviourPoint.Init();
    }
    
    public static class Utill {
        public static Vector3 GetMousePos() {
            var mousePos = Cam.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return mousePos;
        }
    }
    public static class PhaseManager {
        public static void TurnBegin() {
            phase = GamePhase.turnBegin;
            game.CountOfturn += 1;
            
            PlayerManager.BehaviourPoint.Reset(game.CountOfturn);
            OpponentManager.BehaviourPoint.Add(1);

            PlayerManager.DrawManager.isDrew = false;
            OpponentManager.DrawManager.Draw();
            OpponentManager.DrawManager.Draw();
            OpponentManager.DrawManager.Draw();

            for(int i = 0; i < game.opponentHand.value.Count; i++) {
                Debug.Log(game.opponentHand.value[i].name);
            }

            CardGameEngine.game.field.TurnStart();

            PassiveManager.Act(PassiveActTiming.turnBegin);
            StateManager.Act(StateActTiming.turnBegin);

            PhaseManager.Setting();
        }
        public static void Setting() {
            OpponentManager.Algorithm.Summons();
            phase = GamePhase.setting;
        }
        public static void Startbattle() {
            phase = GamePhase.startbattle;
            PassiveManager.Act(PassiveActTiming.startbattle);
            StateManager.Act(StateActTiming.startbattle);
            Debug.Log("start");
        }
        public static async Task Battle() {
            phase = GamePhase.battle;
            await SkillManager.Active();
            Debug.Log("act");
        }
        public static void FinishBattle() {
            phase = GamePhase.finishBattle;
            PassiveManager.Act(PassiveActTiming.finishBattle);
            StateManager.Act(StateActTiming.finishBattle);
            Debug.Log("done");
        }
        public static void TurnEnd() {
            phase = GamePhase.turnEnd;
            PassiveManager.Act(PassiveActTiming.turnEnd);
            StateManager.Act(StateActTiming.turnEnd);
            StateManager.DecreaseCountAll();
            foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
                foreach(var card in arr) {
                    if(card != null) {
                        ((Card)card).defence = 0;
                        ((Card)card).UpdateStats();
                    }
                }
            }
            foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
                foreach(var card in arr) {
                    if(card != null) {
                        ((Card)card).defence = 0;
                        ((Card)card).UpdateStats();
                    }
                }
            }
            Debug.Log("end");
        }
    }
    public static void KillAt(Target target, int row, int column) {
        Field field = CardGameEngine.game.field;
        ArrayList[] targetFieldArrangement = target == Target.player ? field.playerFieldArrangement: field.opponentFieldArrangement;
        GameObject cardObj = ((Card)targetFieldArrangement[column][row]).gameObject;
        SkillManager.RemoveBlock(cardObj.GetComponent<CardComponent>().SkillBlock);
        GameObject.Destroy(cardObj);
        targetFieldArrangement[column][row] = null;
    }
    public static void Kill(Card card, bool isSameTime) {
        GameObject cardObj = card.gameObject;
        var cardComponent = cardObj.GetComponent<CardComponent>();
        int column = cardComponent.column;
        int row = cardComponent.row;
        Target target = card.target;
        Field field = CardGameEngine.game.field;
        ArrayList[] targetFieldArrangement = target == Target.player ? field.playerFieldArrangement: field.opponentFieldArrangement;
        if(!isSameTime) SkillManager.RemoveBlock(cardObj.GetComponent<CardComponent>().SkillBlock); // 죽으면 발동하려헀던 스킬을 없앰.
        GameObject.Destroy(cardObj);
        targetFieldArrangement[column][row] = null;
        Debug.Log(Player.deck.value[Player.deck.value.Count-1].health);
    }
    public static Card SelectedCard;
    public static GameObject PoppedSkillBar;
    public static GameObject PoppedBook;
    public static async void Loop() {
        if(Input.GetMouseButtonDown(0)) {
            PlayerManager.DrawManager.DoneDraw();
            if(PoppedSkillBar != null) {
                Vector2 pos = Cam.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, (1<<7)|(1<<8));
                if(hit.collider == null) {
                    CardGameEngine.SelectedCard?.gameObject.GetComponent<CardComponent>().UnSelectThisCard();
                }
            }
            if(PoppedBook != null) {
                GameObject.Destroy(PoppedBook);
            }
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            PhaseManager.Startbattle();
            await PhaseManager.Battle();
            PhaseManager.FinishBattle();
            PhaseManager.TurnEnd();
            PhaseManager.TurnBegin();
        }
        
    }
}
