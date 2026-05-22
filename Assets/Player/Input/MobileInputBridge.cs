using UnityEngine;

public class MobileInputBridge : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SwipeInput swipeInput;

    private void Awake()
    {
        swipeInput.OnSwipeLeft += player.SwipeLeft;
        swipeInput.OnSwipeRight += player.SwipeRight;
        swipeInput.OnSwipeUp += player.SwipeUp;
    }
}
