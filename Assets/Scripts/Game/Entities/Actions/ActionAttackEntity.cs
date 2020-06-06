using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Actions
{
    /// <summary>
    /// This script make the Entity go closer to '_target'.
    /// Than, the Entity stop moving and attacks '_target'.
    /// </summary>
    public class ActionAttackEntity : Action
    {
        Entity _target;

        public ActionAttackEntity(Entity owner, Entity target) : base(owner)
        {
            Debug.LogWarningFormat("'{1}' can't execute action {0} because _target is null.", this.GetType(), _owner.name);
            _target = target;
        }

        public override void OnStateExit()
        {
            entity.GetCharacterComponent<EntityMovement>().StopMoving();
        }

        public override void Tick()
        {
            // if target has been killed
            if (!_target.IsInstanciate)
            {
                TargetDeath();
                return;
            }

            EntityMovement entityMovement = entity.GetCharacterComponent<EntityMovement>();
            EntityDetection entityDetection = entity.GetCharacterComponent<EntityDetection>();

            // Can entity attack target?
            if (entityDetection.IsEntityInAttackRange(_target))
            {
                entityMovement.StopMoving();

                EntityAttack entityAttack = entity.GetCharacterComponent<EntityAttack>();
                entityAttack.DoAttack(_target);
            }
            else // Otherwise, entity go closer.
            {
                entityMovement.MoveToEntity(_target);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} attacks {1}.", _owner.name, _target.name);
        }

        public override bool CanExecuteAction()
        {            
            return _target != null;
        }

        private void TargetDeath()
        {
            // try to auto attack nearest enemy
            bool attacksANewEnemy = entity.GetCharacterComponent<EntityAttack>().TryStartActionAttackNearestEnemy();

            // if not enemy nearest, stop current action
            if (!attacksANewEnemy)
            {
                entity.StopCurrentAction();
            }

        }
    }
}
