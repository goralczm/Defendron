using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minWidth, maxWidth;
    [SerializeField] private float minHeight, maxHeight;
    [SerializeField] private float minCamSize, maxCamSize;

    private float _currMinWidth, _currMaxWidth;
    private float _currMinHeight, _currMaxHeight;
    private float _zoom;

    private void Start()
    {
        _zoom = Camera.main.orthographicSize;
        _currMinHeight = _zoom - minHeight;
        _currMaxHeight = _zoom + maxHeight;
        _currMinWidth = _zoom - minWidth;
        _currMaxWidth = _zoom + maxWidth;
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float scroll = Input.mouseScrollDelta.y;
        _zoom -= scroll;
        _zoom = Mathf.Clamp(_zoom, minCamSize, maxCamSize);
        Camera.main.orthographicSize = _zoom;

        Vector3 targetPos = new Vector3(Mathf.Clamp(transform.position.x + horizontal, _zoom - _currMinWidth, _currMaxWidth - _zoom), Mathf.Clamp(transform.position.y + vertical, _zoom - _currMinHeight, _currMaxHeight - _zoom), -10);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * cameraSpeed);
    }
}