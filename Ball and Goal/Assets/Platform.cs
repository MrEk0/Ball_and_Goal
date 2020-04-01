using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Ball ball;

    private float _currentSize;

    private void Awake()
    {
        _currentSize = transform.localScale.x;
    }

    private void OnEnable()
    {
        ball.onBulletFormed += ResizePlatformWidth;
    }

    private void OnDisable()
    {
        ball.onBulletFormed -= ResizePlatformWidth;
    }

    private void ResizePlatformWidth()
    {
        _currentSize -= Time.deltaTime;
        transform.localScale = new Vector3(_currentSize, transform.localScale.y, transform.localScale.z);
    }
}
