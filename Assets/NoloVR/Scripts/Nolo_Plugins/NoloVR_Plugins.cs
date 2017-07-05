/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Plugins.cs
 *   
*************************************************************/

using System.Runtime.InteropServices;

public enum NoloDeviceType
{
    Hmd = 0,
    LeftController,
    RightController,
    BaseStationOne,
}
public enum NoloButtonID
{
    TouchPad,
    Trigger,
    Menu,
    System,
    Grip
}
public enum NoloTouchID
{
    TouchPad
}
public enum NoloError
{
    None = 0,             //没有错误
    ConnectFail,          //连接失败
    NoConnect,            //未连接
    DisConnect,           //断开连接
    UnKnow,               //未知错误
}
public enum NoloTrackingStatus
{
    NotConnect = 0,
    Normal,
    OutofRange
}

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate void DisConnectedCallBack();
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate void ConnectedCallBack();

public class NoloVR_Plugins
{
    //sdk version
    private const int nolo_SDK_Version = 2;
    //hmd version dk2 or cv1
    private static int hmdVersion = 0;
    //total number
    public const int trackedDeviceNumber = 4;

    #region method
    public static Nolo_Transform GetPose(int deviceIndex)
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        if (hmdVersion >= API_1_0_0.apiVersion)
        {
            return new Nolo_Transform(API_1_0_0.GetPoseByDeviceType(deviceIndex));
        }
        else
        {
            return new Nolo_Transform();
        }
    }
    public static Nolo_ControllerStates GetControllerStates(int deviceIndex)
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        if (hmdVersion >= API_1_0_0.apiVersion)
        {
            return API_1_0_0.GetControllerStatesByDeviceTyp(deviceIndex);
        }
        else
        {
            return new Nolo_ControllerStates();
        }
    }
    public static int GetElectricity(int deviceIndex)
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        if (hmdVersion >= API_1_0_0.apiVersion)
        {
            return API_1_0_0.GetElectricityByDeviceType(deviceIndex);
        }
        else
        {
            return 0;
        }
    }
    public static int GetHmdVersion()
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        return hmdVersion;
    }
    public static int GetTrackingStatus(int type)
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        if (hmdVersion >=API_2_0_0.apiVersion)
        {
            return API_2_0_0.GetDeviceTrackingStatus(type);
        }
        return 0;
    }
    public static Nolo_Vector3 GetHmdInitPosition()
    {
        if (hmdVersion <= 0)
        {
            hmdVersion = API_1_0_0.GetVersionByDeviceType(0);
        }
        if (hmdVersion >= API_2_0_0.apiVersion)
        {
            return API_2_0_0.GetHmdInitPosition();
        }
        return new Nolo_Vector3();
    }
    #endregion

    #region Struct
    [StructLayout(LayoutKind.Sequential)]
    public struct Nolo_Vector2
    {
        public float x;
        public float y;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Nolo_Vector3
    {
        public float x;
        public float y;
        public float z;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Nolo_Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Nolo_Pose
    {
        public Nolo_Vector3 pos;
        public Nolo_Quaternion rot;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Nolo_ControllerStates
    {
        public uint buttons;
        public uint touches;
        public Nolo_Vector2 touchpadAxis;
    }
    #endregion

    #region API
    public class API
    {
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        public const string dllName = "noloRuntime";
#elif UNITY_ANDROID
        public const string dllName = "libNoloVrUnity";
#endif
    }
    public class API_1_0_0 : API
    {
        public static int apiVersion = 1;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        [DllImport(dllName)]
        public static extern void open_Nolo_ZeroMQ();

        [DllImport(dllName)]
        public static extern void close_Nolo_ZeroMQ();

        [DllImport(dllName, EntryPoint = "get_Nolo_VersionID")]
        public static extern int GetVersionByDeviceType(int type);

        [DllImport(dllName, EntryPoint = "get_Nolo_Battery")]
        public static extern int GetElectricityByDeviceType(int type);

        [DllImport(dllName, EntryPoint = "get_Nolo_Pose")]
        public static extern Nolo_Pose GetPoseByDeviceType(int type);

        [DllImport(dllName, EntryPoint = "get_Nolo_ControllerStates")]
        public static extern Nolo_ControllerStates GetControllerStatesByDeviceTyp(int type);
       
        [DllImport(dllName, EntryPoint = "set_Nolo_TriggerHapticPulse")]
        public static extern bool Nolovr_TriggerHapticPulse(int type,int intensity);

        [DllImport(dllName, EntryPoint = "disConnect_FunCallBack")]
        public static extern bool disConnenct_FunCallBack(DisConnectedCallBack callback);

        [DllImport(dllName, EntryPoint = "connectSuccess_FunCallBack")]
        public static extern bool connectSuccess_FunCallBack(ConnectedCallBack callback);
#elif UNITY_ANDROID
        [DllImport(dllName)]
        public static extern int GetVersionByDeviceType(int type);

        [DllImport(dllName)]
        public static extern int GetElectricityByDeviceType(int type);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall)]
        public static extern Nolo_Pose GetPoseByDeviceType(int type);

        [DllImport(dllName, EntryPoint = "GetControllerStatesByDeviceType")]
        public static extern Nolo_ControllerStates GetControllerStatesByDeviceTyp(int type);

#endif
    }
    public class API_2_0_0 : API
    {
        public static int apiVersion = 2;
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        [DllImport(dllName, EntryPoint = "get_Nolo_StateByDeviceType")]
        public static extern int GetDeviceTrackingStatus(int type);
        [DllImport(dllName, EntryPoint = "get_Nolo_HMDInitPosition")]
        public static extern Nolo_Vector3 GetHmdInitPosition();
#elif UNITY_ANDROID
        [DllImport(dllName)]
        public static extern int GetDeviceTrackingStatus(int type);
        [DllImport(dllName)]
        public static extern Nolo_Vector3 GetHmdInitPosition();
#endif
    }
    #endregion
}
