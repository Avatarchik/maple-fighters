﻿using ComponentModel.Common;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.SceneObjects
{
    internal class PortalInfoProvider : Component<ISceneObject>, IPortalInfoProvider
    {
        public Maps Map { get; }

        public PortalInfoProvider(Maps map)
        {
            Map = map;
        }
    }
}