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

    [HideInInspector] public bool isBuilding;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    private TowerTemplate _currentTower;
    private TowerAi _currTowerGhost;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        foreach (TowerTemplate tower in towers)
        {
            GameObject tmp = Instantiate(towerButtonPrefab, shopUi);
            tmp.GetComponent<TowerButton>().Setup(tower);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isBuilding = false;

        #region Building
        if (isBuilding)
        {
            if (_currTowerGhost == null)
            {
                _currTowerGhost = _currentTower.CreateTower().GetComponent<TowerAi>();
                _currTowerGhost.ShowRangeIndicator();
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
                    _currTowerGhost.HideRangeIndicator();
                    _currTowerGhost.GetComponent<TowerAi>().isBuilding = false;
                    _currTowerGhost.GetComponent<BoxCollider2D>().enabled = true;
                    _currTowerGhost = null;
                    isBuilding = false;
                    _audioManager.Play("build");
                }
            }
            else
                rend.color = noBuildingColor;
        }
        #endregion
    }

    public void BuildTower(TowerTemplate towerTemplate)
    {
        if (towerTemplate.towerLevels[0].cost > _gameManager.money)
            return;
        _audioManager.Play("select");
        isBuilding = true;
        _currentTower = towerTemplate;
    }
}
