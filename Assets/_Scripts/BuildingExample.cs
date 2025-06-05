using UnityEngine;

public class Building : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceCost
    {
        public ResourceType type;
        public int amount;
    }

    [SerializeField] private ResourceCost[] costs;
    [SerializeField] private ResourceType incomeType;
    [SerializeField] private int resourceIncrease;
    [SerializeField] private float timeForIncrease;
    [SerializeField] private Vector2 sizeInTiles = Vector2.one;

    public Vector2 SizeInTiles => sizeInTiles;
    public ResourceCost[] Costs => costs;

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
            gm.AddResource(incomeType, resourceIncrease);
        }
    }

}

