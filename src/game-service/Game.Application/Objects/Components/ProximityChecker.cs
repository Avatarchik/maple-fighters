using System.Collections.Generic;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private IInterestArea<IGameObject> interestArea;
        private List<IGameObject> gameObjects;

        public ProximityChecker()
        {
            gameObjects = new List<IGameObject>();
        }

        protected override void OnAwake()
        {
            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            var scene = presenceSceneProvider.GetScene();
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(scene, gameObject);

            SubscribeToNearbySceneObjectsEvents();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromNearbySceneObjectsEvents();

            interestArea?.Dispose();
        }

        private void SubscribeToNearbySceneObjectsEvents()
        {
            interestArea.NearbySceneObjectsEvents.SceneObjectAdded += OnSceneObjectAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectRemoved += SceneObjectRemoved;
            interestArea.NearbySceneObjectsEvents.SceneObjectsAdded += OnSceneObjectsAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectsRemoved += OnSceneObjectsRemoved;
        }

        private void UnsubscribeFromNearbySceneObjectsEvents()
        {
            interestArea.NearbySceneObjectsEvents.SceneObjectAdded -= OnSceneObjectAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectRemoved -= SceneObjectRemoved;
            interestArea.NearbySceneObjectsEvents.SceneObjectsAdded -= OnSceneObjectsAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectsRemoved -= OnSceneObjectsRemoved;
        }

        private void OnSceneObjectAdded(IGameObject sceneObject)
        {
            gameObjects.Add(sceneObject);
        }

        private void SceneObjectRemoved(IGameObject sceneObject)
        {
            gameObjects.Remove(sceneObject);
        }

        private void OnSceneObjectsAdded(IEnumerable<IGameObject> sceneObjects)
        {
            gameObjects.AddRange(sceneObjects);
        }

        private void OnSceneObjectsRemoved(IEnumerable<IGameObject> sceneObjects)
        {
            foreach (var sceneObject in sceneObjects)
            {
                gameObjects.Remove(sceneObject);
            }
        }

        public IEnumerable<IGameObject> GetGameObjects()
        {
            return gameObjects;
        }
    }
}