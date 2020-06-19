﻿using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace InterestManagement
{
    public class InterestArea<TSceneObject> : IInterestArea<TSceneObject>
        where TSceneObject : ISceneObject
    {
        private readonly TSceneObject sceneObject;
        private readonly List<IRegion<TSceneObject>> regions;
        private readonly NearbySceneObjectsCollection<TSceneObject> nearbySceneObjects;

        private IScene<TSceneObject> scene;

        public InterestArea(TSceneObject sceneObject)
        {
            this.sceneObject = sceneObject;

            regions = new List<IRegion<TSceneObject>>();
            nearbySceneObjects = new NearbySceneObjectsCollection<TSceneObject>();
        }

        public void SetScene(IScene<TSceneObject> scene)
        {
            this.scene = scene;

            UpdateNearbyRegions();
            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();

            foreach (var region in regions)
            {
                region.Unsubscribe(sceneObject);
            }

            regions?.Clear();
            nearbySceneObjects?.Clear();
        }

        private void SubscribeToPositionChanged()
        {
            sceneObject.Transform.PositionChanged += UpdateNearbyRegions;
        }

        private void UnsubscribeFromPositionChanged()
        {
            sceneObject.Transform.PositionChanged -= UpdateNearbyRegions;
        }

        private void UpdateNearbyRegions()
        {
            SubscribeToVisibleRegions();
            UnsubscribeFromInvisibleRegions();
        }

        private void SubscribeToVisibleRegions()
        {
            var visibleRegions =
                scene?.MatrixRegion.GetRegions(sceneObject.Transform);
            if (visibleRegions != null)
            {
                foreach (var region in visibleRegions)
                {
                    if (regions.Contains(region))
                    {
                        continue;
                    }

                    regions.Add(region);

                    if (region.SubscriberCount() != 0)
                    {
                        nearbySceneObjects.Add(region.GetAllSubscribers());
                    }

                    region.Subscribe(sceneObject);

                    SubscribeToRegionEvents(region);
                }
            }
        }

        private void UnsubscribeFromInvisibleRegions()
        {
            var invisibleRegions =
                regions
                    .Where(
                        region =>
                            !region.IsOverlaps(
                                sceneObject.Transform.Position,
                                sceneObject.Transform.Size))
                    .ToArray();

            foreach (var region in invisibleRegions)
            {
                regions.Remove(region);
                region.Unsubscribe(sceneObject);

                if (region.SubscriberCount() != 0)
                {
                    foreach (var subscriber in region.GetAllSubscribers())
                    {
                        var position = sceneObject.Transform.Position;
                        var size = sceneObject.Transform.Size;

                        if (IsOverlapsWithNearbyRegions(position, size) == false)
                        {
                            nearbySceneObjects.Remove(subscriber);
                        }
                    }
                }

                UnsubscribeFromRegionEvents(region);
            }
        }

        private void SubscribeToRegionEvents(IRegion<TSceneObject> region)
        {
            region.SubscriberAdded += OnSubscriberAdded;
            region.SubscriberRemoved += OnSubscriberRemoved;
        }

        private void UnsubscribeFromRegionEvents(IRegion<TSceneObject> region)
        {
            region.SubscriberAdded -= OnSubscriberAdded;
            region.SubscriberRemoved -= OnSubscriberRemoved;
        }

        private void OnSubscriberAdded(TSceneObject sceneObject)
        {
            nearbySceneObjects.Add(sceneObject);
        }

        private void OnSubscriberRemoved(TSceneObject sceneObject)
        {
            var position = sceneObject.Transform.Position;
            var size = sceneObject.Transform.Size;

            if (IsOverlapsWithNearbyRegions(position, size) == false)
            {
                nearbySceneObjects.Remove(sceneObject);
            }
        }

        private bool IsOverlapsWithNearbyRegions(Vector2 position, Vector2 size)
        {
            return regions.Any(region => region.IsOverlaps(position, size));
        }

        public IEnumerable<IRegion<TSceneObject>> GetRegions()
        {
            return regions;
        }

        public IEnumerable<TSceneObject> GetNearbySceneObjects()
        {
            return nearbySceneObjects.GetSceneObjects();
        }

        public INearbySceneObjectsEvents<TSceneObject> GetNearbySceneObjectsEvents()
        {
            return nearbySceneObjects;
        }
    }
}