using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask _obstacleMask;
    [SerializeField] float _velocity = 20f;

    private Rigidbody _rb;
    public Ball Ball { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector3(0, 0, _velocity);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<BoxCollider>() && !collision.GetComponent<Goal>())
        {
            GiveDamage(collision.transform.position);         
        }

        if (Ball)
        {
            Ball.CanFire = true;
            Ball.FindAWay();
        }

        gameObject.SetActive(false);
    }

    private void GiveDamage(Vector3 initialPosition)
    {
        Collider[] touchColliders=Physics.OverlapSphere(initialPosition, transform.localScale.x, _obstacleMask);
        
        for(int i=0; i<touchColliders.Length; i++)
        {
            touchColliders[i].gameObject.SetActive(false);
        }
    }
    
}
