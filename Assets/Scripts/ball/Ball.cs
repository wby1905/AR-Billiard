using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public BallState state;
    public float stopThreshold = 0.02f;
    public int ballNumber;

    private Rigidbody rb;
    private AudioSource audioSource;
    public AudioClip bounceSound;
    public AudioClip wallSound;


    private Vector3 _initPos;
    private Vector3 _savedVelocity;
    private Vector3 _savedAngularV;

    // Start is called before the first frame update
    void Start()
    {
        state = BallState.Idle;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        _initPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > stopThreshold)
        {
            state = BallState.Moving;
        }
        else
        {
            state = BallState.Idle;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole" && Vector3.Distance(transform.position, other.transform.position) < other.GetComponent<SphereCollider>().radius)
        {
            state = BallState.InHole;
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Ball")
        {
            audioSource.PlayOneShot(bounceSound, rb.velocity.magnitude);
        }
        else if (other.gameObject.tag == "Wall")
        {
            audioSource.PlayOneShot(wallSound, rb.velocity.magnitude);
        }

    }

    public void Reset()
    {
        transform.localPosition = _initPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(true);
        state = BallState.Idle;
    }

    public void OnPause()
    {
        _savedVelocity = rb.velocity;
        _savedAngularV = rb.angularVelocity;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        rb.isKinematic = true;
    }

    public void OnResume()
    {
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(_savedVelocity, ForceMode.VelocityChange);
        rb.AddTorque(_savedAngularV, ForceMode.VelocityChange);
    }
}
