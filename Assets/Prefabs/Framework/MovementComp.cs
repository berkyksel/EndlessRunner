using System;
using UnityEngine;

public class MovementComp : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 20f;
    [SerializeField] Vector3 MoveDir = Vector3.forward;
    public bool spawnedNext = false;

    [SerializeField] Vector3 Destination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SetMoveDir(Vector3 dir)
    {
        MoveDir = dir;
    }

    public void SetDestination(Vector3 newDestination) 
    {
        Destination = newDestination;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += MoveDir * MoveSpeed * Time.deltaTime;
        if(Vector3.Dot((Destination - transform.position).normalized, MoveDir) < 0 )
        {
            Destroy(gameObject);
        }
    }

    internal void SetMoveSpeed(float newMoveSpeed)
    {
        MoveSpeed = newMoveSpeed;
    }

    public void CopyFrom(MovementComp other)
    {
        SetMoveSpeed(other.MoveSpeed);
        SetMoveDir(other.MoveDir);
        SetDestination(other.Destination);
    }
}
