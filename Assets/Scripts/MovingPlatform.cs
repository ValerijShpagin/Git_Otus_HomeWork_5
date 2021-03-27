using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speedPlatform;
    [SerializeField] private GameObject _startPos;
    [SerializeField] private List<GameObject> _points = new List<GameObject>();

    private int index;
    [SerializeField] private Vector3[] currentPoint = new Vector3[3];

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            currentPoint[i] = new Vector3(_points[i].transform.position.x, _points[i].transform.position.y, _points[i].transform.position.z);
        }

        index = 0;
        _speedPlatform = 1.5f;
        transform.position = _startPos.transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint[index], Time.deltaTime * _speedPlatform);

        if (transform.position == currentPoint[index]) 
        {
            index = (index + 1) % currentPoint.Length;
        }
    }

}
