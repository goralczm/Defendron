using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private TowerTemplate[] towers;
    [SerializeField] private Transform shopUi;
    [SerializeField] private GameObject towerButtonPrefab;
    [SerializeField] private Tilemap nodesTilemap;
    [SerializeField] private LayerMask buildingLayers;
    [SerializeField] private Color noBuildingColor;
    [SerializeField] private Color ghostColor;

    private GameManager _gameManager;
    private TowerTemplate _currentTower;
    private GameObject _currTowerGhost;
    private Transform _currTowerTarget;
    private bool _isBuilding;
    private bool _isDeleting;

    private void Start()
    {
        _gameManager = GameManager.instance;
        foreach (TowerTemplate tower in towers)
        {
            GameObject tmp = Instantiate(towerButtonPrefab, shopUi);
            tmp.GetComponent<TowerButton>().Setup(tower);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isBuilding = false;
            _isDeleting = false;
        }

        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D towerHit = Physics2D.Raycast(raycast.origin, raycast.direction, Mathf.Infinity, 1 << 7);
        if (towerHit.collider != null)

        #region Building
        if (_isBuilding)
        {
            if (_currTowerGhost == null)
            {
                _currTowerGhost = _currentTower.CreateTower();
            }
            SpriteRenderer rend = _currTowerGhost.GetComponent<SpriteRenderer>();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buildingLayers);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPos = nodesTilemap.WorldToCell(mousePos);
            _currTowerGhost.transform.position = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0);

            if (hit.collider != null && hit.collider.tag == "Ground")
            {
                rend.color = ghostColor;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _gameManager.money -= _currentTower.towerLevels[0].cost;
                    rend.color = Color.white;
                    _currTowerGhost.GetComponent<TowerAi>().enabled = true;
                    _currTowerGhost.GetComponent<BoxCollider2D>().enabled = true;
                    _currTowerGhost = null;
                    _isBuilding = false;
                }
            }
            else
                rend.color = noBuildingColor;
        }
        #endregion
        else
        {
            if (_currTowerGhost != null)
                Destroy(_currTowerGhost);

            if (Input.GetKeyDown(KeyCode.B))
                _isDeleting = !_isDeleting;

            if (towerHit.collider != null)
            {
                if (_isDeleting)
                {
                    if (_currTowerTarget != null && towerHit.transform != _currTowerTarget)
                        _currTowerTarget.GetComponent<SpriteRenderer>().color = Color.white;
                    _currTowerTarget = towerHit.transform;
                    _currTowerTarget.GetComponent<SpriteRenderer>().color = noBuildingColor;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        _gameManager.money += towerHit.collider.GetComponent<TowerAi>().towerTemplate.towerLevels[0].cost / 2;
                        Destroy(towerHit.collider.gameObject);
                    }
                }
                else
                {
                    if (_currTowerTarget != null)
                    {
                        _currTowerTarget.GetComponent<SpriteRenderer>().color = Color.white;
                        _currTowerTarget = null;
                    }
                }
            }
            else
            {
                if (_currTowerTarget != null)
                {
                    _currTowerTarget.GetComponent<SpriteRenderer>().color = Color.white;
                    _currTowerTarget = null;
                }
            }
        }
    }

    public void BuildTower(TowerTemplate towerTemplate)
    {
        if (towerTemplate.towerLevels[0].cost > _gameManager.money)
            return;
        _isBuilding = true;
        _currentTower = towerTemplate;
    }
}
