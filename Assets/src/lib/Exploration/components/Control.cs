using UnityEngine;

public class Control : MonoBehaviour {
    Rigidbody2D rigid;
    public float Speed = 0.5f;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start() {

    }
    void Update() {
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f) * Time.deltaTime;
        transform.position = transform.position + horizontal;
    }
}