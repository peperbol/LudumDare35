using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float maxRotationSpeed;
    public Transform head;
    public Collider2D killZone;
    public static int num  = 1;
    public float TurnSpeed;
    public float Orientation { get { return orientations[id - 1]; } set { orientations[id - 1] = value; } }
    static float[] orientations = new float[8];
    int id;
    bool shooting;
    bool pulling;
    Queue<Vector3> track;
    Queue<Quaternion> trackRots;
    bool flying;
    public float FlySpeed;
    LineRenderer lr;
    Vector3 headOffset;
    [System.NonSerialized]
    public ParticleSystem ps;
    public int ID { get { return id; } }

    void Awake()
    {
        Shooting = false;
        lr = GetComponent<LineRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
        track = new Queue<Vector3>();
        trackRots = new Queue<Quaternion>();
        headOffset = -head.localPosition;
        id = num++;
        setLayer(transform);
        GetComponentsInChildren<Renderer>().All(e => { e.material = Resources.Load<Material>("mat_Player" + ID);return true; });
        ps.GetComponent<Renderer>().material = Resources.Load<Material>("mat_Particle" + ID);
    }
    void setLayer(Transform t)
    {
        t.gameObject.layer = 20 + ID;
        for (int i = 0; i < t.childCount; i++)
        {
            setLayer(t.GetChild(i));
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
            Quaternion q = Quaternion.LookRotation(Vector3.forward, target);
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

    public void Hit() { flying = false; }
    IEnumerator Fly()
    {
        flying = true;
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
    }

    IEnumerator Pull()
    {
        ps.Play();
        Vector3 pos = head.position;
        Quaternion q = head.rotation;
        if (track.Count > 2)
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
        Shooting = false;
        head.localRotation = Quaternion.identity;
        head.localPosition = -headOffset;
        ps.Stop();
        pulling = false;
    }
}
