﻿using Lortedo.Utilities.Debugging;
using Lortedo.Utilities.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Game.ConstructionSystem
{
    public class ConstructionState : AbstractConstructionState
    {
        #region Fields
        ConstructionBuilding _constructionBuilding;
        #endregion

        public ConstructionState(GameManager owner, string entityID) : base(owner, entityID)
        { }

        #region Methods
        #region Public override
        public override void Tick()
        {
            UpdateConstructionBuildingPosition();

            base.Tick();
        }
        #endregion

        #region Protected override        
        protected override void OnMouseUp()
            => ConstructBuilding();

        protected override void OnCurrentBuildingSet(string entityID, EntityData buildingData)
        {
            var building = ObjectPooler.Instance.SpawnFromPool(buildingData.Prefab, Vector3.zero, Quaternion.identity, true);

            Assert.IsNotNull(building, "Building out of ObjectPooler is null.");

            _constructionBuilding = new ConstructionBuilding(building, entityID, buildingData);

            UpdateConstructionBuildingPosition();
        }

        protected override void ConstructBuilding()
        {
            GameObject building = _constructionBuilding.Building;

            // register tile
            const TileFlag tileFlagCondition = TileFlag.Free | TileFlag.Visible;
            bool successfulSetTile = TileSystem.Instance.TrySetTile(building, EntityData.TileSize, tileFlagCondition);

            if (!successfulSetTile)
                return;

            _constructionBuilding.SetConstructionAsFinish(Team.Player);

            // then leave
            SucessfulBuild = true;
            _owner.State = null;
        }

        protected override void DestroyAllConstructionBuildings()
        {
            _constructionBuilding.Destroy();
        }
        #endregion

        #region Private methods
        private void UpdateConstructionBuildingPosition()
        {
            if (GameManager.Instance.Grid.GetNearestPositionFromMouse(out Vector3 newPosition, terrainLayerMask))
            {
                _constructionBuilding.SetPosition(newPosition);
            }
            else
            {
                Debug.LogWarningFormat("Building State : Can't find nearest position from mouse. We can't update the building position.");
            }
        }
        #endregion
        #endregion
    }
}
