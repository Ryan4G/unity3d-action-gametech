using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeBlink : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Transform testTransform;

    public Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BlinkTo(transform, targetPos);
        }
    }

    void BlinkTo(Transform trans, Vector3 dstPoint)
    {
        var diff = dstPoint - trans.position;
        // len of vector
        var length = diff.magnitude;

        var dir = diff.normalized;

        var hit = default(RaycastHit);

        if (_rigidbody.SweepTest(dir, out hit, length))
        {
            var dstClosePoint = hit.collider.ClosestPointOnBounds(testTransform.position);

            var selfClosestPoint = _rigidbody.ClosestPointOnBounds(dstClosePoint);

            var closestPointDiff = selfClosestPoint - transform.position;

            transform.position = dstClosePoint - closestPointDiff;
        }
        else
        {
            trans.position = dstPoint;
        }
    }
}
