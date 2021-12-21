using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossAI : MonoBehaviour
{
    public float speed = 0.5f;

    Vector3 direction;

    public Transform enemyTransform;

    enum EnemeyDirection
    {
        Left,
        Right
    };

    // Start is called before the first frame update
    void Start()
    {
        //Vector3 pos = Vector3.Cross(Vector3.forward, Vector3.right);
        //Debug.Log($"{pos.x},{pos.y},{pos.z}");

        direction = transform.forward;

        var enemyDirection = GetEnemyDirection(transform, enemyTransform);
        Debug.Log($"Enemy on the {enemyDirection.ToString()} side.");
    }

    // Update is called once per frame
    void Update()
    {
        MoveUpdate();
    }

    private void MoveUpdate()
    {
        var hasWall = Physics.Raycast(transform.position, direction, 1f);
        if (hasWall)
        { 
            //Debug.Log($"hasWall");
            var byPassDirection = Vector3.Cross(direction, Physics.gravity.normalized);
            direction = byPassDirection;

            //float angle = Vector3.Angle(transform.forward, byPassDirection);

            //transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        transform.Translate(direction * speed * Time.deltaTime);
    }

    EnemeyDirection GetEnemyDirection(Transform self, Transform enmey)
    {
        var cross = Vector3.Cross(self.forward, enmey.position - self.position);

        if (cross.y > 0)
        {
            return EnemeyDirection.Right;
        }

        return EnemeyDirection.Left;
    }
}
