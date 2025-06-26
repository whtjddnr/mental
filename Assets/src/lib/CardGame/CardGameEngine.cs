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
            // Behaviour Point
            PlayerManager.BehaviourPoint.Reset(game.CountOfturn);
            OpponentManager.BehaviourPoint.Reset(game.CountOfturn);
            // Draw
            PlayerManager.DrawManager.isDrew = false;
            PlayerManager.DrawManager.Draw(false);
            OpponentManager.DrawManager.Draw();
            // Field Effect
            CardGameEngine.game.field.Effect(GamePhase.turnBegin);
            // State
            StateManager.Act(StateActTiming.turnBegin);
            // Passive
            PassiveManager.Act(PassiveActTiming.turnBegin);
            // Next
            PhaseManager.Setting();
        }
        public static void Setting() {
            phase = GamePhase.setting;
            // OpponentManager.Algorithm.Summons();
        }
        public static void Startbattle() {
            phase = GamePhase.startbattle;
            // State
            StateManager.Act(StateActTiming.startbattle);
            // Passive
            PassiveManager.Act(PassiveActTiming.startbattle);
        }
        public static async Task Battle() {
            phase = GamePhase.battle;
            // Active
            await SkillManager.Active();
        }
        public static void FinishBattle() {
            phase = GamePhase.finishBattle;
            // State
            StateManager.Act(StateActTiming.finishBattle);
            // Passive
            PassiveManager.Act(PassiveActTiming.finishBattle);
        }
        public static void TurnEnd() {
            phase = GamePhase.turnEnd;
            // State
            StateManager.Act(StateActTiming.turnEnd);
            StateManager.DecreaseCountAll();
            // Passive
            PassiveManager.Act(PassiveActTiming.turnEnd);
            // Init Defence
            foreach(var arr in CardGameEngine.game.field.playerFieldArrangement) {
                foreach(var card in arr) {
                    if(card != null) {
                        ((Card)card).spec.defence = 0;
                        ((Card)card).UpdateStats();
                    }
                }
            }
            foreach(var arr in CardGameEngine.game.field.opponentFieldArrangement) {
                foreach(var card in arr) {
                    if(card != null) {
                        ((Card)card).spec.defence = 0;
                        ((Card)card).UpdateStats();
                    }
                }
            }
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
        Debug.Log(Player.deck.value[Player.deck.value.Count-1].spec.health);
    }
    public static Card SelectedCard;
    public static GameObject PoppedBook;
    public static async void Loop() {
        if(Input.GetMouseButtonDown(0)) {
            PlayerManager.DrawManager.DoneDraw();
            if(SkillBarManager.skillBars != null) {
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
