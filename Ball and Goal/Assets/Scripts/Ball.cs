using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Aim _aim;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] LayerMask _obstacleMask;
    [SerializeField] float _minBallSize = 1f;
    [SerializeField] float _minDistanceToObstacle = 10f;
    [SerializeField] float _speed = 10f;

    private GameObject _bullet;
    private Transform _thisTransform;
    private Rigidbody _rb;
    private Vector3 _target;
    Vector3 _newBallPosition;

    private float _castDistance = Mathf.Infinity;
    private float _currentBallSize;
    private float _bulletSize;
    private float _initialBulletSize;

    public bool CanFire { get; set; } = true;

    public event Action onBulletFormed;

    private void Awake()
    {
        _thisTransform = transform;
        _currentBallSize = _thisTransform.localScale.x;
        _rb = GetComponent<Rigidbody>();
        _target = _thisTransform.position;

        InstantiateBullet();
    }

    private void InstantiateBullet()
    {
        _bullet = Instantiate(_bulletPrefab, _thisTransform.position, _thisTransform.rotation);
        _bullet.GetComponent<Bullet>().Ball = this;
        _initialBulletSize = _bullet.transform.localScale.x;
        _bulletSize = _initialBulletSize;
        _bullet.SetActive(false);
    }

    private void Start()
    {
        _aim.FollowBall(_thisTransform);
        FindAWay();
    }


    void Update()
    {
        if (!IsReachedTarget() || !CanFire)
        {
            return;
        }

        MouseClickBehaviour();

        TouchBehaviour();
    }

    private void MouseClickBehaviour()
    {
        if (Input.GetMouseButton(0))
        {
            _aim.IncreaseImageSize();
            ResizeBall();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shot();
            _aim.ResizeBackImage();
        }
    }

    private void TouchBehaviour()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _aim.IncreaseImageSize();
                    ResizeBall();
                    break;
                case TouchPhase.Ended:
                    Shot();
                    _aim.ResizeBackImage();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveTheBall();
    }

    private bool IsReachedTarget()
    {
        float distance = Vector3.SqrMagnitude(_target - _thisTransform.position);
        if(Mathf.Approximately(distance, 0f))
        {
            return true;
        }

        return false;
    }

    public void FindAWay()
    {
        RaycastHit hit;
        bool hasTouched=Physics.SphereCast(_thisTransform.position, _currentBallSize*0.5f, _thisTransform.forward,
            out hit, _castDistance, _obstacleMask);

        if (hasTouched)
        {
            float distanceToObstacle = hit.distance;

            if (distanceToObstacle > _minDistanceToObstacle)
            {
                float distanceToGo = distanceToObstacle - _minDistanceToObstacle;
                _target.z += distanceToGo;
            }

            if (hit.transform.GetComponent<Goal>())
            {
                _target.z = hit.transform.position.z;
            }
        }
    }

    private void MoveTheBall()
    {
        _newBallPosition = Vector3.MoveTowards(_thisTransform.position, _target, _speed * Time.fixedDeltaTime);
        _rb.MovePosition(_newBallPosition);
        _aim.FollowBall(_thisTransform);
    }


    private void ResizeBall()
    {
        _bulletSize += Time.deltaTime;
        _currentBallSize -= Time.deltaTime;

        if (_currentBallSize<_minBallSize)
        {
            _gameOverPanel.SetActive(true);
            CanFire = false;
            return;
        }

        onBulletFormed();
        _thisTransform.localScale = new Vector3(_currentBallSize, _currentBallSize, _currentBallSize);
        _bullet.transform.localScale = new Vector3(_bulletSize, _bulletSize, _bulletSize);
    }

    private void Shot()
    {
        _bullet.transform.position = _thisTransform.position;
        _bullet.SetActive(true);

        CanFire = false;
        _bulletSize = _initialBulletSize;
    }
}
