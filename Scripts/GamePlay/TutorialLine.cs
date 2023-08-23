using Assets._GAME.Scripts.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLine : MonoBehaviour

{
    private Transform _target;

    private LineRenderer _lineRenderer;
    void Start()
    {
        _target = GamePlayLoader.Instance.Runner.transform;
        _lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer?.SetPosition(0, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
		
	}

    // Update is called once per frame
    void Update()
    {
        _lineRenderer?.SetPosition(1, _target.position);
    }
}
