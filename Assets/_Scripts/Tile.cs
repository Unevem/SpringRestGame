using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool isOccupied;
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public bool IsOccupied => isOccupied;

    public void SetIsOccupied(bool newOccupiedState)
    {
        isOccupied = newOccupiedState;
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _highlight.SetActive(false); // Garante que comece desativado
    }

    void OnMouseEnter()
    {
        if (!isOccupied) // Mostra highlight apenas se não estiver ocupada
        {
            _highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}