/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Playform.cs
 *   
*************************************************************/

public abstract class NoloVR_Playform
{

    //init method to prepare connect device
    public abstract bool InitDevice();
    //connect device to get data
    public abstract bool ConnectDevice(); 
    //disconnect device
    public abstract void DisconnectDevice();
    //reconnect device callback
    public abstract void ReconnectDeviceCallBack(); 
    //disconnect device callback
    public abstract void DisConnectedCallBack();
    //HapticPulse
    public abstract void TriggerHapticPulse(int deviceIndex,int intensity);

    protected static NoloError playformError = NoloError.UnKnow;

    public NoloError GetPlayformError()
    {
        return playformError;
    }

    protected NoloVR_Playform()
    {
        if (playformError == NoloError.UnKnow)
        {
            InitDevice();
        }
    }

    private static NoloVR_Playform instance;

    public static NoloVR_Playform InitPlayform()
    {
        if (instance == null)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            instance = new NoloVR_WinPlayform();
#elif UNITY_ANDROID
            instance = new NoloVR_AndroidPlayform();
#else
            Debug.Log("NoloVR  Don't support this playform");
#endif
        }
        return instance;
    }

    ~NoloVR_Playform()
    {
        if (instance != null)
        {
            DisconnectDevice();
            instance = null;
        }
    }

}