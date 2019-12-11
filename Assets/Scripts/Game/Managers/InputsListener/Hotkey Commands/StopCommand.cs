﻿using Lortedo.Utilities.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class StopCommand : Command
    {
        public override void Execute()
        {
            SelectedGroupsActionsCaller.OrderStop();
        }
    }
}
