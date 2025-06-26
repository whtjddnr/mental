using UnityEngine;

public class Arrow : MonoBehaviour {
    public GameObject[] points;
    public GameObject point;
    public GameObject cursor;
    public int numOfPoint;
    private Vector3 startPos;
    private Vector3 direction;
    public bool isShow;
    void Start() {
        points = new GameObject[numOfPoint];
        for(int i = 0; i < numOfPoint; i++) {
            if(i != numOfPoint-1) {
                points[i] = Instantiate(point, transform.position, Quaternion.identity);
                points[i].SetActive(false);
            } else {
                points[i] = Instantiate(cursor, transform.position, Quaternion.identity);
                points[i].SetActive(false);
            }
        }
    }
    void Update() {
        direction = new Vector3(CardGameEngine.Utill.GetMousePos().x, CardGameEngine.Utill.GetMousePos().y, -4);
        for(int i = 0; i < numOfPoint; i++) {
            points[i].transform.position = Vector3.Lerp(startPos, direction, i / (float)(numOfPoint - 1));
        }
    }
    public void Show(Vector3 startPos) {
        this.startPos = new Vector3(startPos.x, startPos.y, -4);
        this.isShow = true;
        for(int i = 0; i < numOfPoint; i++) {
            points[i].SetActive(true);
            points[i].GetComponent<ArrowVisual>().SetStartPos(startPos);
        }
    }
    public void Hide() {
        this.isShow = false;
        for(int i = 0; i < numOfPoint; i++) {
            points[i].SetActive(false);
        }
    }
}