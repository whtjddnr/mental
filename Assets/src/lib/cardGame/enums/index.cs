public enum Target {
  opponent,
  player
}
public enum CardLocation {
  onDeck,
  onHand,
  onField
}
public enum PlayerBehaviour {
  nothing,
  selectCard,
  drawing,
  summoning,
  sacrificing,
  selectSkillTarget
}
public enum GamePhase {
  turnBegin,
  setting,
  startbattle,
  battle,
  finishBattle,
  turnEnd
}
public enum SkillTargettingType {
  
}