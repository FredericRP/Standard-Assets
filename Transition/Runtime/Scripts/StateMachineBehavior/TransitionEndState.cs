using UnityEngine;

namespace FredericRP.Transition
{

  public class TransitionEndState : StateMachineBehaviour
  {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
      if (animator.GetBool("visible"))
        animator.gameObject.GetComponent<Transition>().TriggerDisplayed();
      else
        animator.gameObject.GetComponent<Transition>().TriggerHidden();
    }
  }

}