﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.FogOfWar
{
    public interface IFogVision
    {
        float ViewRadius { get; }
        Vector3 Position { get; }
    }
}
