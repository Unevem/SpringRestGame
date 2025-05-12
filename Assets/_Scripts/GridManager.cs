using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public void ExpandGrid()
    {
        int newWidth = _width + 1;
        int newHeight = _height + 1;

        // ---- Adiciona NOVAS TILES na última COLUNA (x = _width) ----
        for (int y = 0; y < _height; y++)
        {
            CreateTile(_width, y); // Nova coluna (exceto o canto)
        }

        // ---- Adiciona NOVAS TILES na última LINHA (y = _height) ----
        for (int x = 0; x < newWidth; x++) // newWidth para incluir o canto
        {
            CreateTile(x, _height); // Nova linha (incluindo o canto)
        }

        // Atualiza dimensões
        _width = newWidth;
        _height = newHeight;

    }

    private void CreateTile(int x, int y)
    {
        var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {y}";

        bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        spawnedTile.Init(isOffset);

        _tiles[new Vector2(x, y)] = spawnedTile;
    }

    public bool AreaIsClear(Vector2 position, Vector2 size)
    {
        for (float x = position.x; x < position.x + size.x; x++)
        {
            for (float y = position.y; y < position.y + size.y; y++)
            {
                Tile tile = GetTileAtPosition(new Vector2(x, y));
                if (tile == null || tile.IsOccupied)
                    return false;
            }
        }
        return true;
    }

    public void OccupyArea(Vector2 position, Vector2 size, bool occupied)
    {
        for (float x = position.x; x < position.x + size.x; x++)
        {
            for (float y = position.y; y < position.y + size.y; y++)
            {
                Tile tile = GetTileAtPosition(new Vector2(x, y));
                if (tile != null)
                    tile.SetIsOccupied(occupied);
            }
        }
    }




}