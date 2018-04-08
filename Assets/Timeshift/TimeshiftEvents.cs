using UnityEngine;
using UnityEngine.Events;

public class TimeshiftEvents : MonoBehaviour
{
    public UnityEvent TimeshiftStart = new UnityEvent(), TimeshiftStop = new UnityEvent();

    private void Start()
    {
        FindObjectOfType<TimeMaster>().TimeshiftStart.AddListener(_TimeshiftStart);
        FindObjectOfType<TimeMaster>().TimeshiftStop.AddListener(_TimeshiftStop);
    }

    void _TimeshiftStart()
    {
        TimeshiftStart.Invoke();
    }

    void _TimeshiftStop()
    {
        TimeshiftStop.Invoke();
    }
}