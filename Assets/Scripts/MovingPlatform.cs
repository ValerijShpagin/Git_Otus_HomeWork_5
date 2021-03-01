using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public Transform top;
    //public Transform bottom;
    //public float speed;

    //private float current;  // �� 0 �� 1
    //private float dir;

    //void Start()
    //{
    //    current = 0.0f;
    //    dir = 1.0f;
    //}

    //void Update()
    //{
    //    current += dir * speed * Time.deltaTime;
    //    if (current > 1.0f) {
    //        current = 1.0f;
    //        dir = -1.0f;
    //    } else if (current < 0.0f) {
    //        current = 0.0f;
    //        dir = 1.0f;
    //    }

    //    transform.position = Vector3.Lerp(top.position, bottom.position, current);
    //}


    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public float time;
    private Vector3 b;
    private Vector3 c;
    private Vector3 d;
   [SerializeField] private bool waypoint2;
   [SerializeField] private bool waypoint3;
   [SerializeField] private bool waypoint1;
    public float currentDir;
    public float direction;
    public float speed;

    private void Start()
    {
       

        // transform.position = point1.transform.position;
        // currentDir = 0.0f;
        // direction = 1.0f;
    }

    private void Update()
    {
        b = point2.transform.position;
        c = point3.transform.position;
        d = point1.transform.position;

        if (!waypoint2)
        {
            MoveFearstPointPlatform();
        }

        if (waypoint2 && !waypoint3)
        {
            MoveSecondPointPlatform();
        }
        
        if (waypoint2 && waypoint3 && !waypoint1)
        {
            MoveThreePointPlatform();
        }

    }

    private void MoveFearstPointPlatform()
    {
        if (Vector3.Distance(transform.position, b) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, b, speed);
                     
        }

        else
        {
            waypoint2 = true;
        }
    }

    private void MoveSecondPointPlatform()
    {
        if (Vector3.Distance(transform.position, c) > 0.01f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, c, speed);
        }
        
        else
        {
            waypoint3 = true;
        }
    }
    
    private void MoveThreePointPlatform()
    {
        if (Vector3.Distance(transform.position, d) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, d, speed), speed);
        }

        else
        {
            waypoint2 = false;
            waypoint3 = false;
            waypoint1 = false;
        }
    }
}
