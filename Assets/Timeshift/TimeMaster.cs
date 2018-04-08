using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimeMaster : MonoBehaviour
{
    List<Instant> timeline = new List<Instant>();
    List<TimeDependant> trackedObjects = new List<TimeDependant>();

    [HideInInspector]
    public UnityEvent TimeshiftStart, TimeshiftStop;

    bool rewinding = false;
    public bool Rewinding { get { return rewinding; } private set { } }

    public bool ShowTrace = false;
    public int rewindSpeed = 3;

    private void Start()
    {
        FixedUpdate();
    }

    public void TrackObject(TimeDependant trackedObject)
    {
        trackedObjects.Add(trackedObject);
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
                .Select(obj => obj.GetInstant())
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
        TimeshiftStart.Invoke();
        StartCoroutine(RewindingCoroutine());
    }

    public void StopRewind()
    {
        StopAllCoroutines();
        foreach (var obj in trackedObjects)
        {
            foreach (var script in obj.GetComponents<IAffectedByTime>())
                script.Enabled = true;
        }
        TimeshiftStop.Invoke();
        rewinding = false;
    }

    public IEnumerator RewindingCoroutine()
    {
        while (timeline.Count > 0)
        {
            yield return new WaitForFixedUpdate();
            for (int i = 0; i < rewindSpeed && timeline.Count > 0; i++)
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
                item.SetInstant(info);
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

    public struct ObjectInstant
    {
        public int id;
        public bool enabled;
        public Vector3 position;
        public Quaternion rotation;
        public object data;
        public int animatorState;
        public AnimatorParameterInstant[] animatorParameters;

        public ObjectInstant(int id, bool enabled, Vector3 position, Quaternion rotation, object data)
        {
            this.id = id;
            this.enabled = enabled;
            this.position = position;
            this.rotation = rotation;
            this.data = data;
            this.animatorState = 0;
            this.animatorParameters = new AnimatorParameterInstant[0];
        }

        public ObjectInstant(int id, bool enabled, Vector3 position, Quaternion rotation, int animatorState, AnimatorParameterInstant[] animatorParameters, object data): this(id, enabled, position, rotation, data)
        {
            this.animatorState = animatorState;
            this.animatorParameters = animatorParameters;
        }
    }

    public struct AnimatorParameterInstant
    {
        public int nameHash;
        public AnimatorControllerParameterType type;
        public object value;

        public AnimatorParameterInstant(Animator animator, AnimatorControllerParameter parameter)
        {
            type = parameter.type;
            nameHash = parameter.nameHash;
            switch (type)
            {
                case AnimatorControllerParameterType.Bool:
                    value = animator.GetBool(parameter.nameHash);
                    break;
                case AnimatorControllerParameterType.Float:
                    value = animator.GetFloat(parameter.nameHash);
                    break;
                case AnimatorControllerParameterType.Int:
                    value = animator.GetInteger(parameter.nameHash);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    value = animator.GetBool(parameter.nameHash);
                    break;
                default:
                    value = 0;
                    break;
            }
        }
    }
}
