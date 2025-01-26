using DG.Tweening;
using UnityEngine;
public class Zone : MonoBehaviour {
    public Target target;
    public int row;
    public int column;
    void OnMouseDown() {
        if(PlayerManager.SummonsManager.isSacrificingComplete) {
            PlayerManager.SummonsManager.targetOfSummonsCard.gameObject.GetComponent<CardComponent>().tween.Pause();
            PlayerManager.SummonsManager.targetOfSummonsCard.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0);
            PlayerManager.SummonsManager.Summons(PlayerManager.SummonsManager.targetOfSummonsCard, row, column, false);
        }
    }
}