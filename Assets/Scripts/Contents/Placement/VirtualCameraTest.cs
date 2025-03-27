using Cinemachine;
using UnityEngine;

public class VirtualCameraTest : MonoBehaviour
{
    CinemachineBlendListCamera blendList;

    CinemachineVirtualCameraBase vCam1;
    CinemachineVirtualCameraBase vCam2;

    void Start()
    {
        blendList = this.GetComponent<CinemachineBlendListCamera>();
        blendList.m_Loop = false;

        //vCamObj1 = GameObject.Find("Virtual Camera");
        //vCamObj2 = GameObject.Find("PlacementCamera");
        //vCam1 = vCamObj1.GetComponent<CinemachineVirtualCameraBase>();
        //vCam2 = vCamObj2.GetComponent<CinemachineVirtualCameraBase>();

        blendList.m_Instructions[0].m_VirtualCamera = vCam1;
        blendList.m_Instructions[1].m_VirtualCamera = vCam1;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 0.3f;
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
