using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact;      //총알파편효과
    ParticleSystem bulletEffect;        //총알 파편 파티클 시스템
    AudioSource bulletAudio;            //총알 발사 사운드
    public Transform crosshair;         //크로스헤어 

    private void Start()
    {
        //총알 효과 파티클 시스템 컴포넌트 가져오기
        bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        //총알 효과 오디오소스 가져오기
        bulletAudio = bulletImpact.GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //크로스헤어 표시
        ARAVRInput.DrawCrosshair(crosshair);

        //사용자가 IndexTrigger 버튼 입력시
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            //총알 오디오 재생
            bulletAudio.Stop();
            bulletAudio.PlayOneShot(bulletAudio.clip);

            //레이를 쏘기위한 시작단계
            Ray ray = new Ray(ARAVRInput.RHandPosition ,ARAVRInput.RHandDirection);

            //충돌정보 저장
            RaycastHit hit;

            //플레이어 레이어
            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            //타워 레이어
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;

            //레이 쏘고 충돌정보가 hit 에 담긴다.     Player 와 Tower 제외
            if (Physics.Raycast(ray, out hit, 200, ~layerMask))
            {
                //총알 파편 이펙트 효과 
                bulletEffect.Stop();
                bulletEffect.Play();

                //레이가 부딫힌 노말방향 으로 총알 이펙트 방향 설정
                bulletImpact.forward = hit.normal;
                //부딫힌 위치를 설정
                bulletImpact.position = hit.point;
            }
        }
    }
}
