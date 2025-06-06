using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ResourceType { Wood, Stone, People, Food }

public class GameManager : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [SerializeField] private BuildCursor buildCursor;
    [SerializeField] private GameObject resourceTextPrefab;
    public GameOverScreen gameOverScreen;
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasConstrucoes;

    [Header("UI de Recursos")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text peopleText;
    [SerializeField] private TMP_Text foodText;

    private Dictionary<ResourceType, int> resources = new();
    private float totalRecursosGastos = 0f;
    private int totalConstrucoesFeitas = 0;
    private Building buildingToPlace;

    private void Start()
    {
        // Inicialização dos recursos
        resources[ResourceType.Wood] = 100;
        resources[ResourceType.Stone] = 80;
        resources[ResourceType.People] = 10;
        resources[ResourceType.Food] = 50;

        UpdateAllResourceUI();

        if (gameOverScreen != null)
        {
            gameOverScreen.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HandleBuildingPlacement();
    }

    private void HandleBuildingPlacement()
    {
        if (Input.GetMouseButtonDown(0) && buildingToPlace != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));

            Tile tile = FindObjectOfType<GridManager>().GetTileAtPosition(gridPosition);

            if (tile != null && !tile.IsOccupied)
            {
                PlaceBuilding(tile);
            }
        }
    }

    private void PlaceBuilding(Tile tile)
    {
        Instantiate(buildingToPlace, tile.transform.position, Quaternion.identity);
        AddPeopleFromBuilding(buildingToPlace);

        buildingToPlace = null;
        tile.SetIsOccupied(true);
        buildCursor.gameObject.SetActive(false);
        Cursor.visible = true;

        totalConstrucoesFeitas++;
    }

    public void BuyBuilding(Building building)
    {
        if (CanAffordBuilding(building))
        {
            PurchaseBuilding(building);
        }
    }

    private bool CanAffordBuilding(Building building)
    {
        foreach (var cost in building.Costs)
        {
            if (GetResource(cost.type) < cost.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void PurchaseBuilding(Building building)
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

    // ===== SISTEMA DE RECURSOS ATUALIZADO =====
    private void UpdateAllResourceUI()
    {
        woodText.text = GetResource(ResourceType.Wood).ToString();
        stoneText.text = GetResource(ResourceType.Stone).ToString();
        peopleText.text = GetResource(ResourceType.People).ToString();
        foodText.text = GetResource(ResourceType.Food).ToString();
    }

    private void UpdateResourceUI(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodText.text = GetResource(type).ToString();
                break;
            case ResourceType.Stone:
                stoneText.text = GetResource(type).ToString();
                break;
            case ResourceType.People:
                peopleText.text = GetResource(type).ToString();
                break;
            case ResourceType.Food:
                foodText.text = GetResource(type).ToString();
                break;
        }
    }

    public int GetResource(ResourceType type)
    {
        return resources.TryGetValue(type, out int amount) ? amount : 0;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (!resources.ContainsKey(type))
            resources[type] = 0;

        resources[type] += amount;
        UpdateResourceUI(type);
    }

    public bool SpendResource(ResourceType type, int amount)
    {
        if (GetResource(type) < amount)
            return false;

        resources[type] -= amount;
        totalRecursosGastos += amount;
        UpdateResourceUI(type);
        return true;
    }
    // ===== FIM DO SISTEMA DE RECURSOS =====

    public void AddPeopleFromBuilding(Building building)
    {
        if (building.IncomeType == ResourceType.People)
        {
            AddResource(ResourceType.People, building.ResourceIncrease);
        }
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.gameObject.SetActive(true);
            gameOverScreen.Setup(totalRecursosGastos, totalConstrucoesFeitas);
        }

        canvasMenu?.SetActive(false);
        canvasConstrucoes?.SetActive(false);
    }

    public Dictionary<ResourceType, int> GetTotalResourceIncreasePerType()
    {
        Dictionary<ResourceType, int> totalIncrease = new();

        Building[] allBuildings = FindObjectsOfType<Building>();
        foreach (Building building in allBuildings)
        {
            ResourceType type = building.IncomeType;
            int increase = building.ResourceIncrease;

            if (!totalIncrease.ContainsKey(type))
                totalIncrease[type] = 0;

            totalIncrease[type] += increase;
        }

        return totalIncrease;
    }
}