using UnityEngine;
using System.Collections;

public class NoloVR_BaseStation : MonoBehaviour {
    private Vector3 prePos = Vector3.zero;
	void Update () 
    {
        if (App.noloPlayform.GetPlayformError() == NoloError.None)
        {
            transform.localPosition = new Vector3(0, -NoloVR_Plugins.GetHmdInitPosition().y, 0);
        }
	}
}
