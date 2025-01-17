﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Entities.Actions
{
    /// <summary>
    /// This script toggle NavMeshObstacle. It's used for gates buildings.
    /// </summary>
    public class ActionToggleNavMeshObstacle : Action
    {
        private NavMeshObstacle _navMeshObstacle;

        private NavMeshObstacle NavMeshObstacle
        {
            get
            {
                if (_navMeshObstacle == null)
                    _navMeshObstacle = _owner.GetComponent<NavMeshObstacle>();

                return _navMeshObstacle;
            }
        }

        public ActionToggleNavMeshObstacle(Entity owner) : base(owner)
        { }

        public override bool CanExecuteAction()
        {
            return NavMeshObstacle != null;
        }

        public override void Tick()
        {
            ToggleNavMeshObstacle();
        }

        void ToggleNavMeshObstacle()
        {
            _owner.GetCharacterComponent<EntityNavMeshCollisionToggler>().ToggleNavMeshObstacle();
            LeaveAction();
        }
    }
}
