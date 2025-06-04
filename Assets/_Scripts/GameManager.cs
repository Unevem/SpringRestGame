using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float resource;
    [SerializeField] private TMP_Text ResourceDisplay;
    [SerializeField] private GameObject resourceTextPrefab;

    [SerializeField] private BuildCursor buildCursor;
    [SerializeField] private Tile[] tiles;

    public GameOverScreen gameOverScreen;

    [SerializeField] private GameObject CanvasMenu;
    [SerializeField] private GameObject CanvasConstruções;

    private float totalRecursosGastos = 0f;
    private int totalConstrucoesFeitas = 0; // ✅ NOVO: contador de construções feitas

    public float Resource
    {
        get => resource;
        set => resource = value;
    }

    private Building buildingToPlace;
    private GameObject spawnedResourceText;

    private void Start()
    {
        if (ResourceDisplay == null && resourceTextPrefab != null)
        {
            spawnedResourceText = Instantiate(resourceTextPrefab);
            ResourceDisplay = spawnedResourceText.GetComponent<TMP_Text>();

            if (GameObject.Find("Canvas") != null)
            {
                spawnedResourceText.transform.SetParent(GameObject.Find("Canvas").transform);
            }
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.gameObject.SetActive(false);
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

                totalConstrucoesFeitas++; // ✅ Conta cada construção feita
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GameOver();
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
            totalRecursosGastos += building.Cost;

            buildingToPlace = building;
        }
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.gameObject.SetActive(true);
            gameOverScreen.Setup(totalRecursosGastos, totalConstrucoesFeitas); // ✅ Passa também as construções feitas
        }

        if (CanvasMenu != null) CanvasMenu.SetActive(false);
        if (CanvasConstruções != null) CanvasConstruções.SetActive(false);
    }
}
