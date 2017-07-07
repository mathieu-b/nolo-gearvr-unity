/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Controller.cs
 *   
*************************************************************/

using UnityEngine;

using System.Threading;

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
            mBufferedNoloState = new BufferedNoloState(index);
        }

        public int index { get; private set; }
        public Nolo_Transform GetPose()
        {
            return mBufferedNoloState.Get().pose;
        }

        public bool GetNoloButtonPressed(uint buttonMask)
        {
            return (mBufferedNoloState.Get().controllerStates.buttons & buttonMask) != 0;
        }

        NoloState mState = new NoloState();

        public bool GetNoloButtonDown(uint buttonMask)
        {
            mBufferedNoloState.GetCopyTo(mState);

            return (mState.controllerStates.buttons & buttonMask) != 0 && 
                   (mState.preControllerStates.buttons & buttonMask) == 0;
        }

        public bool GetNoloButtonUp(uint buttonMask)
        {
            mBufferedNoloState.GetCopyTo(mState);

            return (mState.controllerStates.buttons & buttonMask) == 0 &&
                   (mState.preControllerStates.buttons & buttonMask) != 0;
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
            return (mBufferedNoloState.Get().controllerStates.touches & touchMask) !=0;
        }

        public bool GetNoloTouchDown(uint touchMask)
        {
            mBufferedNoloState.GetCopyTo(mState);

            return (mState.controllerStates.touches & touchMask) != 0 && 
                   (mState.preControllerStates.touches & touchMask) == 0;
        }

        public bool GetNoloTouchUp(uint touchMask)
        {
            mBufferedNoloState.GetCopyTo(mState);

            return (mState.controllerStates.touches & touchMask) == 0 && 
                   (mState.preControllerStates.touches & touchMask) != 0;
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
        Vector2 mGetAxisVector2 = new Vector2();
        Vector2 mVector2Zero = Vector2.zero;

        public Vector2 GetAxis(NoloTouchID axisIndex = NoloTouchID.TouchPad)
        {
            mBufferedNoloState.GetCopyTo(mState);

            if ((mState.controllerStates.touches & (1 << (int)axisIndex)) != 0)
            {
                mGetAxisVector2.x = mState.controllerStates.touchpadAxis.x;
                mGetAxisVector2.y = mState.controllerStates.touchpadAxis.y;
            }
            else
            {
                mGetAxisVector2 = mVector2Zero;
            }

            return mGetAxisVector2;
        }

        public NoloTrackingStatus GetTrackingStaus()
        {
            mBufferedNoloState.GetCopyTo(mState);

            return (mState.trackingStatus == 0 ? NoloTrackingStatus.OutofRange :
                    mState.trackingStatus == 1 ? NoloTrackingStatus.Normal :
                    NoloTrackingStatus.NotConnect);
        }

        class NoloState
        {
            public volatile int trackingStatus;
            public NoloVR_Plugins.Nolo_ControllerStates controllerStates = new NoloVR_Plugins.Nolo_ControllerStates(); // struct
            public NoloVR_Plugins.Nolo_ControllerStates preControllerStates = new NoloVR_Plugins.Nolo_ControllerStates(); // struct
            public Nolo_Transform pose = new Nolo_Transform(); // struct

            public void CopyTo(NoloState other)
            {
                other.controllerStates = controllerStates;
                other.preControllerStates = preControllerStates;
                other.pose = pose;
                other.trackingStatus = trackingStatus;
            }
        }

        class BufferedNoloState
        {
            public BufferedNoloState(int index)
            {
                mIndex = index;
            }

            int mIndex;

            NoloState mNoloState = new NoloState();
            NoloState mNoloStateCopy = new NoloState();

            NoloState result = new NoloState();

            public void SetFrom(NoloState state)
            {
                Monitor.Enter(mNoloState);
                state.CopyTo(mNoloState);
                Monitor.Exit(mNoloState);
            }

            public NoloState Get()
            {
                if (Monitor.TryEnter(mNoloState))
                {
                    mNoloState.CopyTo(result);
                    mNoloState.CopyTo(mNoloStateCopy);
                    Monitor.Exit(mNoloState);
                }
                else
                {
                    mNoloStateCopy.CopyTo(result);
                }

                return result;
            }

            public void GetCopyTo(NoloState target)
            {
                Get().CopyTo(target);
            }
        }

        BufferedNoloState mBufferedNoloState;

        NoloState mCallbackNoloState = new NoloState();

        public void PeriodicUpdate()
        {
            mCallbackNoloState.preControllerStates = mCallbackNoloState.controllerStates;

            if (App.noloPlayform.GetPlayformError() == NoloError.None)
            {
                mCallbackNoloState.pose = NoloVR_Plugins.GetPose(index);
                mCallbackNoloState.controllerStates = NoloVR_Plugins.GetControllerStates(index);
                mCallbackNoloState.trackingStatus = NoloVR_Plugins.GetTrackingStatus(index);
            }

            mBufferedNoloState.SetFrom(mCallbackNoloState);
        }

        //HapticPulse  parameter must be in 0~100
        public void TriggerHapticPulse(int intensity)
        {
            if (App.noloPlayform.GetPlayformError() == NoloError.None)
            {
                App.noloPlayform.TriggerHapticPulse(index, intensity);
            }
        }
    }
    
    //device manager
    public static NoloDevice[] devices;

    static readonly int kPollIntervalMilliseconds = 1000 / 59;

    static System.Threading.Timer cNoloPollingTimer;

    // Based on: https://stackoverflow.com/a/12797382/1252502

    static System.Diagnostics.Stopwatch cStopwatch = new System.Diagnostics.Stopwatch();

    static void NoloPeriodicUpdate(object pedro)
    {
        cStopwatch.Reset();
        cStopwatch.Start();

        // Long running operation:
        if (devices != null)
        {
            int num_devices = devices.Length;

            for (int i = 0; i < devices.Length; i++)
            {
                devices[i].PeriodicUpdate();
            }
        }

        var next_time_to_wait = System.Math.Max( 0, kPollIntervalMilliseconds - cStopwatch.ElapsedMilliseconds );
        cNoloPollingTimer.Change(next_time_to_wait, Timeout.Infinite );

        //Debug.Log("ALIVE " + next_time_to_wait);
    }

    public static NoloDevice GetDevice(NoloDeviceType deviceIndex)
    {
        if (devices == null)
        {
            devices = new NoloDevice[NoloVR_Plugins.trackedDeviceNumber];

            for (int i = 0; i < devices.Length; i++)
            {
                devices[i] = new NoloDevice(i);
            }

            // Start polling
            cNoloPollingTimer = new Timer(NoloPeriodicUpdate, null, kPollIntervalMilliseconds, Timeout.Infinite);
        }

        return devices[(int)deviceIndex];
    }

    public static NoloDevice GetDevice(NoloVR_TrackedDevice trackedObject)
    {
        return GetDevice(trackedObject.deviceType);
    }

}
