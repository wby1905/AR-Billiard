using UnityEngine;

public class CueBall : MonoBehaviour
{

    private int firstHit = -1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && firstHit == -1)
        {
            firstHit = collision.gameObject.GetComponent<Ball>().ballNumber;
        }
    }

    public int GetFirstHit()
    {
        return firstHit;
    }

    public void ResetFirstHit()
    {
        firstHit = -1;
    }


}