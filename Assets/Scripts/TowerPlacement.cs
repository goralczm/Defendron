using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacement : MonoBehaviour
{
    public Tilemap tilemap;
    public TowerTemplate tower;
    public Color noBuildingColor;
    public Color ghostColor;

    private GameObject currTowerGhost;
    private Transform currDelTarget;
    private bool isBuilding;
    private bool isDeleting;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (Input.GetKeyDown(KeyCode.Alpha1) && !isDeleting)
            isBuilding = !isBuilding;

        if (isBuilding)
        {
            if (currTowerGhost == null)
            {
                currTowerGhost = tower.CreateTower();
            }
            SpriteRenderer rend = currTowerGhost.GetComponent<SpriteRenderer>();

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 cellPos = tilemap.WorldToCell(mousePos);
            currTowerGhost.transform.position = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f, 0);

            if (hit.collider != null && hit.collider.tag != "Tower" && hit.collider.tag == "Ground")
            {
                rend.color = ghostColor;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    rend.color = Color.white;
                    currTowerGhost.GetComponent<TowerAi>().enabled = true;
                    currTowerGhost.GetComponent<BoxCollider2D>().enabled = true;
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

            if (Input.GetKeyDown(KeyCode.B))
                isDeleting = !isDeleting;

            if (isDeleting)
            {
                if (hit.collider != null && hit.collider.tag == "Tower")
                {
                    if (currDelTarget != null && currDelTarget.transform != hit.transform)
                        currDelTarget.GetComponent<SpriteRenderer>().color = Color.white;
                    currDelTarget = hit.transform;
                    currDelTarget.GetComponent<SpriteRenderer>().color = noBuildingColor;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                        Destroy(hit.collider.gameObject);
                }
                else
                {
                    if (currDelTarget != null)
                    {
                        currDelTarget.GetComponent<SpriteRenderer>().color = Color.white;
                        currDelTarget = null;
                    }
                }
            }
            else
            {
                if (currDelTarget != null)
                {
                    currDelTarget.GetComponent<SpriteRenderer>().color = Color.white;
                    currDelTarget = null;
                }
            }
        }
    }
}
