using UnityEngine;

public class Arrow : MonoBehaviour {
    public void DrawArrow(Vector3 drawStartPos, Vector3 drawEndPos)
    {
        LineRenderer arrowLine = GetComponent<LineRenderer>();
        float arrowheadSize = 1;
        // drawEndPos.y = drawStartPos.y;
        float z = drawStartPos.z;
        Vector3 startPos = new Vector3(drawStartPos.x, drawStartPos.y , drawStartPos.z-1);
        Vector3 endPos = new Vector3(drawEndPos.x, drawEndPos.y , drawEndPos.z-1);
        float percentSize = (float)(arrowheadSize / Vector3.Distance(startPos, endPos));
        arrowLine.positionCount = 4;
        arrowLine.SetPosition(0, startPos);
        arrowLine.SetPosition(1, Vector3.Lerp(startPos, endPos, 0.999f - percentSize));
        arrowLine.SetPosition(2, Vector3.Lerp(startPos, endPos, 1 - percentSize));
        arrowLine.SetPosition(3, endPos);
        arrowLine.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f),
            new Keyframe(0.999f - percentSize, 0.4f),
            new Keyframe(1 - percentSize, 1f),
            new Keyframe(1 - percentSize, 1f),
            new Keyframe(1, 0f)
        );
    }

}