using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CoinCollectEffect : MonoBehaviour
{
	[SerializeField] GameObject _coinGO;
	List<GameObject> coinList;
	private int _numberOfCoin = 7;
	public bool _isCompleteEffect;

	public IEnumerator StartCollectionEffect(RectTransform coinFrom, RectTransform coinTo,
		//Action<float> OnCollecting, 
		Action OnCompleteCollecting)
	{
		StartCoroutine(CollectEffect(coinFrom, coinTo));

		// yield return new WaitUntil( () => _isCompleteEffect);

		yield return new WaitForSeconds(1.1f);

		foreach (var coin in coinList)
			Destroy(coin);

		OnCompleteCollecting?.Invoke();
	}

	private IEnumerator CollectEffect(RectTransform from, RectTransform to)
	{
		if (from == null || to == null) yield break;
		_isCompleteEffect = false;

		coinList = new List<GameObject>();

		//int currentCount = 0;

		for(int i = 0; i < _numberOfCoin; i++)
        {
			Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-_numberOfCoin * 50.0f, _numberOfCoin * 50.0f), UnityEngine.Random.Range(-200f, 0f), 0);
			var child = Instantiate(_coinGO, from.position, Quaternion.identity, transform);

			child.transform.DOMove(from.position + randomPos, 0.1f, false);
			coinList.Add(child.gameObject);
		}

		yield return new WaitForSeconds(0.35f);

		for (int i = 0; i < _numberOfCoin; i++)
		{
			var child = coinList[i];

			child.transform.DOMove(to.position, UnityEngine.Random.Range(0.25f, 1))
							.SetEase(Ease.InFlash);
							//.OnComplete(() =>
							//{
							//	OnCollecting?.Invoke(_numberOfCoin);
							//	Destroy(child);

							//	currentCount++;
							//	_isCompleteEffect = currentCount == _numberOfCoin;
							//});
		}
	}
}
