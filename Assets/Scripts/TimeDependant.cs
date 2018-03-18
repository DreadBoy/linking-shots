using UnityEngine;

public class TimeDependant : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<TimeMaster>().TrackObject(this);
    }
}
