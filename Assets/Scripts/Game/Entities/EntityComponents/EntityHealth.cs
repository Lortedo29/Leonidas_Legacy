﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : EntityComponent
{
    #region Fields
    [Header("Health Slider Behaviour")]
    [SerializeField] private Canvas _sliderCanvas;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private bool _hideHealthSliderIfFull = true;

    private int _hp;
    private int _maxHp;
    #endregion

    #region Properties
    public int MaxHp { get => _maxHp; }
    public int Hp { get => _hp; }
    public bool IsAlive { get => _hp > 0 ? true : false; }
    #endregion

    #region Methods
    #region MonoBehaviour Callbacks
    void Start()
    {
        _hp = Entity.Data.Hp;
        _maxHp = Entity.Data.Hp;

        SetHealthContent();
    }

    void OnEnable()
    {
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogCover += EntityHealth_OnFogCover;
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogUncover += EntityHealth_OnFogUncover;
    }

    void OnDisable()
    {
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogCover -= EntityHealth_OnFogCover;
        Entity.GetCharacterComponent<EntityFogCoverable>().OnFogUncover -= EntityHealth_OnFogUncover;
    }
    #endregion

    #region Events handlers
    private void EntityHealth_OnFogCover(Game.FogOfWar.IFogCoverable fogCoverable)
    {
        HideHealth();
    }

    private void EntityHealth_OnFogUncover(Game.FogOfWar.IFogCoverable fogCoverable)
    {
        DisplayHealth();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// GoDamage to this entity.
    /// </summary>
    /// <param name="damage">Amount of hp to be removed</param>
    /// <param name="attacker">Entity which do damage to the entity</param>
    public void GetDamage(int damage, Entity attacker)
    {
        if (Entity.Data.IsInvincible)
            return;

        _hp -= damage;
        _hp = Mathf.Clamp(_hp, 0, _maxHp);

        SetHealthContent();

        if (!IsAlive)
        {
            Entity.Death();
        }
    }
    #endregion

    #region Private methods
    void DisplayHealth()
    {
        if (_healthSlider == null)
            return;

        // hide or not the slider
        if (_hideHealthSliderIfFull)
        {
            bool isFullLife = (_hp == _maxHp);
            bool shouldBeActive = !isFullLife;

            _sliderCanvas.gameObject.SetActive(shouldBeActive);
        }
    }

    void HideHealth()
    {
        _sliderCanvas.gameObject.SetActive(false);
    }

    void SetHealthContent()
    {
        if (_healthSlider == null)
            return;

        // update the value
        _healthSlider.maxValue = _maxHp;
        _healthSlider.value = _hp;

        DisplayHealth();
    }
    #endregion
    #endregion
}
