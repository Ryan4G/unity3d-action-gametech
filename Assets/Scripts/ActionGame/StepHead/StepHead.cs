using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        var cacheColor = Gizmos.color;
        var yAxis = -Physics.gravity.normalized;
        var center = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint) - yAxis * gradient * 0.5f;

        var diameter = radius * 2f;
        var bounds = new Bounds(center, new Vector3(diameter, gradient, diameter));
        var topCenter = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint);

        var bottomCenter = topCenter - yAxis * gradient;
        var forward = Quaternion.FromToRotation(Vector3.up, yAxis) * Vector3.forward;

        var lastPoint = (Vector3?)null;
        for (int delta = 30, i = -delta; i < 360; i += delta)
        {
            var quat = Quaternion.AngleAxis(i, -yAxis);
            var bottomPoint = bottomCenter + quat * forward * radius;
            Gizmos.DrawLine(bottomCenter, bottomPoint);
            Gizmos.DrawLine(bottomPoint, topCenter);

            if (lastPoint != null)
            {
                Gizmos.DrawLine(lastPoint.Value, bottomPoint);
            }

            lastPoint = bottomPoint;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(topCenter, bottomCenter - yAxis * areaBottomExtern);
        Gizmos.color = cacheColor;
    }
}
