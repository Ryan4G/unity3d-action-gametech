using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubeDash : MonoBehaviour
{
    public Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DashTo(transform, targetPos));
        }
    }

    IEnumerator DashTo(Transform trans, Vector3 dstPoint)
    {
        var selfPosition = trans.position;
        //dstPoint = GetGroundPoint(dstPoint);

        var waitForFixedUpdate = new WaitForFixedUpdate();
        var beginTime = Time.fixedTime;

        for (var duration = 0.15f; Time.fixedTime - beginTime < duration;)
        {
            var t = (Time.fixedTime - beginTime) / duration;

            t = t * t;

            trans.position = Vector3.Lerp(selfPosition, dstPoint, t);
            yield return waitForFixedUpdate;
        }
    }
}
