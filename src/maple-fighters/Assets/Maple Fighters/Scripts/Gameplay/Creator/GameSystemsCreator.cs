﻿using System;
using System.Collections.Generic;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map;
using Scripts.Gameplay.Player;
using Scripts.UI.Chat;
using Scripts.UI.Focus;
using UI.Manager;
using UnityEngine;

namespace Scripts.Gameplay.Creator
{
    public class GameSystemsCreator : MonoBehaviour
    {
        private void Awake()
        {
            CreateGameComponents(GetCreatorsComponents());
            CreateGameComponents(GetContainersComponents());
            CreateGameComponents(GetGUIControllersComponents());

            Destroy(gameObject);
        }

        private IEnumerable<Type> GetCreatorsComponents()
        {
            yield return typeof(CharacterCreator);
            yield return typeof(EnterMapMessageSender);
        }

        private IEnumerable<Type> GetContainersComponents()
        {
            yield return typeof(EntityContainer);
        }

        private IEnumerable<Type> GetGUIControllersComponents()
        {
            yield return typeof(FocusStateController);
            yield return typeof(ChatController);
        }

        private void CreateGameComponents(IEnumerable<Type> components)
        {
            foreach (var component in components)
            {
                var name = component.Name.MakeSpaceBetweenWords();
                var creatorGameObject = new GameObject(name);

                creatorGameObject.AddComponent(component);
            }
        }
    }
}