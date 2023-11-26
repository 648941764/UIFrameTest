using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonMono<CameraController>
{
    [SerializeField] Transform _leftPoint;
    [SerializeField] Transform _rightPoint;
    [SerializeField] private Transform _player;
    private float _smoothing = 3f;
    private Camera _cam;
    Vector3 _bottomLeft;
    Vector3 _topRight;
    Vector3 _lastPlayerPos;
    bool _canFollow;

    private float left, right;

    public float Left => left;
    public float Right => right;

    private void Awake()
    {
        _lastPlayerPos = transform.position;
        _cam = GetComponent<Camera>();
        CalculateMoveRange();
    }

    private void LateUpdate()
    {
        //if (_player != null)
        //{
        //    _canFollow = true;
        //    Vector3 playerPos = new Vector3(_player.position.x, transform.position.y, transform.position.z);
        //    if (_bottomLeft.x <= _leftPoint.position.x && _player.position.x < transform.position.x || _topRight.x >= _rightPoint.position.x && _player.position.x > transform.position.x)
        //    {
        //        _canFollow = false;
        //    }

        //    if (_canFollow)
        //    {
        //        if (transform.position != playerPos)
        //        {
        //            transform.position = Vector3.Lerp(transform.position, playerPos, _smoothing * Time.deltaTime);
        //        }
        //    }

        //}
    }

    private void Update()
    {
        //_bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
        //_topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, _cam.nearClipPlane));
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    //Debug.Log("相机最右边的那个点：" + _topRight.x);
        //    //Debug.Log("相机最左边那个点：" + _bottomLeft.x);
        //    //Debug.Log("右边边界点：" + _rightPoint.position.x);
        //    //Debug.Log("相机的高度：" + _cam.orthographicSize * 2f);
        //    //Debug.Log("相机的宽度：" + 10 * _cam.aspect);
        //    Debug.Log("相机左下角：" + _bottomLeft);
        //    GetCameraFourCorn();
        //}
    }

    public void GetCameraCorns()
    {
        Vector3 bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
        Vector3 topLeft = _cam.ViewportToWorldPoint(new Vector3(0, 1, _cam.nearClipPlane));
        Vector3 topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, _cam.nearClipPlane));
        Vector3 bottomRight = _cam.ViewportToWorldPoint(new Vector3(1, 0, _cam.nearClipPlane));

        Debug.Log("Bottom-Left: " + bottomLeft);
        Debug.Log("Top-Left: " + topLeft);
        Debug.Log("Top-Right: " + topRight);
        Debug.Log("Bottom-Right: " + bottomRight);
    }

    public void GetCameraFourCorn()
    {
        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        Vector3 bottomLeft = new Vector3(-height / 2, width / 2, 0f);
     
        Vector3 worldBm = Camera.main.ScreenToWorldPoint(bottomLeft);
        Debug.Log(worldBm);
    }

    public void TestCamerMove()
    {
        Vector3 playerPos = new Vector3(_player.position.x, transform.position.y, transform.position.z);
        Vector3 difference = playerPos - _lastPlayerPos;

        //移动相机前先判断相机是否超过了那两个边界点
        Vector3 nextCameraLeft = new Vector3(_bottomLeft.x + difference.x, transform.position.y);
        Vector3 nextCameraRight = new Vector3(_topRight.x + difference.x, transform.position.y);
        if (nextCameraLeft.x < _leftPoint.position.x || nextCameraRight.x > _rightPoint.position.x)
        {
            //让相机的位置就在这个临界点

        }
        else
        {
            if (transform.position != playerPos)
            {
                transform.position = Vector3.Lerp(transform.position, playerPos, _smoothing * Time.deltaTime);
            }
        }
        _lastPlayerPos = playerPos;

    }

    public void CalculateMoveRange()
    {
        Vector3 left = GameObject.Find("CameraStartPoint").transform.position;
        Vector3 right = GameObject.Find("CameraEndCamera").transform.position;

        float viewportWithHalf = _cam.orthographicSize * Screen.width / Screen.height;

        Vector3 pos = _cam.transform.position;
        pos.y = 0f;
        _cam.transform.position = pos;

        this.left = left.x + viewportWithHalf;
        this.right = right.x - viewportWithHalf;
    }

    public void Focus(Vector3 point)
    {
        point.y = 0f;
        point.x = Mathf.Clamp(point.x, left, right);
        _cam.transform.position = point;
    }

    public void FocusTo(Vector3 diff)
    {
        Vector3 pos = _cam.transform.position + diff;
        Focus(pos);
    }

    public void FocusPlayer()
    {
        float xPos = _player.position.x;
        if (xPos < left || xPos > right)
        {
            return;
        }
        Vector3 pos = _cam.transform.position;
        pos.x = _player.position.x;
        _cam.transform.position = pos;
    }
}
