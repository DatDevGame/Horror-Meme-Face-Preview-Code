using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapJumpScareMonster : AbtractTrapJumpScare
{
    [SerializeField]
    private Animator _monsterJumpAnimator;
    private const float LENGTH_ANIMATION_EXIT = 0.1F;

    private IEnumerator _JumpScareHandle;
    protected override void ActiveJumpScare()
    {
        base.ActiveJumpScare();

        _monsterJumpAnimator.SetTrigger("OnTriggerHeadAnimation");

        if (_JumpScareHandle != null)
            StopCoroutine(_JumpScareHandle);

        _JumpScareHandle = JumpScareHandle();
        StartCoroutine(_JumpScareHandle);
    }

    IEnumerator JumpScareHandle()
    {
        _monsterJumpAnimator.SetTrigger("OnTriggerHeadAnimation");
        yield return new WaitForSeconds(TIME_DESTROY);
        _monsterJumpAnimator.SetTrigger("OffTriggerHeadAnimation");
        yield return new WaitForSeconds(LENGTH_ANIMATION_EXIT);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (_JumpScareHandle != null)
            StopCoroutine(_JumpScareHandle);
    }
}
