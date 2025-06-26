public enum Target {
  opponent,
  player
}
public enum CardLocation {
  unknown,
  onDeck,
  onHand,
  onField
}
public enum PlayerBehaviour {
  nothing,
  draggingCard,
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