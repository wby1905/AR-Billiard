using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holes : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip goalSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball" && Vector3.Distance(transform.position, other.transform.position) < other.GetComponent<SphereCollider>().radius)
        {
            audioSource.PlayOneShot(goalSound);
        }
    }
}
