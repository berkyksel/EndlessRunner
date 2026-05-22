using System;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] TrainSegment segmentPrefab;
    [SerializeField] Vector2 SegmentCountRange;
    [SerializeField] Threat threat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenereateTrainBody();
    }

    private void GenereateTrainBody()
    {
        int BodyCount = UnityEngine.Random.Range((int)SegmentCountRange.x, (int)SegmentCountRange.y);
        for (int i = 0; i < BodyCount; i++)
        {
            Vector3 spawnPos = transform.position + transform.forward * segmentPrefab.GetSegmentLength() * i;
            TrainSegment newSegment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
            if(i == 0)
            {
                newSegment.SetHead();
            }
            MovementComp segmentMove = newSegment.GetMovementComponent();
            segmentMove.CopyFrom(threat.GetMovementComponent());
            segmentMove.SetMoveSpeed(4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
