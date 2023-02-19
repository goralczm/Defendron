using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera Movement
    /*[SerializeField] private float cameraSpeed;
    [SerializeField] private float minWidth, maxWidth;
    [SerializeField] private float minHeight, maxHeight;
    [SerializeField] private float minCamSize, maxCamSize;

    private float _currMinWidth, _currMaxWidth;
    private float _currMinHeight, _currMaxHeight;*/
    #endregion

    [SerializeField] private float startAmount = 0.7f;
    [SerializeField] private float startDuration = 0.5f;
    [SerializeField] private float minZoom;

    private Transform _target;
    private Vector3 _originalPos;
    private Vector3 _originalParentPos;
    private float _shakeAmount = 0;
    private float _shakeDuration = 0;
    private float _startZoom;
    private float _zoom;
    private bool _canShake;

    private void Start()
    {
        #region Camera Movement
        /*_zoom = Camera.main.orthographicSize;
        _currMinHeight = _zoom - minHeight;
        _currMaxHeight = _zoom + maxHeight;
        _currMinWidth = _zoom - minWidth;
        _currMaxWidth = _zoom + maxWidth;*/
        #endregion
        _originalPos = transform.position;
        _originalParentPos = transform.parent.transform.position;
        _startZoom = Camera.main.orthographicSize;
        _zoom = _startZoom;
        _canShake = true;
    }
    void Update()
    {
        #region Camera Movement
        /*if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float scroll = Input.mouseScrollDelta.y;
            _zoom -= scroll;
            _zoom = Mathf.Clamp(_zoom, minCamSize, maxCamSize);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _zoom, Time.deltaTime * cameraSpeed);

            Vector3 targetPos = new Vector3(Mathf.Clamp(transform.position.x + horizontal, _zoom - _currMinWidth, _currMaxWidth - _zoom), Mathf.Clamp(transform.position.y + vertical, _zoom - _currMinHeight, _currMaxHeight - _zoom), -10);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
        }*/
        #endregion

        //Position
        Vector3 targetPos = _target != null ? new Vector3(_target.position.x, _target.position.y, -10) : _originalPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 7f);

        //Rotation
        Quaternion targetRot = _target != null ? Quaternion.Euler(0, 0, -15f) : Quaternion.identity;
        transform.rotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * 7f);

        //Zoom
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _zoom, Time.deltaTime * 15f);

        //Shake
        if (!_canShake)
        {
            _shakeDuration = 0;
            return;
        }

        if (_shakeDuration > 0)
        {
            transform.parent.transform.position = _originalParentPos + Random.insideUnitSphere * _shakeAmount;
            _shakeDuration -= Time.deltaTime;
        }
        else
            transform.parent.transform.position = _originalParentPos;
    }

    public void TriggerShake()
    {
        _shakeAmount = startAmount;
        _shakeDuration = startDuration;
    }

    public void FocusOnTarget(Transform newTarget)
    {
        _target = newTarget;
        _zoom = minZoom;
        _canShake = false;
    }

    public void DefocusTarget()
    {
        _target = null;
        _zoom = _startZoom;
        _canShake = true;
    }
}
