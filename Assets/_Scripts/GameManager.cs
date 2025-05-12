using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float resource;
    [SerializeField] private TMP_Text ResourceDisplay; // Substitui o Text padrão pelo TMP_Text
    [SerializeField] private GameObject resourceTextPrefab; // Prefab do TMP_Text (opcional)

    [SerializeField] private BuildCursor buildCursor;
    [SerializeField] private Tile[] tiles;

    public float Resource {
        get => resource;
        set => resource = value;
    }
    
    private Building buildingToPlace;
    private GameObject spawnedResourceText; // Instância do texto (usada se instanciar dinamicamente)

    private void Start()
    {
        // Instancia o texto dinamicamente se não estiver na cena
        if (ResourceDisplay == null && resourceTextPrefab != null)
        {
            spawnedResourceText = Instantiate(resourceTextPrefab);
            ResourceDisplay = spawnedResourceText.GetComponent<TMP_Text>();

            // Garante que o texto seja filho do Canvas
            if (GameObject.Find("Canvas") != null)
            {
                spawnedResourceText.transform.SetParent(GameObject.Find("Canvas").transform);
            }
        }
    }

private void Update()
{
    if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gridPosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        
        Tile tile = FindObjectOfType<GridManager>().GetTileAtPosition(gridPosition);
        
        if (tile != null && !tile.IsOccupied)
        {
            Instantiate(buildingToPlace, tile.transform.position, Quaternion.identity);
            buildingToPlace = null;
            tile.SetIsOccupied(true);
            buildCursor.gameObject.SetActive(false);
            Cursor.visible = true;
        }
    }
}

    public void BuyBuilding(Building building)
    {
        if (resource >= building.Cost)
        {
            buildCursor.gameObject.SetActive(true);
            buildCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            resource -= building.Cost;
            buildingToPlace = building;
        }
    }
}