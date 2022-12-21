using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
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
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                rend.color = ghostColor;
                currTowerGhost.transform.position = hit.collider.transform.position;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    rend.color = Color.white;
                    Destroy(hit.collider.GetComponent<BoxCollider2D>());
                    currTowerGhost = null;
                    isBuilding = false;
                }
            }
            else
            {
                rend.color = noBuildingColor;
                currTowerGhost.transform.position = mousePos;
            }
        }
        else
        {
            if (currTowerGhost != null)
                Destroy(currTowerGhost);
        }
    }
}
