using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private int ResourceIncrease;
    [SerializeField] private float timeForIncrease;
    [SerializeField] private Vector2 sizeInTiles = Vector2.one;

    public Vector2 SizeInTiles => sizeInTiles;

    public int Cost => cost;

    private float nextIncreaseTime;
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Time.time > nextIncreaseTime)
        {
            nextIncreaseTime = Time.time + timeForIncrease;
            gm.Resource += ResourceIncrease;
        }
    }

    // Adicione este m�todo para detectar cliques
    private void OnMouseDown()
    {
        // Verifica se o GameManager existe e tem o m�todo BuyBuilding
        if (gm != null)
        {
            gm.BuyBuilding(this); // Passa esta pr�pria constru��o como par�metro
        }
    }
}