using UnityEngine;
using System.Collections;

public class HeadPin : MonoBehaviour
{
    

    public void OnCollisionEnter2D(Collision2D collision)
    {

        GetComponentInParent<PlayerMovement>().Hit();
    }
}
