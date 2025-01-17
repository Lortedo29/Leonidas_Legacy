﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tartaros/World/Grid")]
public partial class SnapGridDatabase : ScriptableObject
{
    #region Fields
    [SerializeField] private float _gridLength = 80;
    [SerializeField] private float _cellSize = 1f;

    [Header("Information")]
    [SerializeField, MyBox.ReadOnly] private int _cellsTotalCount = 0;

    private int _cellCount = 51;
    #endregion

    #region Properties
    public int CellCount { get => _cellCount; }
    public float CellSize { get => _cellSize; }
    #endregion

    #region Methods
    #region MonoBehaviour Callbacks
    void Awake()
    {
        _cellCount = Mathf.RoundToInt(_gridLength / _cellSize);
    }
    #endregion

    #region Public Methods
    public bool GetNearestPositionFromMouse(out Vector3 positionFromMouse, int layerMask = ~0)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            positionFromMouse = GetNearestPosition(hit.point);
            return true;
        }
        else
        {
            positionFromMouse = Vector3.zero;
            return false;
        }
    }

    public Vector2Int GetNearestCoords(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / _cellSize);
        int zCount = Mathf.RoundToInt(position.z / _cellSize);

        return new Vector2Int(xCount, zCount);
    }

    public Vector3 GetNearestPosition(Vector3 position)
    {
        return GetNearestPosition(position.x, position.y, position.z);
    }

    public Vector3 GetNearestPosition(float x, float y, float z)
    {
        float xCount = Mathf.Round(x / _cellSize);
        float yCount = Mathf.Round(y / _cellSize);
        float zCount = Mathf.Round(z / _cellSize);

        Vector3 result = new Vector3(
             xCount * _cellSize,
             yCount * _cellSize,
             zCount * _cellSize);

        return result;
    }

    public Vector2Int WorldToCoords(Vector3 position)
    {
        var gridPosition = GetNearestPosition(position);

        var cellSize = _cellSize;
        Vector2Int result = new Vector2Int(
            (int)(gridPosition.x / cellSize),
            (int)(gridPosition.z / cellSize));

        return result;
    }


    public Vector3 CoordsToWorldPosition(Vector2Int coords)
    {
        var cellSize = _cellSize;
        return new Vector3(coords.x * cellSize, 0, coords.y * cellSize);
    }
    #endregion
    #endregion
}

#if UNITY_EDITOR
public partial class SnapGridDatabase : ScriptableObject
{
    public void OnValidate()
    {
        // is _cellCount even ?
        if (_cellCount % 2 == 0)
        {
            _cellCount++;
        }

        _cellsTotalCount = _cellCount * _cellCount;

        _cellCount = Mathf.RoundToInt(_gridLength / _cellSize);
    }


    public void DrawGizmos()
    {
        DrawGizmos(Color.red);
    }

    public void DrawGizmos(Color gridColor)
    {
        Gizmos.color = gridColor;

        for (int x = 0; x <= _cellCount; x++)
        {
            for (int z = 0; z <= _cellCount; z++)
            {
                var point = new Vector3(x * _cellSize, 0, z * _cellSize);
                Gizmos.DrawWireCube(point, new Vector3(_cellSize, 0, _cellSize));
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Vector3.zero, 0.15f);
    }
    
    public void SetGridLength(float length)
    {
        _gridLength = length;
        OnValidate();
    }
}
#endif