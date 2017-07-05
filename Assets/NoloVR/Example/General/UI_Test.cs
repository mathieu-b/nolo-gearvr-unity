using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UI_Test : MonoBehaviour 
{

    private Text UIText;

    [SerializeField]
    bool isTextEnabled = false;

	void Start () {
        UIText = GetComponent<Text>();
    }

    void Update()
    {
        if (isTextEnabled == false) return;

        UIText.text =
                  "HMD POS       :" + NoloVR_Controller.GetDevice(NoloDeviceType.Hmd).GetPose().pos + "\n"
            + "HMD ROT       :" + NoloVR_Controller.GetDevice(NoloDeviceType.Hmd).GetPose().rot + "\n"
            + "HMD VER       :" + NoloVR_Plugins.GetHmdVersion() + "\n"
            + "HMD TRACK     :" + NoloVR_Plugins.GetTrackingStatus(0) + "\n"
            + "LEFT POS      :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetPose().pos + "\n"
            + "LEFT ROT      :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetPose().rot.x + "\n"

            + "LEFT TRIGGER  :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonPressed(NoloButtonID.Trigger) + "\n"
            + "LEFT MENU     :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonPressed(NoloButtonID.Menu) + "\n"
            + "LEFT TOUCHPAD :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonPressed(NoloButtonID.TouchPad) + "\n"
            + "LEFT SYSTEM   :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonPressed(NoloButtonID.System) + "\n"
            + "LEFT GRIP     :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetNoloButtonPressed(NoloButtonID.Grip) + "\n"
            + "LEFT AXIS     :" + NoloVR_Controller.GetDevice(NoloDeviceType.LeftController).GetAxis() + "\n"
            + "LEFT ELE      :" + NoloVR_Plugins.GetElectricity(1) + "\n"
            + "LEFT TRACK    :" + NoloVR_Plugins.GetTrackingStatus(1) + "\n"
            + "RIGHT POS     :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetPose().pos + "\n"
            + "RIGHT ROT     :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetPose().rot + "\n"

            + "RIGHT TRIGGER :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonPressed(NoloButtonID.Trigger) + "\n"
            + "RIGHT MENU    :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonPressed(NoloButtonID.Menu) + "\n"
            + "RIGHT TOUCHPAD:" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonPressed(NoloButtonID.TouchPad) + "\n"
            + "RIGHT SYSTEM  :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonPressed(NoloButtonID.System) + "\n"
            + "RIGHT GRIP    :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetNoloButtonPressed(NoloButtonID.Grip) + "\n"
            + "RIGHT AXIS    :" + NoloVR_Controller.GetDevice(NoloDeviceType.RightController).GetAxis() + "\n"
            + "RIGHT ELE     :" + NoloVR_Plugins.GetElectricity(2) + "\n"
            + "RIGHT TRACK   :" + NoloVR_Plugins.GetTrackingStatus(2) + "\n";

    }
}
