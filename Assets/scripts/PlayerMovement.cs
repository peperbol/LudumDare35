using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float maxRotationSpeed;
    public Transform head;
    public Collider killZone;
    public static int num  = 1;
    public float TurnSpeed;
    public float Orientation { get { return orientations[id - 1]; } set { orientations[id - 1] = value; } }
    public Renderer headVisual;
    static float[] orientations = new float[8];
    int id;
    bool shooting;
    bool pulling;
    Queue<Vector3> track;
    Queue<Quaternion> trackRots;
    bool flying;
    public float FlySpeed;
    LineRenderer lr;
    Collider headCollider;
    Vector3 headOffset;
    public int minShootDistance = 2;
    [System.NonSerialized]
    public ParticleSystem ps;
    
    public int ID { get { return id; } }

    void Awake()
    {
        Shooting = false;
        lr = GetComponent<LineRenderer>();
        headCollider = head.GetComponentInChildren<Collider>();
        ps = GetComponentInChildren<ParticleSystem>();
        if (ps)
            ps.Stop();
        track = new Queue<Vector3>();
        trackRots = new Queue<Quaternion>();
        headOffset = -head.localPosition;
        id = num++;
        setLayer(transform, 24);
        setLayer(head, 16);
        setLayer(killZone.transform, 8);
        GetComponentsInChildren<Renderer>().All(e => { e.material = Resources.Load<Material>("mat_Player" + ID); return true; });

        if (ps)
            ps.GetComponent<Renderer>().material = Resources.Load<Material>("mat_waterflowparticle");
        headVisual.material.color = headVisual.material.color.SetA(0.3f);
        headVisual.enabled = false;
    }
    void setLayer(Transform t, int start)
    {
        t.gameObject.layer = start + ID - 1;
        for (int i = 0; i < t.childCount; i++)
        {
            setLayer(t.GetChild(i), start);
        }
    }
    // Update is called once per frame
    void Update()
    {
        RotateTo(RotationSpace.Rotate(new Vector2(Input.GetAxis(id +"Horizontal"), Input.GetAxis(id + "Vertical")),Orientation));
        Rotate(Input.GetAxis(id + "Turn"));
        if (Input.GetButtonDown(id + "Shoot")) Shoot();
        lr.SetVertexCount(track.Count);
        lr.SetPositions(track.ToArray());
    }

    Transform toRotate;

    void RotateTo(Vector2 target)
    {
        if (target.sqrMagnitude > 0 && toRotate)
        {

            Quaternion q = Quaternion.LookRotation( Vector3.down, new Vector3(target.x,0,target.y));
            toRotate.rotation = Quaternion.Lerp(toRotate.rotation, q, target.magnitude / maxRotationSpeed * Time.deltaTime * 60);
        }
    }
    void Rotate(float f)
    {
        if ( toRotate)
        {
            toRotate.Rotate(Vector3.forward, f * Time.deltaTime * TurnSpeed);
        }
    }
    bool Shooting
    {
        get
        {
            return shooting;
        }
        set
        {
            shooting = value;
            if (value) {
                toRotate = head;
            }
            else {
                toRotate = transform;
            }

            head.GetComponentsInChildren<Collider2D>().All(e => { return e.enabled = value; });
        }
    }
    void Shoot()
    {
        if (!Shooting)
        {
            Shooting = true;
            StartCoroutine(Fly());
        }
        else if (!flying && !pulling)
        {
            pulling = true;
            StartCoroutine(Pull());
        }
    }

    public void Hit() { flying = false;  }
    IEnumerator Fly()
    {

        headCollider.enabled = headVisual.enabled = flying = true;
        track.Enqueue(head.position);
        trackRots.Enqueue(head.rotation);

        yield return null;
        while (flying)
        {
            head.position += head.up * FlySpeed * Time.deltaTime;
            track.Enqueue(head.position);
            trackRots.Enqueue(head.rotation);
            yield return null;
        }
        toRotate = null;
        if (track.Count <= minShootDistance)
        {
            pulling = true;
            StartCoroutine(Pull());
        }
        
        headCollider.enabled = false;
    }

    IEnumerator Pull()
    {
        if (ps)
            ps.Play();
        Vector3 pos = head.position;
        Quaternion q = head.rotation;
        if (track.Count > minShootDistance)
        {
            killZone.enabled = true;
            while (track.Count > 0)
            {
                transform.position = track.Dequeue() + transform.TransformVector(headOffset);
                transform.rotation = trackRots.Dequeue();
                head.position = pos;
                head.rotation = q;
                yield return null;
            }

            killZone.enabled = false;
        }
        else
        {
            track.Clear();
        }
        Shooting = false;
        head.localRotation = Quaternion.identity;
        head.localPosition = -headOffset;

        if (ps)
            ps.Stop();
        pulling = false;
        headVisual.enabled = false;
    }
}
