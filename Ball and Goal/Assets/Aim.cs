using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    [SerializeField] float growSpeed = 50f;
    [SerializeField] Vector3 aimOffset;

    float newAimSize;
    Vector2 aimStartSize;
    RectTransform rectTransform;
    Camera mainCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        aimStartSize = rectTransform.sizeDelta;
        newAimSize = aimStartSize.x;
        mainCamera = Camera.main;
    }

    public void IncreaseImage()
    {
        newAimSize += Time.deltaTime * growSpeed;
        rectTransform.sizeDelta = new Vector2(newAimSize, newAimSize);
    }

    public void FollowBall(Transform ballTransform)
    {
        Vector2 pointToMove = mainCamera.WorldToScreenPoint(ballTransform.position + /*new Vector3(0f, 0.5f, 0f)*/aimOffset);
        rectTransform.position = pointToMove;
    }

    public void ResizeImage()
    {
        rectTransform.sizeDelta = aimStartSize;
        newAimSize = aimStartSize.x;
    }
}
