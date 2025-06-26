using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Field {
    public string theme = "default";
    private Opponent opponent;
    public int[] opponentFieldConstruction;
    public int[] playerFieldConstruction;
    public ArrayList[] playerFieldArrangement = new ArrayList[] {new ArrayList(), new ArrayList()};
    public ArrayList[] opponentFieldArrangement = new ArrayList[] {new ArrayList(), new ArrayList()};
    private ArrayList[] playerFieldPos = new ArrayList[] {new ArrayList(), new ArrayList()};
    private ArrayList[] opponentFieldPos = new ArrayList[] {new ArrayList(), new ArrayList()};
    public Field(int[] playerFieldConstruction, int[] opponentFieldConstruction, Opponent opponent) {
        this.opponentFieldConstruction = opponentFieldConstruction;
        this.playerFieldConstruction = playerFieldConstruction;
        this.opponent = opponent;
        // field 구조에 맞춰 field 배치에 null을 채워 초기화
        for(var i = 0; i < playerFieldConstruction.Length; i++) {
            for(var n = 0; n < playerFieldConstruction[i]; n++) {
                playerFieldArrangement[i].Add(null);
            }
        }
        for(var i = 0; i < opponentFieldConstruction.Length; i++) {
            for(var n = 0; n < opponentFieldConstruction[i]; n++) {
                opponentFieldArrangement[i].Add(null);
            }
        }
        
    }
    public void Put(Target target, Card card, int row, int column, bool skipMove) {
        float zIndex = -0.05f;
        if(target == Target.player) {
            if(skipMove) {
                card.gameObject.transform.position = new Vector3(((Vector3)this.playerFieldPos[column][row]).x, ((Vector3)this.playerFieldPos[column][row]).y, zIndex);
                card.gameObject.transform.eulerAngles = new Vector3();

            } else {
                card.gameObject.transform.DOMove(new Vector3(((Vector3)this.playerFieldPos[column][row]).x, ((Vector3)this.playerFieldPos[column][row]).y, zIndex), 0.2f);
                card.gameObject.transform.DORotate(Vector3.zero, 0.2f);
            }
            card.gameObject.GetComponent<CardComponent>().row = row;
            card.gameObject.GetComponent<CardComponent>().column = column;
            this.playerFieldArrangement[column][row] = card;
        } else {
            card.gameObject.transform.position = new Vector3(((Vector3)this.opponentFieldPos[column][row]).x, ((Vector3)this.opponentFieldPos[column][row]).y, zIndex);
            card.gameObject.GetComponent<CardComponent>().row = row;
            card.gameObject.GetComponent<CardComponent>().column = column;
            this.opponentFieldArrangement[column][row] = card;
        }
    }
    public void Init() {
        GameObject container = new GameObject("field");
        // field 좌표 계산 및 계산값 저장
        var zone =  Resources.Load<GameObject>($"Prefabs/zone");
        var zoneImage = Resources.Load<Sprite>($"image/zone/{theme}");
        var zoneWidth = zone.GetComponent<SpriteRenderer>().bounds.size.x;
        var zoneHeight = zone.GetComponent<SpriteRenderer>().bounds.size.y;
        for(int i = 0; i < opponentFieldConstruction.Length; i++) {
            for(int n = 0; n < opponentFieldConstruction[i]; n++) {
                var x = -(opponentFieldConstruction[i]*zoneWidth/2 + zoneWidth/4*(opponentFieldConstruction[i]-1)) + (zoneWidth/2) + zoneWidth/4*(n+2) + n*zoneWidth + (zoneWidth/4/2*(opponentFieldConstruction[i]%2-1));
                var y = zoneHeight/2+zoneHeight*i+zoneWidth/4;
                var z = 0.1f;
                var vector = new Vector3(x, y, z);
                this.opponentFieldPos[i].Add(vector);
                var obj = GameObject.Instantiate(zone, vector, Quaternion.identity);
                obj.transform.SetParent(container.transform, false);
                obj.name = $"opponent{i}_{n}";
                obj.AddComponent<Zone>();
                obj.GetComponent<Zone>().target = Target.opponent;
                obj.GetComponent<Zone>().row = n;
                obj.GetComponent<Zone>().column = i;
            }
        } 
        for(int i = 0; i < playerFieldConstruction.Length; i++) {
            for(int n = 0; n < playerFieldConstruction[i]; n++) {
                var x = -(playerFieldConstruction[i]*zoneWidth/2 + zoneWidth/4*(playerFieldConstruction[i]-1)) + (zoneWidth/2) + zoneWidth/4*(n+2) + n*zoneWidth + (zoneWidth/4/2*(playerFieldConstruction[i]%2-1));
                var y = -(zoneHeight/2+zoneHeight*i+zoneWidth/4);
                var z = 0.1f;
                var vector = new Vector3(x, y, z);
                this.playerFieldPos[i].Add(vector);
                var obj = GameObject.Instantiate(zone, vector, Quaternion.identity);
                obj.transform.SetParent(container.transform, false);
                obj.name = $"player{i}_{n}";
                obj.AddComponent<Zone>();
                obj.GetComponent<Zone>().target = Target.player;
                obj.GetComponent<Zone>().row = n;
                obj.GetComponent<Zone>().column = i;
            }
        }
        // player
        var deckZone =  Resources.Load<GameObject>($"Prefabs/deckZone");
        var playerDeckZone = GameObject.Instantiate(
            deckZone, 
            new Vector3(
                Cam.Instance.Camera.orthographicSize*Cam.Instance.Camera.aspect-zoneWidth*1.5f/2, 
                -Cam.Instance.Camera.orthographicSize,
                -2
            ), 
            Quaternion.identity
        );
        playerDeckZone.transform.DOScale(1.5f, 0);
        playerDeckZone.AddComponent<DeckZone>();
        playerDeckZone.name = "playerDeckZone";
        playerDeckZone.GetComponent<DeckZone>().target = Target.player;
        playerDeckZone.transform.SetParent(container.transform, false);
        
        for(var i = 0; i < Player.deck.value.Count; i++) {
            var card =  Player.deck.value[i];
            card.location = CardLocation.onDeck;
            card.Render(theme, new Vector3(0, zoneHeight/130*2*i, -1));
            card.gameObject.transform.Find("display").rotation = Quaternion.Euler(0, 180, 0);
            card.gameObject.name = $"{i}";
            card.gameObject.transform.SetParent(playerDeckZone.transform, false);
        }
    }
    public abstract void Effect(GamePhase gamePhase);
}