/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_AndroidPlayform.cs
 *   
*************************************************************/

using UnityEngine;
using System;

public class NoloVR_AndroidPlayform : NoloVR_Playform
{
    AndroidJavaClass unityPlayer;
    AndroidJavaObject currentActivity;
    AndroidJavaObject context;
    AndroidJavaObject jc, jo;

    public override bool InitDevice()
    {
        try
        {
            //init serves
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            jc = new AndroidJavaClass("com.watchdata.usbhostconn.UsbCustomTransfer");
            jo = jc.CallStatic<AndroidJavaObject>("getInstance", context);
            jo.Call("usb_init");
            playformError = NoloError.NoConnect;
        }
        catch (Exception e)
        {
            Debug.Log("NoloVR_AndroidPlayform InitDevice:error"+e.Message);
            playformError = NoloError.ConnectFail;
            return false;
        }
        ConnectDevice();
        return true;
    }

    //Connect Device Method
    public override bool ConnectDevice()
    {
        try
        {
            int result = jo.Call<int>("usb_conn");
            if (result == 1)
            {
                playformError = NoloError.None;
            }
        }
        catch (Exception e)
        {
            Debug.Log("NoloVR_AndroidPlayform ConnectDevice:error"+e.Message);
            playformError = NoloError.ConnectFail;
            return false;
        }
        return true;
    }

    //Close connected
    public override void DisconnectDevice()
    {
        //jo.Call("usb_finish");
        unityPlayer = null;
        currentActivity = null;
        context = null;
        jo = null;
        jc = null;
        playformError = NoloError.DisConnect;
    }
     
    //Reconnect Device CallBack
    public override void ReconnectDeviceCallBack()
    {
        //do nothing
    }

    //Disconnect callback
    public override void DisConnectedCallBack()
    {
        playformError = NoloError.DisConnect;
    }

    // Pre HapticPulse message
    int preDeviceIndex = -1;
    byte preDeviceIndexIntensity;
    // HapticPulse
    // DeviceIndex: device leftcontroller or rightcontroller
    // Intensity: range 0~100 
    public override void TriggerHapticPulse(int deviceIndex, int intensity)
    {
        byte[] coder = new byte[4];
        coder[0] = 0xAA;
        coder[1] = 0x66;
        if (intensity < 0) intensity = 0;
        if (intensity > 100) intensity = 100;
        switch (deviceIndex)
        {
            case (int)NoloDeviceType.LeftController:
                if (preDeviceIndex != deviceIndex)
                {
                    coder[3] = preDeviceIndexIntensity;
                }
                coder[2] = (byte)intensity;
                break;
            case (int)NoloDeviceType.RightController:
                if (preDeviceIndex != deviceIndex)
                {
                    coder[2] = preDeviceIndexIntensity;
                }
                coder[3] = (byte)intensity;
                break;
            default:
                break;
        }
        preDeviceIndex = deviceIndex;
        preDeviceIndexIntensity = (byte)intensity;
        ConnectToSendData(coder);

    } 
    //Connect To SendData
    //Msg：you send message to device
    private void ConnectToSendData(byte[] msg)
    {
        try
        {
            jo.Call("usb_sendData", msg);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Android playform sendData_usb error:" + e);
        }
    }
   
}
