using System.Linq;
using UnityEngine;

static class Utils
{
    public static T GetCurrentBehaviour<T>(this Animator animator) where T : GuardState
    {
        return animator.GetBehaviours<T>().FirstOrDefault(behaviour => behaviour.StateInfo.fullPathHash == animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
    }
}