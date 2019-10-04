﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Leonidas Legacy/World/Grid")]
public class SnapGridDatabase : ScriptableObject
{
    [SerializeField, Range(1, 101)] private int _cellCount = 51;
    public int CellCount { get => _cellCount; }

    [SerializeField] private float _cellSize = 1f;
    public float CellSize { get => _cellSize; }

    [Header("Information")]
    [SerializeField, ReadOnly] private int _cellsTotalCount = 0;

    public void OnValidate()
    {
        // is _cellCount even ?
        if (_cellCount % 2 == 0)
        {
            _cellCount++;
        }

        _cellsTotalCount = _cellCount * _cellCount;
    }
}
