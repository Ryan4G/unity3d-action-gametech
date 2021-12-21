using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundInspector : MonoBehaviour
{
    public Transform[] groundPoints;

    public LayerMask layer;

    public bool isGround = false;

    private float gravity = -9.8f;

    private float _vertSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsOnGroundUpdate(groundPoints, layer, 0.5f, out isGround, out _);

        ControlUpdate();

        if (!isGround)
        {
            Debug.Log("Not on the ground");
        }
    }

    void IsOnGroundUpdate(Transform[] groundPoints, LayerMask layerMask, float length, out bool isOnGround, out RaycastHit cacheRraycastHit)
    {
        isOnGround = false;

        cacheRraycastHit = default(RaycastHit);

        for (int i = 0, iMax = groundPoints.Length; i < iMax; i++)
        {
            var groundPoint = groundPoints[i];
            var hit = default(RaycastHit);
            var isHit = Physics.Raycast(new Ray(groundPoint.position, -groundPoint.up), out hit, length, layerMask);

            if (isHit)
            {
                isOnGround = isHit;
                cacheRraycastHit = hit;
                break;
            }
        }
    }

    void ControlUpdate()
    {
        Vector3 movement = Vector3.zero;

        if (isGround)
        {
            var jump = Input.GetButtonDown("Jump");

            if (jump)
            {
                _vertSpeed = 30.0f;
            }
            else
            {
                _vertSpeed = -1.5f;
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < -10.0f)
            {
                _vertSpeed = -10.0f;
            }
        }

        movement.y = _vertSpeed;

        movement *= Time.deltaTime;

        transform.Translate(movement);
    }
}
