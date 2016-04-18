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
            GameObject g = Instantiate(Resources.Load<GameObject>("pref_Explosion"));
            g.GetComponent<Renderer>().material = p.GetComponentInChildren<Renderer>().material;
            g.transform.position = p.transform.position;
            Destroy(p.gameObject);
        }
        if ( FindObjectsOfType<PlayerMovement>().Length == 2)
        {
            GameOver.instance.PlayerWin(GetComponentInParent<PlayerMovement>().ID);
        }
    }
}
