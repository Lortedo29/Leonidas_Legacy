﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityMovement : EntityComponent
{
    #region Fields
    private NavMeshAgent _navMeshAgent;
    #endregion

    #region Methods
    #region Mono Callbacks
    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.speed = Entity.Data.Speed;
        }
    }
    #endregion

    #region Public methods
    public void SetAvoidance(Avoidance avoidance)
    {
        _navMeshAgent.avoidancePriority = avoidance.ToPriority();
    }

    public void MoveToEntity(Entity target)
    {
        if (!Entity.Data.CanMove)
            return;

        SetAvoidance(Avoidance.Move);

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(target.transform.position);
    }

    public void MoveToPosition(Vector3 position)
    {
        if (!Entity.Data.CanMove) return;

        SetAvoidance(Avoidance.Move);

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(position);
    }

    public void StopMoving()
    {
        if (!Entity.Data.CanMove) return;

        SetAvoidance(Avoidance.Idle);

        _navMeshAgent.SetDestination(transform.position);
        _navMeshAgent.ResetPath();

        _navMeshAgent.isStopped = true;
    }


    public bool HasReachedDestination()
    {
        if (!Entity.Data.CanMove) return true;

        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath) Debug.LogWarningFormat("{0} doesn't have found a valid path to reached it destination.", transform.name);

                if (_navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
    #endregion
    #endregion
}
