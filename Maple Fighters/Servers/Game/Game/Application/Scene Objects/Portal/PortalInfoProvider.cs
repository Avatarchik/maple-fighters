﻿using ComponentModel.Common;
using Game.InterestManagement;
using Game.Common;

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