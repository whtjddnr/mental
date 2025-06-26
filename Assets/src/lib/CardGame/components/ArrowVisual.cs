using UnityEngine;

public class ArrowVisual : MonoBehaviour {
    float angle;
    private Vector2 startPos, mouse;
    private bool isActive;
    public void SetStartPos(Vector2 startPos) {
        this.startPos = startPos;
        isActive = true;
    }
    void Start () {
        
    }
    void Update() {
        if(isActive) {
            mouse = CardGameEngine.Utill.GetMousePos();
            angle = Mathf.Atan2(mouse.y - startPos.y, mouse.x - startPos.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
    }
}