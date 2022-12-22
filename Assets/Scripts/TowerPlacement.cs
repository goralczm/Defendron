using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject towerPrefab;
    public Color noBuildingColor;
    public Color ghostColor;

    private GameObject currTowerGhost;
    private bool isBuilding;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            isBuilding = !isBuilding;

        if (isBuilding)
        {
            if (currTowerGhost == null)
                currTowerGhost = Instantiate(towerPrefab);
            SpriteRenderer rend = currTowerGhost.GetComponent<SpriteRenderer>();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << 6);

            Vector3 cellPos = tilemap.WorldToCell(mousePos);
            currTowerGhost.transform.position = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0);

            if (hit.collider != null)
            {
                rend.color = ghostColor;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    rend.color = Color.white;
                    Destroy(hit.collider.GetComponent<BoxCollider2D>());
                    currTowerGhost.GetComponent<Tower>().enabled = true;
                    currTowerGhost = null;
                    isBuilding = false;
                }
            }
            else
                rend.color = noBuildingColor;
        }
        else
        {
            if (currTowerGhost != null)
                Destroy(currTowerGhost);
        }
    }
}
