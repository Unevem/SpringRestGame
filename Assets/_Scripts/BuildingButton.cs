using UnityEngine;

public class BuildButton : MonoBehaviour
{
    [SerializeField] private Building buildingPrefab;

    public void OnClick()
    {
        FindObjectOfType<GameManager>().BuyBuilding(buildingPrefab);
    }
}
