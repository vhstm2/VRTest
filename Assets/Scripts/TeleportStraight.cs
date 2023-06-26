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

            //�ڷ���Ʈ UI�� Ȱ��ȭ ���̳�?
            if(teleportCircleUI.gameObject.activeSelf)
            {
                //�ɸ��� ��Ʈ�ѷ� ��Ȱ��
                GetComponent<CharacterController>().enabled = false;
                //Player �� ��ġ�� �ڷ���Ʈ UI�� ��ġ + Vector(0,1,0)�� ����
                if(t == teleport.Terrain)
                    transform.position = teleportCircleUI.position  + Vector3.up;
                if(t == teleport.Tower)
                    transform.localPosition = Vector3.zero;

                //�ɸ��� ��Ʈ�ѷ� Ȱ��ȭ
                GetComponent<CharacterController>().enabled = true;
            }
            
            //�ڸ���Ʈ UI ��Ȱ��
            teleportCircleUI.gameObject.SetActive(false);
        }

        else if(ARAVRInput.Get(ARAVRInput.Button.One , ARAVRInput.Controller.LTouch))
        {
            //�ڸ���Ʈ UI �׸���

            Ray ray = new Ray(ARAVRInput.LHandPosition , ARAVRInput.LHandDirection);
            RaycastHit hit;

            int layer =  LayerMask.NameToLayer("Terrain");
            int towerLayer =  LayerMask.NameToLayer("Tower");

            int lastLayer = 1 <<layer | 1<< towerLayer;
            //ray�� Terrain �� ����
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


                //�΋H�� ������ �ڷ���Ʈ UI ǥ��
                //������ ���̽�� ��������
                lr.SetPosition (0 , ray.origin);
                //���� ���̰� Terrain ��򰡿� �΋H�� ���� ��ġ
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
