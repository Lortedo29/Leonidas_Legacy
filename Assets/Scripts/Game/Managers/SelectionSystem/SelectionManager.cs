﻿using Lortedo.Utilities.Pattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : Singleton<SelectionManager>
{
    #region Struct
    public class Group
    {
        public EntityType entityType;
        public Owner owner;
        public List<SelectableEntity> selectedEntities = new List<SelectableEntity>();

        public Group(EntityType entityType, Owner owner)
        {
            this.entityType = entityType;
            this.owner = owner;
        }
    }
    #endregion

    #region Fields
    [SerializeField] private SelectionRectManager _selectionRect;

    private List<Group> _selectedGroups = new List<Group>();
    private int _highlightGroupIndex = -1;
    #endregion

    #region Properties
    public bool IsPlayerSelection { get => _selectedGroups.Count > 0; }
    public Group[] SpartanGroups { get => (from x in _selectedGroups where x.owner == Owner.Sparta select x).ToArray(); }
    public List<Group> SelectedGroups { get => _selectedGroups; }
    #endregion

    #region Methods
    #region MonoBehaviour Callback
    void Update()
    {
        ManageHighlightGroupInput();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearSelection();
        }

        if (EventSystem.current.IsPointerOverGameObject(-1))
            return;

        if (Input.GetMouseButtonUp(0) && !_selectionRect.IsSelecting)
        {
            SwitchEntityUnderMouse();
        }
    }
    #endregion

    #region UI Methods Call
    private void UpdateUI()
    {
        if (_selectedGroups.Count == 0)
        {
            UIManager.Instance.DisplayPanel<PanelConstruction>();
        }
        else
        {
            UIManager.Instance.DisplayPanel<PanelSelection>();
        }
    }
    #endregion

    #region Manage MouseClick
    public void SwitchEntityUnderMouse()
    {
        if (HotkeyManager.Instance.askCursor)
            return;

        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            ClearSelection();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Entity")))
        {
            SelectableEntity hittedSelectableEntity = hit.transform.GetComponent<SelectableEntity>();

            if (hittedSelectableEntity)
            {
                SwitchEntity(hittedSelectableEntity);
            }
        }
    }
    #endregion

    #region Highlight Group Manager
    void ManageHighlightGroupInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _highlightGroupIndex++;
            if (_highlightGroupIndex >= _selectedGroups.Count) _highlightGroupIndex = 0;

            UIManager.Instance.PanelSelection.UpdateSelection(_selectedGroups.ToArray(), _highlightGroupIndex);
        }
    }
    #endregion

    #region SelectedGroups Manager
    public void AddEntity(SelectableEntity selectableEntity)
    {
        if (selectableEntity.Entity.FowEntity.IsCover)
            return;

        Group groupWithSameType = _selectedGroups.FirstOrDefault(x => x.entityType == selectableEntity.Type);

        // create group
        if (groupWithSameType == null)
        {
            groupWithSameType = new Group(selectableEntity.Type, selectableEntity.Owner);
            _selectedGroups.Add(groupWithSameType);

            if (_highlightGroupIndex == -1) _highlightGroupIndex = 0;
        }

        // check if entity isn't selected
        if (groupWithSameType.selectedEntities.Contains(selectableEntity))
        {
            Debug.Log("Selection Manager # " + selectableEntity + " can't be added because is already selected");
            return;
        }

        // add to selectGroup and trigger OnSelect
        groupWithSameType.selectedEntities.Add(selectableEntity);
        selectableEntity.OnSelected();

        UpdateUI();
        UIManager.Instance.PanelSelection.UpdateSelection(_selectedGroups.ToArray(), _highlightGroupIndex);
        HotkeyManager.Instance.SetCommandsHandler(_selectedGroups[0].entityType);
    }

    public void RemoveEntity(SelectableEntity selectableEntity)
    {
        Group groupWithSameType = _selectedGroups.FirstOrDefault(x => x.entityType == selectableEntity.Type);

        if (groupWithSameType == null ||
           (groupWithSameType != null && groupWithSameType.selectedEntities.Contains(selectableEntity) == false))
        {
            Debug.LogWarning("Can't remove selectableEntity that's not selected!");
            return;
        }

        selectableEntity.OnDeselect();
        groupWithSameType.selectedEntities.Remove(selectableEntity);

        // delete group if empty
        if (groupWithSameType.selectedEntities.Count == 0)
        {
            int removeGroupIndex = _selectedGroups.IndexOf(groupWithSameType);

            _selectedGroups.Remove(groupWithSameType);

            // update HighlightGroupIndex
            if (removeGroupIndex < _highlightGroupIndex)
            {
                _highlightGroupIndex--;
                if (_highlightGroupIndex < 0) _highlightGroupIndex = 0;
            }
        }

        if (_selectedGroups.Count == 0) _highlightGroupIndex = -1;

        UpdateUI();
        UIManager.Instance.PanelSelection.UpdateSelection(_selectedGroups.ToArray(), _highlightGroupIndex);
        HotkeyManager.Instance.SetCommandsHandler(_selectedGroups[0].entityType);
    }

    /// <summary>
    /// Remove SelectableEntity if it's selected. And add it, if it's not selected.
    /// </summary>
    public void SwitchEntity(SelectableEntity selectableEntity)
    {
        bool isEntitySelected = _selectedGroups.Exists(x => x.selectedEntities.Contains(selectableEntity));

        if (isEntitySelected)
        {
            RemoveEntity(selectableEntity);
        }
        else
        {
            AddEntity(selectableEntity);
        }
    }

    public void ClearSelection()
    {
        foreach (var item in _selectedGroups)
        {
            for (int j = 0; j < item.selectedEntities.Count; j++)
            {
                item.selectedEntities[j].OnDeselect();
            }
        }

        _selectedGroups.Clear();
        _highlightGroupIndex = -1;

        UIManager.Instance.PanelSelection.UpdateSelection(_selectedGroups.ToArray(), _highlightGroupIndex);
        HotkeyManager.Instance.ClearCommandsHandler();
        UpdateUI();
    }
    #endregion
    #endregion
}
