using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainSegment : MonoBehaviour
{
    [SerializeField] Mesh HeadMesh;
    [SerializeField] Mesh[] SegmentMeshes;

    [SerializeField] MeshFilter TrainMesh;
    [SerializeField] BoxCollider TrainCollider;
    [SerializeField] MovementComp MovementComponent;

    bool bIsHead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomTrainMesh();
    }

    private void RandomTrainMesh()
    {
        if (bIsHead)
            return;
        int pick = Random.Range(0, SegmentMeshes.Length);
        TrainMesh.mesh = SegmentMeshes[pick];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetSegmentLength()
    {
        return TrainCollider.size.z;
    }

    internal void SetHead()
    {
        TrainMesh.mesh = HeadMesh;
        bIsHead = true;
    }

    internal MovementComp GetMovementComponent()
    {
        return MovementComponent;
    }
}
