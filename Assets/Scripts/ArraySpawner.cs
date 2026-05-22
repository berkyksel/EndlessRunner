using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    [SerializeField] int Amount = 10;
    [SerializeField] float Gap = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i <= Amount; i++) {
            Vector3 spawnPosition = transform.position + transform.forward * Gap * i;
            GameObject nextCoin = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            nextCoin.GetComponent<ArraySpawner>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
