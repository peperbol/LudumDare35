using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement p = collision.GetComponentInParent<PlayerMovement>();
        if (p)
        {
            Destroy(p.gameObject);
        }
        if ( FindObjectsOfType<PlayerMovement>().Length == 2)
        {
            GameOver.instance.PlayerWin(GetComponentInParent<PlayerMovement>().ID);
        }
    }
}
