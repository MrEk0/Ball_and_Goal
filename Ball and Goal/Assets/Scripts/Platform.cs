using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Ball _ball;

    private float _currentSize;
    private float _yScale;
    private float _zScale;

    private void Awake()
    {
        _currentSize = transform.localScale.x;
        _yScale = transform.localScale.y;
        _zScale = transform.localScale.z;
    }

    private void OnEnable()
    {
        _ball.onBulletFormed += ResizePlatformWidth;
    }

    private void OnDisable()
    {
        _ball.onBulletFormed -= ResizePlatformWidth;
    }

    private void ResizePlatformWidth()
    {
        _currentSize -= Time.deltaTime;
        transform.localScale = new Vector3(_currentSize, _yScale, _zScale);
    }
}
