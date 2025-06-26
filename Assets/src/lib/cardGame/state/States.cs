using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum StateActTiming {
  turnBegin,
  skillSetting,
  startbattle,
  attack,
  sameTimeAttack,
  defence,
  finishBattle,
  turnEnd
}
public abstract class State {
    public State(Card card, int count) {
        this.card = card;
        this.Count += count;
    }
    public Card card;
    public int Count;
    public int X;
    abstract public bool IsVariableState { get; }
    abstract public bool IsStatic { get; }
    abstract public string Id { get; }
    abstract public string Name { get; }
    abstract public string Description { get; }
    public void IncreaseCount(int count) {
        Count += count;
    }
    public void DecreaseCount(int count) {
        Count -= count;
        if(Count <= 0) {
            card.states.value.Remove(this);
        }
    }
    public void IncreaseVariable(int x) {
        X += x;
    }
    public void DecreaseVariable(int x) {
        X -= x;
        if(X <= 0) {
            X = 0;
        }
    }
    abstract public StateActTiming When { get; }
    abstract public void Act();

}
public class States {
    public List<State> value = new List<State>() {};
    public void Add(State state) {
        bool isAppended = false;
        for(int i = 0; i < this.value.Count; i++) {
            State e = this.value[i];
            if(e.Id == state.Id) {
                isAppended = true;
                e.IncreaseCount(state.Count);
            }
        }
        if(!isAppended) {
            value.Add(state);
        }
    }
}