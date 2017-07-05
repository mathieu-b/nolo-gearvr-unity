using UnityEngine;
using System.Collections;

public class BoundaryRegion : MonoBehaviour {
    public GameObject baseStation;
    public SpriteRenderer partner;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<NoloVR_TrackedDevice>()!=null)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            partner.enabled = true;
            baseStation.SetActive(true);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<NoloVR_TrackedDevice>() != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            partner.enabled = false;
            baseStation.SetActive(false);
        }
    }
}