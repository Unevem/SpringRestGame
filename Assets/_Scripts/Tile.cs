using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private bool isOccupied;
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    // basicamente faz a mesma coisa que a linha de baixo, porem com menos linhas (é uma propriedade [pesquisa ricardo)
    //public bool IsOccupied
    //{
    //    get => isOccupied;
    //    set => isOccupied = value;
    //}

    public bool IsOccupied => isOccupied;

    public void SetIsOccupied(bool newOccupiedState)
    {
        isOccupied = newOccupiedState;
    }

    public void Init(bool isOffset)
    {
        // _renderer.color = isOffset ? _offsetColor : _baseColor; basicamente faz a mesma coisa que a linha de baixo, porem com menos linhas (é um ifternario [pesquisa ricardo)
        if (isOffset)
        {
            _renderer.color = _offsetColor;
        }
        else
        {
            _renderer.color = _baseColor;
        }
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void Update()
    {
        if (isOccupied == true)
        {
            _highlight.SetActive(true);
        }
        else
        {
            _highlight.SetActive(false);
        }
    }


}