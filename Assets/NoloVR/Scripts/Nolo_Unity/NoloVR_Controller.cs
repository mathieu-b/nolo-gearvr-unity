/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Controller.cs
 *   
*************************************************************/

using UnityEngine;

public class NoloVR_Controller {
    //button mask
    public class ButtonMask
    {
        public const uint TouchPad = 1 << (int)NoloButtonID.TouchPad;
        public const uint Trigger = 1 << (int)NoloButtonID.Trigger;
        public const uint Menu = 1 << (int)NoloButtonID.Menu;
        public const uint System = 1 << (int)NoloButtonID.System;
        public const uint Grip = 1 << (int)NoloButtonID.Grip;
    }
    //touch mask
    public class TouchMask
    {
        public const uint TouchPad = 1 << (int)NoloTouchID.TouchPad;
    }
    //device message
    public class NoloDevice
    {
        public NoloDevice(int num)
        {
            index = num;
        }
        public int index { get; private set; }
        public Nolo_Transform GetPose()
        {
            Update();
            return pose;
        }

        public bool GetNoloButtonPressed(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) != 0;
        }
        public bool GetNoloButtonDown(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) != 0 && (preControllerStates.buttons & buttonMask) == 0;
        }
        public bool GetNoloButtonUp(uint buttonMask)
        {
            Update();
            return (controllerStates.buttons & buttonMask) == 0 && (preControllerStates.buttons & buttonMask) != 0;
        }

        public bool GetNoloButtonPressed(NoloButtonID button)
        {
            return GetNoloButtonPressed((uint)1 << (int)button);
        }
        public bool GetNoloButtonDown(NoloButtonID button)
        {
            return GetNoloButtonDown((uint)1 << (int)button);
        }
        public bool GetNoloButtonUp(NoloButtonID button)
        {
            return GetNoloButtonUp((uint)1 << (int)button);
        }

        public bool GetNoloTouchPressed(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) !=0;
        }
        public bool GetNoloTouchDown(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) != 0 && (preControllerStates.touches & touchMask) == 0;
        }
        public bool GetNoloTouchUp(uint touchMask)
        {
            Update();
            return (controllerStates.touches & touchMask) == 0 && (preControllerStates.touches & touchMask) != 0;
        }

        public bool GetNoloTouchPressed(NoloTouchID touch)
        {
            return GetNoloTouchPressed((uint)1 << (int)touch);
        }
        public bool GetNoloTouchDown(NoloTouchID touch)
        {
            return GetNoloTouchDown((uint)1 << (int)touch);
        }
        public bool GetNoloTouchUp(NoloTouchID touch)
        {
            return GetNoloTouchUp((uint)1 << (int)touch);
        }

        //touch axis return vector2 x(-1~1)y(-1,1)
        public Vector2 GetAxis(NoloTouchID axisIndex = NoloTouchID.TouchPad)
        {
            Update();
            if ((controllerStates.touches &(1 << (int)axisIndex)) != 0)
            {
                return new Vector2(controllerStates.touchpadAxis.x, controllerStates.touchpadAxis.y);
            }
            return Vector2.zero;
        }
        public NoloTrackingStatus GetTrackingStaus()
        {
            Update();
            if (trackingStatus == 0)
            {
                return NoloTrackingStatus.OutofRange;
            }
            if (trackingStatus == 1)
            {
                return NoloTrackingStatus.Normal;
            }
            return NoloTrackingStatus.NotConnect;
        }

        private int trackingStatus;
        private NoloVR_Plugins.Nolo_ControllerStates controllerStates, preControllerStates;
        private Nolo_Transform pose;
        private int preFrame = -1;
        public void Update()
        {
            if (Time.frameCount != preFrame)
            {
                preFrame = Time.frameCount;
                preControllerStates = controllerStates;
                if (NoloVR_Playform.InitPlayform().GetPlayformError() == NoloError.None)
                {
                    pose = NoloVR_Plugins.GetPose(index);
                    controllerStates = NoloVR_Plugins.GetControllerStates(index);
                    trackingStatus = NoloVR_Plugins.GetTrackingStatus(index);
                }
            }
        }

        //HapticPulse  parameter must be in 0~100
        public void TriggerHapticPulse(int intensity)
        {
            if (NoloVR_Playform.InitPlayform().GetPlayformError() == NoloError.None)
            {
                NoloVR_Playform.InitPlayform().TriggerHapticPulse(index, intensity);
            }
        }
    }
    
    //device manager
    public static NoloDevice[] devices;
    public static NoloDevice GetDevice(NoloDeviceType deviceIndex)
    {
        if (devices == null)
        {
            devices = new NoloDevice[NoloVR_Plugins.trackedDeviceNumber];
            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new NoloDevice(i);
            }
        }
        return devices[(int)deviceIndex];
    }
    public static NoloDevice GetDevice(NoloVR_TrackedDevice trackedObject)
    {
        return GetDevice(trackedObject.deviceType);
    }

}
