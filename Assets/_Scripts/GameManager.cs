using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ResourceType { Wood, Stone, People }

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text resourceDisplay; // (Você pode expandir depois pra exibir vários)
    [SerializeField] private GameObject resourceTextPrefab;
    [SerializeField] private BuildCursor buildCursor;
    [SerializeField] private Tile[] tiles;

    // Novo sistema de recursos
    [SerializeField] private Dictionary<ResourceType, int> resources = new();

    private Building buildingToPlace;
    private GameObject spawnedResourceText;

    private void Start()
    {
        // Inicializa os recursos com valores iniciais
        resources[ResourceType.Wood] = 100;
        resources[ResourceType.Stone] = 80;
        resources[ResourceType.People] = 10;

        if (resourceDisplay == null && resourceTextPrefab != null)
        {
            spawnedResourceText = Instantiate(resourceTextPrefab);
            resourceDisplay = spawnedResourceText.GetComponent<TMP_Text>();

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

        // Exemplo básico de exibição (você pode separar por TMP_Texts depois)
        if (resourceDisplay != null)
        {
            resourceDisplay.text = $"Wood: {GetResource(ResourceType.Wood)}\n" +
                                   $"Stone: {GetResource(ResourceType.Stone)}\n" +
                                   $"People: {GetResource(ResourceType.People)}";
        }
    }

    public void BuyBuilding(Building building)
    {
        bool canAfford = true;

        foreach (var cost in building.Costs)
        {
            if (GetResource(cost.type) < cost.amount)
            {
                canAfford = false;
                break;
            }
        }

        if (canAfford)
        {
            foreach (var cost in building.Costs)
            {
                SpendResource(cost.type, cost.amount);
            }

            buildCursor.gameObject.SetActive(true);
            buildCursor.GetComponent<SpriteRenderer>().sprite = building.GetComponent<SpriteRenderer>().sprite;
            Cursor.visible = false;

            buildingToPlace = building;
        }
    }

    public int GetResource(ResourceType type) =>
        resources.TryGetValue(type, out int amount) ? amount : 0;

    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (GetResource(type) < amount) return false;

        resources[type] -= amount;
        return true;
    }
}
