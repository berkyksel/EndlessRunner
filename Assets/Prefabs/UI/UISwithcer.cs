using UnityEngine;

public class UISwithcer : MonoBehaviour
{
    [SerializeField] Transform DefaultSubUI;

    Transform currentActivatedUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.parent == transform)
            {
                child.gameObject.SetActive(false); 
            }
            if (DefaultSubUI != null)
            {
                DefaultSubUI.gameObject.SetActive(true);
                currentActivatedUI = DefaultSubUI;
            }
        }
        SetActiveUI(DefaultSubUI);
        
    }

    public void SetActiveUI(Transform newActiveUI)
    {
        if(newActiveUI == currentActivatedUI)
        {
            return;
        }

        if(currentActivatedUI != null )
        {
            currentActivatedUI.gameObject.SetActive(false);
        }

        newActiveUI.gameObject.SetActive(true);
        currentActivatedUI = newActiveUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
