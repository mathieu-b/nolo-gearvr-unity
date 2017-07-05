/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_TrackedDevice.cs
 *   
*************************************************************/

using UnityEngine;

public class NoloVR_TrackedDevice : MonoBehaviour {

    public NoloDeviceType deviceType;
    private GameObject vrCamera;
    private Vector3 centralPointVector = new Vector3(0, 0.09f, 0.06f);

    void Start()
    {
        vrCamera = NoloVR_Manager.GetInstance().VRCamera;
    }
	void Update () {
        UpdatePose();
    }

    void UpdatePose()
    {
        var pose = NoloVR_Controller.GetDevice(deviceType).GetPose();
        if (deviceType == NoloDeviceType.Hmd)
        {
            if (vrCamera == null)
            {
                Debug.LogError("Not find your vr camera");
                transform.localPosition = pose.pos;
                return;
            }
            //pose.rot.eulerAngles.y;1
            //vrCamera.transform.rotation.eulerAngles.y;2
            //transform.localRotation.eulerAngles.y;3
            //3 = 2>>1;

            //transform.localRotation = Quaternion.Euler(0, HMDRotationFuse(pose.rot.eulerAngles.y, vrCamera.transform.rotation.eulerAngles.y), 0);
            transform.localPosition = pose.pos - vrCamera.transform.localPosition - vrCamera.transform.localRotation * centralPointVector;

        }
        else
        {
            transform.localPosition = pose.pos;
            transform.localRotation = pose.rot;
        }
    }
    float HMDRotationFuse(float nolo,float phone)
    {
        float difference = nolo - phone;
        return transform.localRotation.eulerAngles.y - difference;
    }
}
