using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] Aim aim;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float minBallSize = 1f;
    [SerializeField] float minDistanceToObstacle = 10f;
    float castDistance = Mathf.Infinity;
    [SerializeField] float speed = 10f;

    GameObject bullet;
    Rigidbody rb;
    Vector3 target;
    float currentBallSize;
    float bulletSize;
    float initialBulletSize;

    public bool CanFire { get; set; } = true;

    public event Action onBulletFormed;

    private void Awake()
    {
        currentBallSize = transform.localScale.x;
        rb = GetComponent<Rigidbody>();
        target = transform.position;
        //aim.FollowBall(transform);

        InstantiateBullet();
    }

    private void InstantiateBullet()
    {
        bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().ball = this;
        initialBulletSize = bullet.transform.localScale.x;
        bulletSize = initialBulletSize;
        bullet.SetActive(false);
    }

    private void Start()
    {
        aim.FollowBall(transform);
        FindAWay();
    }


    void Update()
    {
        if(!IsReachedTarget() || !CanFire)
        {
            MoveTheBall();
            return;
        }
        

        if (Input.GetMouseButton(0))
        {
            aim.IncreaseImage();
            ResizeBall();
        }

        if(Input.GetMouseButtonUp(0))
        {
            Shot();
            aim.ResizeImage();
        }
    }

    private bool IsReachedTarget()
    {
        float distance = Vector3.SqrMagnitude(target - transform.position);
        if(Mathf.Approximately(distance, 0f))
        {
            return true;
        }

        return false;
    }

    public void FindAWay()
    {
        RaycastHit hit;
        bool hasTouched=Physics.SphereCast(transform.position, transform.localScale.x/2, transform.forward,
            out hit, castDistance, obstacleMask);

        if (hasTouched)
        {
            float distanceToObstacle = hit.distance;

            if (distanceToObstacle > minDistanceToObstacle)
            {
                float distanceToGo = distanceToObstacle - minDistanceToObstacle;
                target = new Vector3(transform.position.x, transform.position.y, transform.position.z + distanceToGo);
            }

            if (hit.transform.GetComponent<Goal>())
            {
                target = hit.transform.position;
            }
        }
    }

    private void MoveTheBall()
    {
        Vector3 newPosition= Vector3.MoveTowards(transform.position, target, speed*Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        aim.FollowBall(transform);
    }


    private void ResizeBall()
    {
        bulletSize += Time.deltaTime;
        currentBallSize -= Time.deltaTime;

        if (currentBallSize<minBallSize)
        {
            gameOverPanel.SetActive(true);
            CanFire = false;
        }

        onBulletFormed();
        transform.localScale = new Vector3(currentBallSize, currentBallSize, currentBallSize);
    }

    private void Shot()
    {
        bullet.transform.position = transform.position;
        bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        bullet.SetActive(true);

        CanFire = false;
        bulletSize = initialBulletSize;
    }
}
