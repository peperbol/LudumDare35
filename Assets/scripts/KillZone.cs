using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log(5456);
        PlayerMovement p = collision.GetComponentInParent<PlayerMovement>();
        if (p)
        {
            if(p.ps)
            p.ps.transform.SetParent(null);
            GameObject g = Instantiate(Resources.Load<GameObject>("pref_Explosion"));
            g.GetComponent<Renderer>().material = p.GetComponentInChildren<Renderer>().material;
            g.transform.position = p.transform.position;
            Destroy(p.gameObject);
        }
        if (FindObjectsOfType<PlayerMovement>().Length == 2)
        {
            GameOver.instance.PlayerWin(GetComponentInParent<PlayerMovement>().ID);
        }
    }
}
