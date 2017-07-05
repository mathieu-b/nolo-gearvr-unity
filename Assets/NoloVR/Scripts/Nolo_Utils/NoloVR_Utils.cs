/*************************************************************
 * 
 *  Copyright(c) 2017 Lyrobotix.Co.Ltd.All rights reserved.
 *  NoloVR_Utils.cs
 *   
*************************************************************/
using UnityEngine;
using System.Collections;

public struct Nolo_Transform
{
    public Vector3 pos;
    public Quaternion rot;
    public Nolo_Transform(NoloVR_Plugins.Nolo_Pose pose)
    {
        this.pos.x = pose.pos.x;
        this.pos.y = pose.pos.y;
        this.pos.z = pose.pos.z;

        this.rot.w = pose.rot.w;
        this.rot.x = pose.rot.x;
        this.rot.y = pose.rot.y;
        this.rot.z = pose.rot.z;
    }
    public Nolo_Transform(Vector3 pos,Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
    public static Nolo_Transform identity
    {
       get { return new Nolo_Transform(Vector3.zero, Quaternion.identity); }
    }
}


public class NoloVR_Utils
{
    public static Vector3 RecenterPos(int index)
    {
        return Vector3.zero;
    }
}
