using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private Vector2 startTouch;
    private Vector2 endTouch;

    [SerializeField] float swipeThreshold = 80f;

    public System.Action OnSwipeLeft;
    public System.Action OnSwipeRight;
    public System.Action OnSwipeUp;

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouch = touch.position;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            endTouch = touch.position;
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        Vector2 delta = endTouch - startTouch;

        if (delta.magnitude < swipeThreshold) return;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                OnSwipeRight?.Invoke();
            else
                OnSwipeLeft?.Invoke();
        }
        else
        {
            if (delta.y > 0)
                OnSwipeUp?.Invoke();
        }
    }
}
