using UnityEngine;

public class FailZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GamePlayStatics.GetGameMode().GameOver();
        }
    }

    
}
