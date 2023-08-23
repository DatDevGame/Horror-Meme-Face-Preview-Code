using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
	public void AddDisposable(IDisposable disposable);

	public void ClearObservers();
}
