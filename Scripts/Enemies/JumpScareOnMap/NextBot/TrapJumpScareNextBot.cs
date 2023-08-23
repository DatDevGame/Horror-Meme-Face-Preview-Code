using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapJumpScareNextBot : AbtractTrapJumpScare
{
    [SerializeField]
    private Animator _nextBotJumpAnimator;

    [SerializeField]
    private SpriteRenderer _nextbotSprite;

    IEnumerator _delayDestroy;
    protected override void ActiveJumpScare()
    {
        base.ActiveJumpScare();

        _nextbotSprite.color = new Color32(255, 255, 255, 255);
        _nextBotJumpAnimator.SetTrigger("TriggerJumpScare");

        if (_delayDestroy != null)
            StopCoroutine(_delayDestroy);

        _delayDestroy = DestroyObject();
        StartCoroutine(_delayDestroy);
    }
    private void OnDestroy()
    {
        if (_delayDestroy != null)
            StopCoroutine(_delayDestroy);
    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(TIME_DESTROY);
        Destroy(this.gameObject);
    }
}
