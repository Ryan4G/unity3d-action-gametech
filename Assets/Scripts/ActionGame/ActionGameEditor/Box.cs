using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("OnCollisionEnter");

        var player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            PushBox(player.transform, transform);
        }
    }

    void PushBox(Transform playerTransform, Transform boxTransform)
    {
        const float ERROR = .5f;

        Debug.Log($"{playerTransform.forward.x},{playerTransform.forward.y},{playerTransform.forward.z}");

        if (Vector3.Dot(playerTransform.forward, -boxTransform.forward) > ERROR)
        {
            Debug.Log("Pushed box forward");
        }
        else if (Vector3.Dot(playerTransform.forward, boxTransform.forward) > ERROR)
        {
            Debug.Log("Pushed box backward");
        }
        else if (Vector3.Dot(playerTransform.forward, -boxTransform.right) > ERROR)
        {
            Debug.Log("Pushed box left");
        }
        else if (Vector3.Dot(playerTransform.forward, boxTransform.right) > ERROR)
        {
            Debug.Log("Pushed box right");
        }
    }
}
