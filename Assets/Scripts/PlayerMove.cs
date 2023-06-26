using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5;

    CharacterController cc;

    //중력가속도 크기
    public float gravity = -9.8f;

    //수직속도
    float yVelocity = 0;

    //점프 크기
    public float jompPower = 5;

    private bool jumping = false;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);
        yVelocity += gravity * Time.deltaTime;

        //Player가 바닥에 있느냐 
        if(cc.isGrounded) 
        {
            yVelocity = 0;
            jumping = false;
        }

        if (ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch) && !jumping)
        {
            yVelocity = jompPower;
            jumping = true;
        }

      //  print($"yVelocity {yVelocity}");
        dir.y = yVelocity;


        cc.Move(dir * speed * Time.deltaTime);
    }
}
