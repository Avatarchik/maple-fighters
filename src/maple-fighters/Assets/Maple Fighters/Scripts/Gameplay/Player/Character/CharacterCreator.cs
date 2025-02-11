﻿using System.Collections;
using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterCreator : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneEntered += OnSceneEntered;
            gameApi.GameObjectsAdded += OnGameObjectsAdded;
        }

        private void OnDisable()
        {
            gameApi.SceneEntered -= OnSceneEntered;
            gameApi.GameObjectsAdded -= OnGameObjectsAdded;
        }

        private void OnSceneEntered(EnteredSceneMessage message)
        {
            var id = message.GameObjectId;
            var name = message.CharacterData.Name;
            var @class = message.CharacterData.Class;
            var direction = message.SpawnData.Direction;
            var characterData = new CharacterData(name, @class, direction);

            StartCoroutine(WaitFrameAndSpawn(id, characterData));
        }

        private void OnGameObjectsAdded(GameObjectsAddedMessage message)
        {
            foreach (var gameObject in message.GameObjects)
            {
                var id = gameObject.Id;
                var name = gameObject.CharacterData.Name;
                var @class = gameObject.CharacterData.Class;
                var direction = gameObject.Direction;
                var characterData = new CharacterData(name, @class, direction);

                StartCoroutine(WaitFrameAndSpawn(id, characterData));
            }
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndSpawn(int entityId, CharacterData characterData)
        {
            yield return null;

            if (EntityContainer.GetInstance().GetRemoteEntity(entityId, out var entity))
            {
                var characterDataProvider =
                    entity?.GameObject.GetComponent<CharacterDataProvider>();
                characterDataProvider?.SetCharacterData(characterData);

                var spawnedCharacter = entity?.GameObject.GetComponent<SpawnCharacter>();
                spawnedCharacter?.Spawn();
            }
        }
    }
}