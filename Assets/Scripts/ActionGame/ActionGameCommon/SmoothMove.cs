using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public enum MoveType
    {
        EaseOut,
        SmoothDamp,
        Quicken,
        EaseInOut
    }

    public MoveType moveType = MoveType.EaseOut;

    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        var vx = transform.position;

        if (moveType == MoveType.EaseOut)
        {
            vx = Vector3.Lerp(vx, targetPos, .05f);
        }
        else if (moveType == MoveType.SmoothDamp)
        {
            Vector3 v = Vector3.right;

            vx = Vector3.SmoothDamp(vx, targetPos, ref v, 2f);
        }
        else if (moveType == MoveType.Quicken)
        {
            var t = 0.234f;
            t = t * t * Mathf.Sign(targetPos.x);

            vx = new Vector3(vx.x + t, vx.y, vx.z);
        }
        else if (moveType == MoveType.EaseInOut)
        {
            var t = 0.234f;
            t = Mathf.Pow(t - 1f, 3) + 1f;
            t = t * t * Mathf.Sign(targetPos.x);

            vx = new Vector3(vx.x + t, vx.y, vx.z);
        }

        transform.position = vx;

        if (Mathf.Abs(transform.position.x) >= Mathf.Abs(targetPos.x))
        {
            transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            targetPos = new Vector3(-targetPos.x, targetPos.y, targetPos.z);
        }
    }
}
