/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_TurnaroundTest.cs
 *   
*************************************************************/

using UnityEngine;

public class NoloVR_TurnaroundTest : MonoBehaviour {
    //Rotate around the Y axis
    Quaternion rotationAngle = new Quaternion(0, 1, 0, 0);

    int leftcontroller_PreFrame = -1;
    int rightcontroller_PreFrame = -1;
    
    //double clikc spacing frame count
    int spacingFrame = 20;
	
	void Update () {
        //Nolo SDK Reference Design
        //double-click any controller's menu button to achieve turn around.
        //just make gameboject("NoloManager")'s rotation everytime rotate 180°

        //leftcontroller double click menu button
        if (NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonUp(NoloButtonID.Menu))
        {
            if (Time.frameCount - leftcontroller_PreFrame <= spacingFrame)
            {
                Turnaround();
                leftcontroller_PreFrame = -1;
            }
            else
            {
                leftcontroller_PreFrame = Time.frameCount;
            }
        }
        //rightcontroller double click menu button
        if (NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonUp(NoloButtonID.Menu))
        {
            if (Time.frameCount - rightcontroller_PreFrame <= spacingFrame)
            {
                Turnaround();
                leftcontroller_PreFrame = -1;
            }
            else
            {
                rightcontroller_PreFrame = Time.frameCount;
            }
        }
    }

    //turn around method
    void Turnaround()
    {
        NoloVR_Manager.GetInstance().gameObject.transform.rotation = rotationAngle * NoloVR_Manager.GetInstance().gameObject.transform.rotation;
    }
}
