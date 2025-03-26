using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineVirtualCameraBase;

public class VirtualCameraTest : MonoBehaviour
{
    CinemachineBlendListCamera blendList;

    GameObject vCamObj1;
    GameObject vCamObj2;
    CinemachineVirtualCameraBase vCam1;
    CinemachineVirtualCameraBase vCam2;

    void Start()
    {
        blendList = this.GetComponent<CinemachineBlendListCamera>();
        blendList.m_Loop = false;
        
        vCamObj1 = GameObject.Find("Virtual Camera");
        vCamObj2 = GameObject.Find("PlacementCamera");
        vCam1 = vCamObj1.GetComponent<CinemachineVirtualCameraBase>();
        vCam2 = vCamObj2.GetComponent<CinemachineVirtualCameraBase>();

        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam1;
    }

    public void buttonLeft()
    {
        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam2;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 0.3f;

    }

    public void buttonRight()
    {
        blendList.m_Instructions[0].m_VirtualCamera = vCam2;
        blendList.m_Instructions[1].m_VirtualCamera = vCam1;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 0.3f;
    }
}
