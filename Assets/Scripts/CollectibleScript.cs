using TMPro;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{

    public int coins;
    public TextMeshProUGUI collectibleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        collectibleText.text = coins.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // destroy game object

        coins = coins + 1;
    }
}
