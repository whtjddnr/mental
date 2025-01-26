using TMPro;
using UnityEngine;

public class TextBox {
    public int length;
    public Vector3 pos;
    public string context;
    public GameObject gameObject = new GameObject();
    public TextBox(int length, Vector3 pos, string context) {
        this.length = length;
        this.pos = pos;
        this.context = context;

        gameObject.transform.position = this.pos;
        GameObject startBlock = GameObject.Instantiate(
            Resources.Load<GameObject>($"Prefabs/bar/start"), 
            new Vector3(0, 0, 0), 
            Quaternion.identity
        );
        startBlock.transform.SetParent(gameObject.transform);
        float width = startBlock.GetComponent<SpriteRenderer>().bounds.size.x;
        float height = startBlock.GetComponent<SpriteRenderer>().bounds.size.y;
        for(int i = 0; i < this.length; i++) {
            GameObject middleBlock = GameObject.Instantiate(
                Resources.Load<GameObject>($"Prefabs/bar/middle"), 
                new Vector3(width*(i+1), 0, 0), 
                Quaternion.identity
            );
            middleBlock.transform.SetParent(gameObject.transform);
        }
        GameObject endBlock = GameObject.Instantiate(
            Resources.Load<GameObject>($"Prefabs/bar/end"), 
            new Vector3(width*(length+1), 0, 0), 
            Quaternion.identity
        );
        endBlock.transform.SetParent(gameObject.transform);

        float totalWidth = width*(this.length+2);

        GameObject textObj = GameObject.Instantiate(
            Resources.Load<GameObject>($"Prefabs/text"), 
            new Vector3(0, 0, 0), 
            Quaternion.identity
        );
        textObj.GetComponent<TextMeshPro>().margin = new Vector4(0, 0, 0, 0);
        textObj.GetComponent<TextMeshPro>().text = this.context;
        textObj.GetComponent<TextMeshPro>().fontSize = 3;
        textObj.transform.SetParent(gameObject.transform);
        gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        textObj.transform.position = new Vector3(0, 0, -1);

    }
}