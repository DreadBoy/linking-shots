using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    List<Instant> timeline = new List<Instant>();
    List<TimeDependant> trackedObjects = new List<TimeDependant>();

    bool rewinding = false;

    public bool ShowTrace = false;

    private void Start()
    {
        FixedUpdate();
    }

    public void TrackObject(TimeDependant trackedObject)
    {
        trackedObjects.Add(trackedObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.Space))
            StopRewind();
    }

    private void FixedUpdate()
    {
        if (ShowTrace)
        {
            for (int i = 0; i < timeline.Count - 1; i++)
            {
                for (int j = 0; j < timeline[i].objects.Length; j++)
                {
                    Debug.DrawLine(timeline[i].objects[j].position, timeline[i + 1].objects[j].position);
                }
            }
        }
        if (rewinding)
            return;
        Instant instant = new Instant(
            trackedObjects
                .Select(obj => new ObjectInstant(obj.GetInstanceID(), obj.gameObject.activeSelf,
                    obj.transform.position, obj.transform.rotation, obj.GetData()))
                .ToArray()
        );
        timeline.Add(instant);
    }

    public void StartRewind()
    {
        rewinding = true;
        foreach (var obj in trackedObjects)
        {
            foreach (var script in obj.GetComponents<IAffectedByTime>())
                script.Enabled = false;
        }
        foreach (var timeshift in FindObjectsOfType<TimeshiftColour>())
            timeshift.ShiftTimeStart();
        StartCoroutine(Rewinding());
    }

    public void StopRewind()
    {
        StopAllCoroutines();
        foreach (var obj in trackedObjects)
        {
            foreach (var script in obj.GetComponents<IAffectedByTime>())
                script.Enabled = true;
        }
        foreach (var timeshift in FindObjectsOfType<TimeshiftColour>())
            timeshift.ShiftTimeStop();
        rewinding = false;
    }

    public IEnumerator Rewinding()
    {
        while (timeline.Count > 0)
        {
            yield return new WaitForFixedUpdate();
            RewindFrame();
        }
        StopRewind();
        yield return null;
    }

    void RewindFrame()
    {
        Instant lastFrame = timeline.Last();
        // Find all that need to be destroyed 
        // (are in scene and are not in last frame)
        var needToDestroy = trackedObjects.Where(obj => !lastFrame.objects.Select(f => f.id).Contains(obj.GetInstanceID())).ToArray();
        trackedObjects.RemoveAll(obj => !lastFrame.objects.Select(f => f.id).Contains(obj.GetInstanceID()));
        foreach (var item in needToDestroy)
            Destroy(item.gameObject);

        // Find all that need to be re-enabled
        // That means all objects that are enabled in this frame but disabled in scene
        var needToEnable = trackedObjects.Where(obj => !obj.gameObject.activeSelf && lastFrame.objects.Where(o => o.enabled).Select(o => o.id).Contains(obj.GetInstanceID()));
        foreach (var item in needToEnable)
            item.gameObject.SetActive(true);

        foreach (var item in trackedObjects)
        {
            var info = lastFrame.objects.FirstOrDefault(o => o.id == item.GetInstanceID());
            if (!info.Equals(default(ObjectInstant)))
            {
                item.transform.position = info.position;
                item.transform.rotation = info.rotation;
                item.SetData(info.data);
            }
        }

        timeline = timeline.Take(timeline.Count - 1).ToList();
    }

    struct Instant
    {
        public ObjectInstant[] objects;

        public Instant(ObjectInstant[] objects)
        {
            this.objects = objects;
        }
    }

    struct ObjectInstant
    {
        public int id;
        public bool enabled;
        public Vector3 position;
        public Quaternion rotation;
        public object data;

        public ObjectInstant(int id, bool enabled, Vector3 position, Quaternion rotation, object data)
        {
            this.id = id;
            this.enabled = enabled;
            this.position = position;
            this.rotation = rotation;
            this.data = data;
        }
    }
}
