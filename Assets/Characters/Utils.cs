using System.Linq;
using UnityEngine;

static class Utils
{
    public static T GetCurrentBehaviour<T>(this Animator animator) where T : GuardState
    {
        return animator.GetBehaviours<T>().FirstOrDefault(behaviour => behaviour.StateInfo.fullPathHash == animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }

    public static void SetParameter(this Animator animator, TimeMaster.AnimatorParameterInstant parameter)
    {
        AnimatorControllerParameter param = animator.parameters.FirstOrDefault(p => p.nameHash == parameter.nameHash);
        if (param != null)
            switch (param.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(param.nameHash, (bool)parameter.value);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(param.nameHash, (float)parameter.value);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(param.nameHash, (int)parameter.value);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    bool val = (bool)parameter.value;
                    if (val)
                        animator.SetTrigger(param.nameHash);
                    else
                        animator.ResetTrigger(param.nameHash);
                    break;
            }
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return vector;
    }

    public static void SetActiveOnChildren(this GameObject gameObject, bool value)
    {
        foreach (Transform child in gameObject.transform)
            child.gameObject.SetActive(value);
    }
}