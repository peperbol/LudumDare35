using UnityEngine;
using System.Collections;

public class ArrayCopy : MonoBehaviour {
    public int amountOfcopies;
    public Vector3 offset;
    public Vector3 scale= Vector3.one;
	// Use this for initialization
	void Awake () {
        if (amountOfcopies > 0)
        {
            amountOfcopies -= 1;
            GameObject go = Instantiate(gameObject);
            go.transform.SetParent(transform);
            go.transform.localPosition = offset;
            go.transform.localScale = scale;
            Material mat = go.GetComponent<Renderer>().material;
            go.GetComponent<Renderer>().material.color = mat.color.SetV(mat.color.GetV() * Mathf.Pow((((float)amountOfcopies) / (amountOfcopies + 1)), 3));
            Debug.Log(amountOfcopies + " " + Mathf.Pow((((float)amountOfcopies) / (amountOfcopies + 1)), 2) + " " + mat.color.GetV());
        }
	}
	
}
