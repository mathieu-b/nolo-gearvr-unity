/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Manager.cs
 *   
*************************************************************/

using UnityEngine;
public class NoloVR_Manager : MonoBehaviour
{
    [Tooltip("the camera's rotation should be changed when the app running")]
    public GameObject VRCamera;
    [Tooltip("if you develop a gear app choose Gear,else choose Cardboard")]
    public bool debug = true;
    public NoloLogType logType;
    [HideInInspector]
    public NoloVR_TrackedDevice[] objects;


    private int spacingFrame = 0;
    private static NoloVR_Manager instance;
    private NoloVR_Manager() { }

    public static NoloVR_Manager GetInstance()
    {
        if (instance == null)
        {
            instance = new NoloVR_Manager();
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
        objects = GameObject.FindObjectsOfType<NoloVR_TrackedDevice>();
        if (debug)
        {
            GameObject noloDebugObject = new GameObject("NoloDebugObject");
            noloDebugObject.AddComponent<NoloVR_Logs>().SetLogType(logType);
        }
        GameObject androidCallBack = new GameObject("USB Host Peripherals");
        androidCallBack.AddComponent<NoloVR_AndroidCallBack>();
    }

    void Update ()
    {
        //try to connect the usb device
        if (NoloVR_Playform.InitPlayform().GetPlayformError() != NoloError.None)
        {
            if (spacingFrame < 200)
            {
                spacingFrame ++;
            }
            else
            {
                NoloVR_Playform.InitPlayform().ConnectDevice();
                spacingFrame = 0;
            }
        }
    }

    void OnApplicationQuit()
    {
        //close connect from device
        Debug.Log("Nolo debug:Application quit");
        NoloVR_Playform.InitPlayform().DisconnectDevice();
    }

}
