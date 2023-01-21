using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Material outlinePixelMat;
    [SerializeField] private Material spriteDefaultMat;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minWidth, maxWidth;
    [SerializeField] private float minHeight, maxHeight;
    [SerializeField] private float minCamSize, maxCamSize;
    [SerializeField] private float startAmount = 0.7f;
    [SerializeField] private float startDuration = 0.5f;

    private PopupPanel _popupPanel;
    private TowerPlacement _towerPlacement;
    private TowerAi _target;
    private float _currMinWidth, _currMaxWidth;
    private float _currMinHeight, _currMaxHeight;
    private float _zoom;

    private Vector3 originalPos;
    private float shakeAmount = 0;
    private float shakeDuration = 0;

    private void Start()
    {
        _popupPanel = GameManager.instance.popupPanel;
        _towerPlacement = GameManager.instance.gameObject.GetComponent<TowerPlacement>();
        _zoom = Camera.main.orthographicSize;
        _currMinHeight = _zoom - minHeight;
        _currMaxHeight = _zoom + maxHeight;
        _currMinWidth = _zoom - minWidth;
        _currMaxWidth = _zoom + maxWidth;
        originalPos = transform.localPosition;
    }
    void Update()
    {
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

        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
            transform.localPosition = originalPos;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 7);

        if (hit.collider != null && !_towerPlacement.isBuilding)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_target != null)
                {
                    _target.OnDeselectTarget();
                    _popupPanel.gameObject.SetActive(false);
                }    
                _target = hit.transform.GetComponent<TowerAi>();
                _target.OnSelectTarget();
                _popupPanel.PopulateInfo(_target);
                _popupPanel.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _target != null)
        {
            DeselectTarget();
        }
    }

    public void DeselectTarget()
    {
        _popupPanel.gameObject.SetActive(false);
        _target.OnDeselectTarget();
        _target = null;
    }

    public void TriggerShake()
    {
        shakeAmount = startAmount;
        shakeDuration = startDuration;
    }
}
