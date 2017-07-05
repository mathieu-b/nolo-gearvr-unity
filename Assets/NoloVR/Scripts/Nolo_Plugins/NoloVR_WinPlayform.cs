/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_WinPlayform.cs
 *   
*************************************************************/

using UnityEngine;
using System;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
public class NoloVR_WinPlayform : NoloVR_Playform
{
    DisConnectedCallBack disconn;
    ConnectedCallBack conn;

    public override bool InitDevice()
    {
        try
        {
            disconn = new DisConnectedCallBack(DisConnectedCallBack);
            conn = new ConnectedCallBack(ReconnectDeviceCallBack);
            NoloVR_Plugins.API_1_0_0.disConnenct_FunCallBack(disconn);
            NoloVR_Plugins.API_1_0_0.connectSuccess_FunCallBack(conn);
            playformError = NoloError.NoConnect;
        }
        catch (Exception)
        {
            Debug.Log("NoloVR_WinPlayform InitDevice:error");
            return false;
        }
        ConnectDevice();
        return true;
    }

    public override bool ConnectDevice()
    {
        try
        {
            NoloVR_Plugins.API_1_0_0.open_Nolo_ZeroMQ();
        }
        catch (Exception)
        {
            Debug.Log("NoloVR_WinPlayform ConnectDevice:error");
            playformError = NoloError.ConnectFail;
            return false;
        }
        playformError = NoloError.None;
        return true;

    }

    public override void DisconnectDevice()
    {
        playformError = NoloError.DisConnect;
        NoloVR_Plugins.API_1_0_0.close_Nolo_ZeroMQ();
    }

    public override void DisConnectedCallBack()
    {
        Debug.Log("disconnect nolo device");
        try
        {
            NoloVR_Plugins.API_1_0_0.close_Nolo_ZeroMQ();
            playformError = NoloError.NoConnect;
        }
        catch (Exception e)
        {
            Debug.Log("DisConnectedCallBack:"+e.Message);
            throw;
        }
    } 

    public override void ReconnectDeviceCallBack()
    {
        Debug.Log("reconnect nolo device success");
        try
        {
            playformError = NoloError.None;

        }
        catch (Exception e)
        {
            Debug.Log("ReconnectDevice:" + e.Message);
            throw;
        }

    }


    // Pre HapticPulse message
    int preDeviceIndex = -1;
    byte preDeviceIndexIntensity;
    public override void TriggerHapticPulse(int deviceIndex, int intensity)
    {
        NoloVR_Plugins.API_1_0_0.Nolovr_TriggerHapticPulse(deviceIndex, intensity);
    }
}
#endif