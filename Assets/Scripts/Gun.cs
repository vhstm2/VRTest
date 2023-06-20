using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact;      //�Ѿ�����ȿ��
    ParticleSystem bulletEffect;        //�Ѿ� ���� ��ƼŬ �ý���
    AudioSource bulletAudio;            //�Ѿ� �߻� ����
    public Transform crosshair;         //ũ�ν���� 

    private void Start()
    {
        //�Ѿ� ȿ�� ��ƼŬ �ý��� ������Ʈ ��������
        bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        //�Ѿ� ȿ�� ������ҽ� ��������
        bulletAudio = bulletImpact.GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //ũ�ν���� ǥ��
        ARAVRInput.DrawCrosshair(crosshair);

        //����ڰ� IndexTrigger ��ư �Է½�
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            //�Ѿ� ����� ���
            bulletAudio.Stop();
            bulletAudio.PlayOneShot(bulletAudio.clip);

            //���̸� ������� ���۴ܰ�
            Ray ray = new Ray(ARAVRInput.RHandPosition ,ARAVRInput.RHandDirection);

            //�浹���� ����
            RaycastHit hit;

            //�÷��̾� ���̾�
            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            //Ÿ�� ���̾�
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;

            //���� ��� �浹������ hit �� ����.     Player �� Tower ����
            if (Physics.Raycast(ray, out hit, 200, ~layerMask))
            {
                //�Ѿ� ���� ����Ʈ ȿ�� 
                bulletEffect.Stop();
                bulletEffect.Play();

                //���̰� �΋H�� �븻���� ���� �Ѿ� ����Ʈ ���� ����
                bulletImpact.forward = hit.normal;
                //�΋H�� ��ġ�� ����
                bulletImpact.position = hit.point;
            }
        }
    }
}
