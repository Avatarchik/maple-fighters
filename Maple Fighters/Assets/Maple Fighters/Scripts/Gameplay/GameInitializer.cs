﻿using Scripts.Containers;
using Scripts.Containers.Entity;
using Scripts.Containers.Service;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Gameplay
{
    public class GameInitializer : DontDestroyOnLoad<GameInitializer>
    {
        private IGameService gameService;
        private IEntityContainer entityContainer;

        private void Start()
        {
            gameService = ServiceContainer.GameService;
            entityContainer = GameContainers.EntityContainer;
        }

        private void OnApplicationQuit()
        {
            gameService.Disconnect();
        }
    }
}