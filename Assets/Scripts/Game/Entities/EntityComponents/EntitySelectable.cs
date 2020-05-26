using Lortedo.Utilities.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSelected(Entity entity);
public delegate void OnUnselected(Entity entity);

public class EntitySelectable : EntityComponent
{
    #region Fields
    public event OnSelected OnSelected;
    public event OnUnselected OnUnselected;

    private GameObject _selectionCircle = null;
    private bool _isSelected = false;
    #endregion

    #region Properties
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            bool oldIsSelected = _isSelected;

            _isSelected = value;

            if (oldIsSelected != _isSelected)
            {
                if (_isSelected)
                {
                    OnSelected?.Invoke(Entity);
                    OnSelection();
                }
                else
                {
                    OnUnselected?.Invoke(Entity);
                    OnUnselection();
                }
            }
        }
    }
    #endregion

    #region Methods
    #region Mono Callbacks
    void OnEnable()
    {
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogCover += OnFogCover;
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogUncover += OnFogUncover;
    }

    void OnDisable()
    {
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogCover -= OnFogCover;
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogUncover -= OnFogUncover;

        if (Game.Selection.SelectionManager.Instance != null)
        {
            Game.Selection.SelectionManager.Instance.RemoveEntity(Entity);
        }

        IsSelected = false;
    }
    #endregion

    #region Events handlers
    void OnFogCover(Game.FogOfWar.IFogCoverable fogCoverable)
    {
        HideSelectionCircle();
    }

    void OnFogUncover(Game.FogOfWar.IFogCoverable fogCoverable)
    {
        DisplaySelectionCircle();
    }
    #endregion

    #region Public methods
    void OnSelection()
    {
        DisplaySelectionCircle();
    }

    void OnUnselection()
    {
        HideSelectionCircle();
    }
    #endregion

    #region Private methods
    private void DisplaySelectionCircle()
    {
        if (_selectionCircle != null)
        {
            Debug.LogWarning("Entity Selectable : Cannot display selection, it's already displayed. But it's okay.");
            return;
        }

        Vector3 pos = transform.position + Vector3.up * 0.78f;
        Quaternion rot = Quaternion.Euler(90, 0, 0);

        _selectionCircle = ObjectPooler.Instance.SpawnFromPool(ObjectPoolingTags.keySelectionCircle, pos, rot);

        if (_selectionCircle)
        {
            _selectionCircle.transform.parent = transform;
            _selectionCircle.GetComponent<SelectionCircle>().SetCircleOwner(Entity.Team);
            _selectionCircle.GetComponent<Projector>().orthographicSize = Mathf.Max(Entity.Data.TileSize.x, Entity.Data.TileSize.y);
        }
        else
        {
            Debug.LogErrorFormat("Entity Selectable : No selection circle pool in found. Please check your ObjectPooler.");
        }
    }

    private void HideSelectionCircle()
    {
        if (_selectionCircle == null)
        {
            //Debug.LogWarning("Entity Selectable : Cannot hide selection, it's already hide. It's okay.");
            return;
        }

        ObjectPooler.Instance.EnqueueGameObject(ObjectPoolingTags.keySelectionCircle, _selectionCircle);
        _selectionCircle = null;
    }
    #endregion
    #endregion
}
