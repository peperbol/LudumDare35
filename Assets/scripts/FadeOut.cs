using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {
    public float time = 1;
    SpriteRenderer sr;
    float timer;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        timer = time;
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0) Destroy(gameObject);
        sr.color = sr.color.SetA(time / timer);
	}
}
