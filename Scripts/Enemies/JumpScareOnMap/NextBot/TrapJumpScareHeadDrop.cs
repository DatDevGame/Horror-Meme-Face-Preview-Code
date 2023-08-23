using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapJumpScareHeadDrop : AbtractTrapJumpScare
{
    [SerializeField] private GameObject _headModel;
    IEnumerator _delayDestroy;

    protected override void ActiveJumpScare()
    {
        base.ActiveJumpScare();

        _headModel.SetActive(true);

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
