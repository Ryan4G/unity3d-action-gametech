using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCube : MonoBehaviour
{
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertial = Input.GetAxis("Vertical");

        var upAxis = Physics.gravity.normalized;
        var forwardAxis = Vector3.ProjectOnPlane(Camera.main.transform.forward, upAxis);
        var rightAxis = Vector3.ProjectOnPlane(Camera.main.transform.right, upAxis);

        ExecuteMove((forwardAxis * vertial + rightAxis * horizontal).normalized * speed * Time.deltaTime);
    }

    private void ExecuteMove(Vector3 v)
    {
        transform.Translate(v);
    }
}
