using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask obstacleMask;
    Rigidbody rb;
    public Ball ball { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, 0, 10);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BoxCollider>() && !collision.GetComponent<Goal>())
        {
            GiveDamage(collision.transform.position);         
        }

        if (ball)
        {
            ball.CanFire = true;
            ball.FindAWay();
        }

        gameObject.SetActive(false);
    }

    private void GiveDamage(Vector3 initialPosition)
    {
        Collider[] touchColliders=Physics.OverlapSphere(initialPosition, transform.localScale.x, obstacleMask);
        
        for(int i=0; i<touchColliders.Length; i++)
        {
            touchColliders[i].gameObject.SetActive(false);
        }
    }
    
}
