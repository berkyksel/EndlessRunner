using UnityEngine;

public class Pickup : Spawnable
{
    [SerializeField] int ScoreEffect;
    [SerializeField] float SpeedEffect;
    [SerializeField] float SpeedEffectDuration;

    bool bAdjusted = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SpeedController speedController = FindAnyObjectByType<SpeedController>();
            if(speedController != null)
            {
                speedController.ChangeGlobalSpeed(SpeedEffect, SpeedEffectDuration);

            }

            ScoreKeeper scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
            if(scoreKeeper != null)
            {
                scoreKeeper.ChangeScore(ScoreEffect);
            }
            Destroy(gameObject);
        }
        
        if(other.gameObject.tag == "Threat" && !bAdjusted)
        {
            Collider col = other.gameObject.GetComponent<Collider>();
            if(col != null)
            {
                bAdjusted = true;
                transform.position = col.bounds.center + (col.bounds.extents.y * gameObject.GetComponent<Collider>().bounds.center.y) * Vector3.up;
            }
        }
    }
}
