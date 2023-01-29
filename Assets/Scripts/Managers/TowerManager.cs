using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerManager : MonoBehaviour
{
    [HideInInspector] public bool isBuilding;

    [SerializeField] private TowerTemplate[] towers;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Transform shopUi;
    [SerializeField] private GameObject towerButtonPrefab;
    [SerializeField] private Tilemap nodesTilemap;
    [SerializeField] private LayerMask buildingLayers;
    [SerializeField] private Color noBuildingColor;
    [SerializeField] private Color ghostColor;

    private Material pixelOutlineMat;
    private Material defaultSpriteMat;
    private PopupPanel _popupPanel;
    private TowerAi _currSelectedTower;


    private GameManager _gameManager;
    private AudioManager _audioManager;
    private TowerTemplate _currentTowerTemplate;
    private TowerAi _currTowerGhost;
    private SpriteRenderer _currTowerRend;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _popupPanel = _gameManager.popupPanel;
        pixelOutlineMat = _gameManager.pixelOutlineMat;
        defaultSpriteMat = _gameManager.defaultSpriteMat;
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

        if (Input.GetKeyDown(KeyCode.Escape) && _currSelectedTower != null)
        {
            DeselectTarget();
        }

        #region Accessing cell position based on mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buildingLayers);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cellPos = nodesTilemap.WorldToCell(mousePos);
        #endregion

        #region Building
        if (isBuilding)
        {
            //Creating ghost tower if one does not exist
            if (_currTowerGhost == null)
            {
                _currTowerGhost = Instantiate(towerPrefab, transform.position, Quaternion.identity).GetComponent<TowerAi>();
                _currTowerGhost.PopulateInfo(_currentTowerTemplate);
                _currTowerRend = _currTowerGhost.GetComponent<SpriteRenderer>();
                _currTowerGhost.ShowRangeIndicator();
            }
            
            _currTowerGhost.transform.position = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0);

            //If current raycast does not return any collider assosiated with our nodes tileset, set different color to our tower ghost
            if (hit.collider == null)
            {
                _currTowerRend.color = noBuildingColor;
                return;
            }

            //Checks if current cell is not occupied by other tower
            if (_gameManager.CheckCellState(cellPos) != null)
            {
                _currTowerRend.color = noBuildingColor;
                return;
            }

            _currTowerRend.color = ghostColor;

            #region Building Tower
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _currTowerRend.color = Color.white;
                _gameManager.money -= _currentTowerTemplate.towerLevels[0].cost;

                TowerCell occupiedCell = new TowerCell();
                occupiedCell.tower = _currTowerGhost;
                occupiedCell.cellPos = cellPos;
                _gameManager.towerCells.Add(occupiedCell);

                _currTowerGhost.isBuilding = false;
                _currTowerGhost.GetComponent<BoxCollider2D>().enabled = true;
                _currTowerGhost.HideRangeIndicator();
                _currTowerGhost = null;
                _audioManager.Play("build");
                isBuilding = false;
                return;
            }
            #endregion
        }
        else
        {
            //Delete tower ghost if one exists and the building is cancelled
            if (_currTowerGhost != null)
            {
                _currTowerGhost.HideRangeIndicator();
                Destroy(_currTowerGhost.gameObject);
                _currTowerGhost = null;
                _currTowerRend = null;
            }
        }
        #endregion

        #region Selecting Tower
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TowerAi _towerInCell = _gameManager.CheckCellState(cellPos);
            if (_towerInCell == null)
                return;
    
            if (_towerInCell != _currSelectedTower)
            {
                if (_currSelectedTower != null)
                {
                    _currSelectedTower.OnDeselectTarget();
                    _popupPanel.gameObject.SetActive(false);
                }
            }
            _currSelectedTower = _towerInCell;

            _currSelectedTower.OnSelectTarget();
            _popupPanel.PopulateInfo(_currSelectedTower);
            _popupPanel.gameObject.SetActive(true);
        }
        #endregion
    }

    public void DeselectTarget()
    {
        _popupPanel.gameObject.SetActive(false);
        _currSelectedTower.OnDeselectTarget();
        _currSelectedTower = null;
    }

    public void BuildTower(TowerTemplate towerTemplate)
    {
        if (towerTemplate.towerLevels[0].cost > _gameManager.money)
            return;
        _audioManager.Play("select");
        _currentTowerTemplate = towerTemplate;
        isBuilding = true;
    }
}
