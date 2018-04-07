using UnityEngine;

public abstract class AdvancedStateMachineBehaviour : StateMachineBehaviour
{
    protected AnimatorStateInfo stateInfo;
    public AnimatorStateInfo StateInfo
    {
        get { return stateInfo; }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        this.stateInfo = stateInfo;
    }
}