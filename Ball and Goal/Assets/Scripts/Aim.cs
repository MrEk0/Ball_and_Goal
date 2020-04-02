using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] float _growSpeed = 50f;
    [SerializeField] Vector3 _aimOffset;

    private float _newAimSize;
    private Vector2 _aimStartSize;
    private RectTransform _rectTransform;
    private Camera _mainCamera;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _aimStartSize = _rectTransform.sizeDelta;
        _newAimSize = _aimStartSize.x;
        _mainCamera = Camera.main;
    }

    public void IncreaseImageSize()
    {
        _newAimSize += Time.deltaTime * _growSpeed;
        _rectTransform.sizeDelta = new Vector2(_newAimSize, _newAimSize);
    }

    public void FollowBall(Transform ballTransform)
    {
        Vector2 pointToMove = _mainCamera.WorldToScreenPoint(ballTransform.position + _aimOffset);
        _rectTransform.position = pointToMove;
    }

    public void ResizeBackImage()
    {
        _rectTransform.sizeDelta = _aimStartSize;
        _newAimSize = _aimStartSize.x;
    }
}
