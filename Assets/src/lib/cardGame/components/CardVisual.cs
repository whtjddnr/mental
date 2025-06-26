using DG.Tweening;
using UnityEngine;

public class CardVisual: MonoBehaviour 
{
    [Header("Card")]
    private Vector3 rotationDelta;
    Vector3 movementDelta;
    public CardComponent parentCard;

    [Header("References")]
    public Transform visualShadow;
    private float shadowOffset = 0.1f;
    private Vector3 shadowDistance;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationAmount = 2;
    [SerializeField] private float rotationSpeed = 2;
    void Start() {
        parentCard = this.GetComponent<CardComponent>();
        shadowDistance = visualShadow.localPosition;
    }
    void OnMouseDown() {
        if(parentCard.card.location == CardLocation.onHand) Shadow();
    }
    void OnMouseUp() {
        if(parentCard.card.location == CardLocation.onHand) ReturnPosShadow();
    }
    void Update() {
        if (parentCard == null) return;
        if(parentCard.isDragging) {
            SmoothFollow();
            FollowRotation();
        }
    }
    private void SmoothFollow() {
        this.transform.position = Vector3.Lerp(
            this.transform.position,
            new Vector3(CardGameEngine.Utill.GetMousePos().x, CardGameEngine.Utill.GetMousePos().y, this.transform.position.z),
            Time.deltaTime * 20
        );
    }
    private void FollowRotation()
    {
        Vector3 movement = (transform.position - CardGameEngine.Utill.GetMousePos());
        movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
        Vector3 movementRotation = (parentCard.isDragging ? movementDelta : movement) * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -60, 60));
    }
    private void Shadow() {
        visualShadow.localPosition += (-Vector3.up * shadowOffset);
    }
    public void ReturnPosShadow() {
        visualShadow.localPosition = shadowDistance;
    }
}