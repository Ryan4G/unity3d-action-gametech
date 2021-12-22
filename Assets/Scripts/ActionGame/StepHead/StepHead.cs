using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepHead : MonoBehaviour
{
    Bounds mCacheBounds;

    public LayerMask layerMask;

    public Collider[] mCacheOverlayBoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 fixCenterOffsetPoint = new Vector3(0, 1.5f, 0);

        float gradient = 1.0f;

        float radius = 1.0f;

        float areaBottomExtern = 1.0f;

        var cacheColor = Gizmos.color;
        var yAxis = -Physics.gravity.normalized;
        var center = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint) - yAxis * gradient * 0.5f;

        var diameter = radius * 2f;
        mCacheBounds = new Bounds(center, new Vector3(diameter, gradient, diameter));

    }

    // Update is called once per frame
    void Update()
    {
        var center = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center);

        var hitCount = Physics. (center, mCacheBounds.extents, mCacheOverlayBoxCollider, transform.rotation, layerMask);

        for (int i = 0, iMax = hitCount; i < iMax; i++)
        {
            var item = mCacheOverlayBoxCollider[i];
            if (item.transform == transform)
            {
                continue;
            }

            var rigidbody = item.GetComponet<Rigidbody>();
            if (rigidbody != null)
            {
                ConeFix(rigidbody);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 fixCenterOffsetPoint = new Vector3(0, 1.5f, 0);

        float gradient = 1.0f;

        float radius = 1.0f;

        float areaBottomExtern = 1.0f;

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

        //Debug.Log($"{bounds.center.x},{bounds.center.y},{bounds.center.z}");
        Gizmos.color = cacheColor;
    }

    bool IsConeContain(Vector3 point)
    {
        Vector3 fixCenterOffsetPoint = new Vector3(0, 1.5f, 0);

        float gradient = 1.0f;

        float radius = 1.0f;

        var yAxis = -Physics.gravity.normalized;
        var center = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint) - yAxis * gradient * 0.5f;

        var diameter = radius * 2f;
        //var mCacheBounds = new Bounds(center, new Vector3(diameter, gradient, diameter));

        var upAxis = -Physics.gravity.normalized;
        var topCenter = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center) + upAxis * gradient * 0.5f;

        var bottomCenter = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center) - upAxis * gradient * 0.5f;
        var dir = (bottomCenter - topCenter).normalized;

        var a = Vector3.Project(topCenter, dir);
        var b = Vector3.Project(bottomCenter, dir);
        var c = Vector3.Project(point, dir);

        var rate = Mathf.Clamp01(Vector3.Distance(a, c) / Vector3.Distance(a, b));

        var currentRadius = Mathf.Lerp(0, radius, rate);

        return Vector3.Distance(bottomCenter + (c - b), point) < currentRadius;
    }

    bool ConeFix(Rigidbody target)
    {
        float areaBottomExtern = 1.0f;

        Vector3 speed = Vector3.one;

        Vector3 fixCenterOffsetPoint = new Vector3(0, 1.5f, 0);

        float gradient = 1.0f;

        float radius = 1.0f;

        float error = 1.0f;

        var center = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint) - yAxis * gradient * 0.5f;

        var diameter = radius * 2f;
        var mCacheBounds = new Bounds(center, new Vector3(diameter, gradient, diameter));

        var topCenterPoint = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint);

        var diff = (target.position - topCenterPoint).normalized;
        var yAxis = -Physics.gravity.normalized;
        var offset = Vector3.ProjectOnPlane(diff, yAxis);
        var bottomCenterPoint = topCenterPoint - yAxis * gradient;

        if (!IsConeContain(target.position))
        {
            return false;
        }

        if (Vector3.Distance(offset, Vector3.zero) < error)
        {
            offset = Vector3.ProjectOnPlane(Random.onUnitSphere, yAxis).normalized;
        }

        var expectBottomPoint = bottomCenterPoint + offset * radius;
        var a = topCenterPoint;

        var b = expectBottomPoint - yAxis * areaBottomExtern;
        var c = target.transform.position;
        var finalSpeed = speed.y;
        var ab_dist = Vector3.Distance(a, b);
        var bc_dist = Vector3.Distance(b, c);

        if (ab_dist > bc_dist)
        {
            var rate = (bc_dist / ab_dist);

            finalSpeed = Mathf.Lerp(speed.x, speed.y, rate);
        }

        var coneFixValue = (expectBottomPoint - topCenterPoint).normalized * finalSpeed * Time.fixedDeltaTime;

        target.velocity += coneFixValue;
        return true;
    }
}
