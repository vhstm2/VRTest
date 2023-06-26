using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;

    LineRenderer lr;
    
    public enum teleport
    {
        Terrain,Tower,end
    }
    public teleport t = teleport.end;
    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
    }
    

    Vector3 originScale = Vector3.one * 0.02f;
    void Update()
    {
        if(ARAVRInput.GetDown(ARAVRInput.Button.One , ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true;
        }
        else if(ARAVRInput.GetUp(ARAVRInput.Button.One , ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false;

            //텔레포트 UI가 활성화 중이냐?
            if(teleportCircleUI.gameObject.activeSelf)
            {
                //케릭터 컨트롤러 비활성
                GetComponent<CharacterController>().enabled = false;
                //Player 의 위치를 텔레포트 UI의 위치 + Vector(0,1,0)로 변경
                if(t == teleport.Terrain)
                    transform.position = teleportCircleUI.position  + Vector3.up;
                if(t == teleport.Tower)
                    transform.localPosition = Vector3.zero;

                //케릭터 컨트롤러 활성화
                GetComponent<CharacterController>().enabled = true;
            }
            
            //텔리포트 UI 비활성
            teleportCircleUI.gameObject.SetActive(false);
        }

        else if(ARAVRInput.Get(ARAVRInput.Button.One , ARAVRInput.Controller.LTouch))
        {
            //텔리포트 UI 그리기

            Ray ray = new Ray(ARAVRInput.LHandPosition , ARAVRInput.LHandDirection);
            RaycastHit hit;

            int layer =  LayerMask.NameToLayer("Terrain");
            int towerLayer =  LayerMask.NameToLayer("Tower");

            int lastLayer = 1 <<layer | 1<< towerLayer;
            //ray가 Terrain 만 검출
            if(Physics.Raycast(ray , out hit , 200,lastLayer ))
            {
                // if( hit.collider.CompareTag("Terrain") )
                // {
                //     t = teleport.Terrain;
                // }
                // if( hit.collider.CompareTag("Tower"))
                // {
                //     t = teleport.Tower;
                // }
                
                if( hit.collider.gameObject.layer == layer )
                {
                    t = teleport.Terrain;
                }
                if( hit.collider.gameObject.layer == towerLayer)
                {
                    t = teleport.Tower;
                }


                //부딫힌 지점에 텔레포트 UI 표시
                //시작점 레이쏘는 시작지점
                lr.SetPosition (0 , ray.origin);
                //끝점 레이가 Terrain 어딘가에 부딫힌 지점 위치
                lr.SetPosition(1 , hit.point);
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.transform.position = hit.point;
                teleportCircleUI.forward = hit.normal;
                teleportCircleUI.localScale = originScale * Mathf.Max(1 , hit.distance);

            }
            else
            {
                lr.SetPosition(0 , ray.origin);
                lr.SetPosition(1 , ray.origin + ARAVRInput.LHandDirection * 200);

                teleportCircleUI.gameObject.SetActive(false);
            }

            
        }
    }
}
