using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public static class HandManager
{
    public static void Add(Card card) {
        if(card.gameObject == null) card.Render(CardGameEngine.game.field.theme, new Vector3());
        CardGameEngine.game.playerHand.Add(card);
        card.gameObject.transform.DOScale(1.5f, .2f);
        card.location = CardLocation.onHand;
        card.gameObject.transform.SetParent(HandObj.Instance.Hand.transform, true);
        SortHand();
    }
    public static void SortHand() {
        if(CardGameEngine.game.playerHand.value.Count == 0) return;
        int maxHandCount = 10;
        float cardSpacing = 1f/maxHandCount;
        float firstCardPos = 0.5f - (CardGameEngine.game.playerHand.value.Count - 1) * cardSpacing/2;
        Spline spline = HandObj.Instance.Hand.GetComponent<SplineContainer>().Spline;
        for(int i = 0; i < CardGameEngine.game.playerHand.value.Count; i++) {
            var cardObj = CardGameEngine.game.playerHand.value[i];
            float p = firstCardPos + i * cardSpacing;
            Vector3 splinePos = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            cardObj.gameObject.transform.DOMove(new Vector3(splinePos.x, splinePos.y, -(3+0.01f*i)), 0.25f);
            cardObj.gameObject.transform.DOLocalRotateQuaternion(rotation, 0.25f)
            .OnComplete(() => {
                cardObj.gameObject.GetComponent<CardComponent>().tempPos = cardObj.gameObject.transform.position;
                cardObj.gameObject.GetComponent<CardComponent>().tempRotation = rotation;
            });
        }
    }
}