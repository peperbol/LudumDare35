using UnityEngine;
using System.Collections;

public class RotationSpace : MonoBehaviour {
    static Transform me;

	void Start () {
        me = transform;
	}
	
    public static Vector2 Rotate(Vector2 v, float rot)
    {
        me.rotation = Quaternion.Euler(0, 0, rot);
        return me.TransformDirection(v);
    }
    public static float RotationOf(Vector2 v) {
        me.up = v;
        return me.eulerAngles.z;
    }
}
