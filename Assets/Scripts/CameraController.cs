using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minWidth, maxWidth;
    [SerializeField] private float minHeight, maxHeight;
    [SerializeField] private float minCamSize, maxCamSize;

    private TowerPlacement _towerPlacement;
    private TowerAi _target;
    private float _currMinWidth, _currMaxWidth;
    private float _currMinHeight, _currMaxHeight;
    private float _zoom;

    private void Start()
    {
        _towerPlacement = GameManager.instance.gameObject.GetComponent<TowerPlacement>();
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
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _zoom, Time.deltaTime * cameraSpeed);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 7);

        if (hit.collider != null && !_towerPlacement.isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_target != null)
                {
                    _target.HideRangeIndicator();
                }    
                _target = hit.transform.GetComponent<TowerAi>();
                _target.ShowRangeIndicator();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _target != null)
        {
            _target.HideRangeIndicator();
            _target = null;
        }

        Vector3 targetPos = new Vector3();
        if (_target == null)
            targetPos = new Vector3(Mathf.Clamp(transform.position.x + horizontal, _zoom - _currMinWidth, _currMaxWidth - _zoom), Mathf.Clamp(transform.position.y + vertical, _zoom - _currMinHeight, _currMaxHeight - _zoom), -10);
        else
            targetPos = new Vector3(_target.transform.position.x, _target.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraSpeed);
    }
}
