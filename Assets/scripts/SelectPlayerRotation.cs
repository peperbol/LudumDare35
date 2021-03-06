﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SelectPlayerRotation : MonoBehaviour {
    int id;
    Image img;
    public PlayerMovement player;
	void Start () {
        img = GetComponent<Image>();
        id = player.ID;
        transform.localRotation = Quaternion.Euler(0,0, player.Orientation);
    }
	
	void Update () {
	 if(Input.GetButton(id+"Lbumper") && Input.GetButton(id + "Rbumper"))
        {
            img.enabled = true;
            Vector2 v = new Vector2(Input.GetAxis(id + "Horizontal"), Input.GetAxis(id + "Vertical"));
            if(v.sqrMagnitude > 0.1f) { 
            transform.localRotation = Quaternion.LookRotation(Vector3.forward, -v) ;
            
            player.Orientation = transform.localRotation.eulerAngles.z;
            }
        }
        else img.enabled = false;
    }
}
