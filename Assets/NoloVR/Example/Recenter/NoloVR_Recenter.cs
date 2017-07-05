/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Recenter.cs
 *   
*************************************************************/

using UnityEngine;
using UnityEngine.VR;

public class NoloVR_Recenter : MonoBehaviour {
    int leftcontroller_PreFrame = -1;
    int rightcontroller_PreFrame = -1;

    //Double clikc spacing frame count
    int spacingFrame = 20;

	void Update () {
        //every VR SDK should have the method to reset the camera rotation
        //such as Oculus InputTracking.Recenter();
        //Depending on the SDK you are using, modify the method of the call

        //leftcontroller double click system button
        if (NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonUp(NoloButtonID.System))
        {
            if (Time.frameCount - leftcontroller_PreFrame <= spacingFrame)
            {
                //To do camera recenter method
                //Modify here
                InputTracking.Recenter();
                leftcontroller_PreFrame = -1;
            }
            else
            {
                leftcontroller_PreFrame = Time.frameCount;
            }
        }
        //rightcontroller double click system button
        if (NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonUp(NoloButtonID.System))
        {
            if (Time.frameCount - rightcontroller_PreFrame <= spacingFrame)
            {
                //To do camera recenter method
                //Modify here
                InputTracking.Recenter();
                rightcontroller_PreFrame = -1;
            }
            else
            {
                rightcontroller_PreFrame = Time.frameCount;
            }
        }
    }
}
