using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeDependant : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<TimeMaster>().TrackObject(this);
    }

    public object GetData()
    {
        return GetComponents<IAffectedByTime>().Select(d => new KeyValuePair<int, object>(d.GetHashCode(), d.GetData())).ToArray();
    }

    public void SetData(object data)
    {
        if (data == null)
            return;
        IAffectedByTime[] components = GetComponents<IAffectedByTime>();
        KeyValuePair<int, object>[] dict = data as KeyValuePair<int, object>[];
        foreach (var pair in dict)
        {
            IAffectedByTime component = components.FirstOrDefault(c => c.GetHashCode() == pair.Key);
            if (component != null)
                component.SetData(pair.Value);
        }
    }
}
