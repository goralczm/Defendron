using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Camera Movement
    /*[SerializeField] private float cameraSpeed;
    [SerializeField] private float minWidth, maxWidth;
    [SerializeField] private float minHeight, maxHeight;
    [SerializeField] private float minCamSize, maxCamSize;

    private float _currMinWidth, _currMaxWidth;
    private float _currMinHeight, _currMaxHeight;
    private float _zoom;*/
    #endregion

    [SerializeField] private float startAmount = 0.7f;
    [SerializeField] private float startDuration = 0.5f;

    private Transform _target;
    private Vector3 _originalPos;
    private Vector3 _originalParentPos;
    private float _shakeAmount = 0;
    private float _shakeDuration = 0;

    private void Start()
    {
        #region Camera Movement
        /*_zoom = Camera.main.orthographicSize;
        _currMinHeight = _zoom - minHeight;
        _currMaxHeight = _zoom + maxHeight;
        _currMinWidth = _zoom - minWidth;
        _currMaxWidth = _zoom + maxWidth;*/
        #endregion
        _originalPos = transform.localPosition;
        _originalParentPos = transform.parent.transform.position;
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

        Vector3 targetPos = _target != null ? _target.position : _originalPos;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 10f);

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
    }

    public void DefocusTarget()
    {
        _target = null;
    }
}
