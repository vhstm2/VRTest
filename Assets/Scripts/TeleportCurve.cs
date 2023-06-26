using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{

    //텔레포트에 사용할 UI
    public Transform teleportCircleUI;

    //라인렌더러
    LineRenderer lr;

    //초기값
    Vector3 originScale = Vector3.one * 0.02f;

    //커브의 부드러움 정도
    public int lineSmooth  = 40;
    //커브의 길이
    public float curveLength = 50;

    //중력값
    public float gravity = -60;

    public float simulateTime = 0.02f;

    List<Vector3> lines = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.0f;
        lr.endWidth = 0.2f;
    }

    // Update is called once per frame
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
                transform.position = teleportCircleUI.position  + Vector3.up;
                //케릭터 컨트롤러 활성화
                GetComponent<CharacterController>().enabled = true;
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
        else if(ARAVRInput.Get(ARAVRInput.Button.One , ARAVRInput.Controller.LTouch))
        {
            //주어진 길이크기의 커브 
            MakeLines();
        }           
    }


    private void MakeLines()
    {
        lines.RemoveRange(0 , lines.Count);     //리스트에 담긴 위치정보들을 비워준다

        Vector3 dir = ARAVRInput.LHandDirection * curveLength;  //선이 진행될 방향 설정

        Vector3 pos = ARAVRInput.LHandPosition; //선이 그려질 초기위치

        lines.Add(pos);   

        for (int i = 0; i < lineSmooth; i++)
        {
            Vector3 lastPos = pos;              //현재위치
            dir.y += gravity * simulateTime;    //등속운동으로 다음위치 계산
            pos += dir * simulateTime;

            if(CheckHitRay(lastPos , ref pos))
            {
                lines.Add(pos);
                break;
            }
            else
            {
                teleportCircleUI.gameObject.SetActive(false);
            }

            lines.Add(pos);                     //구한위치 등록
        }

        //그려질 라인렌더러의 크기를 할당
        lr.positionCount = lines.Count;
        //라인렌더러에 구해진 점의 정보를 저장
        lr.SetPositions( lines.ToArray());

    }

    private bool CheckHitRay(Vector3 lastPos , ref Vector3 pos)
    {
        //앞점 lastPos 에서 다음점 pos 로 향하는 벡터 계산
        Vector3 rayDir = pos - lastPos;
        Ray ray = new Ray(lastPos , rayDir);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray , out hitInfo , rayDir.magnitude))
        {
            pos = hitInfo.point;

            int layer =  LayerMask.NameToLayer("Terrain");
            if(hitInfo.collider.gameObject.layer == layer)
            //if(hitInfo.collider.CompareTag("Terrain"))
            {
                Debug.Log(hitInfo.collider.name);
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.transform.position = pos;
                teleportCircleUI.forward = hitInfo.normal;
                float distance = (pos - ARAVRInput.LHandPosition).magnitude;
                teleportCircleUI.localScale = originScale * Mathf.Max( 1 , distance);
            }
            return true;
        }

        return false;
    }


}
