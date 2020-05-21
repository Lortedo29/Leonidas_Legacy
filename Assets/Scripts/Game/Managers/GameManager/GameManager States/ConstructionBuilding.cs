﻿using Lortedo.Utilities.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.ConstructionSystem
{
    /// <summary>
    /// This script manages display of a building to be constructed.
    /// It manages deactivation of components. 
    /// It manages building shader properties.
    /// </summary>
    public class ConstructionBuilding
    {
        #region Fields
        public readonly static string debugLogHeader = "Construction Building : ";

        private readonly GameObject _building;
        private readonly bool _isChainedBuilding;
        private readonly EntityType _entityType;
        #endregion

        #region Properties
        public GameObject Building { get => _building; }
        #endregion

        public ConstructionBuilding(GameObject gameObject, EntityType entityType, bool isChainedBuilding)
        {
            _building = gameObject;
            _entityType = entityType;
            _isChainedBuilding = isChainedBuilding;

            EnableBuildingComponents(false);
        }

        #region Methods
        #region Public Methods
        public void SetPosition(Vector3 newPosition)
        {
            if (newPosition != _building.transform.position)
            {
                _building.transform.position = newPosition;
                _building.GetComponent<EntityResourcesGeneration>().CalculateResourcesPerTick();
                UpdateBuildingMeshColor();
            }
        }

        public void Destroy()
        {
            Object.Destroy(_building);
        }

        /// <summary>
        /// Set building mesh to 'NotInBuildState'
        /// </summary>
        public void ResetBuildingMeshColor()
        {
            if (_building.TryGetComponent(out BuildingMesh buildingMesh))
            {
                buildingMesh.SetState(BuildingMesh.State.NotInBuildState);
            }
            else
            {
                Debug.LogErrorFormat(debugLogHeader + "The current building {0} is missing a BuildingMesh component.", _building.name);
            }
        }

        public void EnableBuildingComponents(bool enabled)
        {
            var fowEntity = _building.GetComponent<EntityFogVision>();
            if (fowEntity) fowEntity.enabled = enabled;

            var collider = _building.GetComponent<Collider>();
            if (collider) collider.enabled = enabled;

            var navMeshAgent = _building.GetComponent<NavMeshAgent>();
            if (navMeshAgent) navMeshAgent.enabled = enabled;

            var navMeshObstacle = _building.GetComponent<NavMeshObstacle>();
            if (navMeshObstacle) navMeshObstacle.enabled = enabled;

            if (_building.TryGetComponent(out EntityResourcesGeneration resourcesGeneration))
                resourcesGeneration.EnableResourceProduction = enabled;

            if (_building.TryGetComponent(out Entity entity))
                entity.enabled = enabled; // disable OnSpawn call

            if (_building.TryGetComponent(out EntityFogCoverable entityFogCoverable))
                entityFogCoverable.enabled = enabled;

            WallAppearance wallAppearence = _building.GetComponentInChildren<WallAppearance>();
            if (wallAppearence)
                wallAppearence.enabled = enabled;
        }

        public void SetConstructionAsFinish(Team teamToSet)
        {
            EnableBuildingComponents(true);
            ResetBuildingMeshColor();

            if (Building.TryGetComponent(out Entity entity))
            {
                entity.Team = teamToSet;
            }

            DynamicsObjects.Instance.SetToParent(_building.transform, "Building");
        }
        #endregion

        #region Private Methods
        private void UpdateBuildingMeshColor()
        {
            BuildingMesh.State state = CanBeConstruct() ? BuildingMesh.State.CanBuild : BuildingMesh.State.CannotBuild;

            if (_building.TryGetComponent(out BuildingMesh buildingMesh))
            {
                buildingMesh.SetState(state);
            }
            else
            {
                Debug.LogErrorFormat(debugLogHeader + "The current building {0} is missing a BuildingMesh component.", _building.name);
            }
        }

        private bool CanBeConstruct()
        {
            Vector2Int coords = TileSystem.Instance.WorldPositionToCoords(_building.transform.position);

            return TileSystem.Instance.IsTileFree(coords) || TileSystem.Instance.IsTileOfType(coords, _entityType);
        }
        #endregion
        #endregion
    }
}
