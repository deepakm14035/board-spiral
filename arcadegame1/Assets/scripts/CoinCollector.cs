using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("coin"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.addCoin();
            GameObject.Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag.Equals("coin2"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.addCoin();
            gameManager.addCoin();
            GameObject.Destroy(collision.gameObject);
        }
    }
}
