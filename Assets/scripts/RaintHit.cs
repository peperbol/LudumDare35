using UnityEngine;
using System.Collections;

public class RaintHit : MonoBehaviour
{
    ParticleSystem ps;
    ParticleCollisionEvent[] particleEvent;
    public GameObject dripPrefab;
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        particleEvent = new ParticleCollisionEvent[16];
    }

    public void OnParticleCollision(GameObject other)
    {
        var safelength = ps.GetSafeCollisionEventSize();
        if(safelength> particleEvent.Length)
        {
            particleEvent = new ParticleCollisionEvent[safelength];
        }
        var numCollisionEvents = ps.GetCollisionEvents(other, particleEvent);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            Instantiate(dripPrefab, particleEvent[i].intersection, Quaternion.Euler(90,0,0));
        }
    }
}
