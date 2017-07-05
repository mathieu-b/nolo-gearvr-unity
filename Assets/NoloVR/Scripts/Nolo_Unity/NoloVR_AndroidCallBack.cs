/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_AndroidCallBack.cs
 *   
*************************************************************/

using UnityEngine;

public class NoloVR_AndroidCallBack : MonoBehaviour {
    //Android Callback
    public void usbDeviceState(string msg) {
        Debug.Log("NoloVR_AndroidCallBack:"+msg);
        switch (msg)
        {
            case "usb 设备断开":
                NoloVR_Playform.InitPlayform().DisConnectedCallBack();
                break;
            case "usb 设备接入":
                NoloVR_Playform.InitPlayform().ReconnectDeviceCallBack();
                break;
            default:
                break;
        }
    }
}
