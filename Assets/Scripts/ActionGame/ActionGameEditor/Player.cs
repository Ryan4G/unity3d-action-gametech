using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Transform m_transform;

    CharacterController m_ch;

    float m_moveSpeed = 3.0f;

    float m_rotateSpeed = 30.0f;

    float m_gravity = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;

        m_ch = this.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    void Control()
    {
        // get mouse moved distance
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        //m_camRot.x -= rv;
        //m_camRot.y += rh;
        //m_camTransform.eulerAngles = m_camRot;

        //Vector3 camrot = m_camTransform.eulerAngles;
        //camrot.x = 0;
        //camrot.z = 0;
        //m_transform.eulerAngles = camrot;

        Vector3 motion = Vector3.zero;
        motion.x = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime;

        m_ch.Move(m_transform.TransformDirection(motion));

        float axis = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            axis = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            axis = 1;
        }

        Vector3 angle = transform.localEulerAngles;
        angle.y += axis * Time.deltaTime * m_rotateSpeed;
        transform.localEulerAngles = angle;
    }
}
