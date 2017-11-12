﻿using System.Collections.Generic;
using System.IO;
using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;
using Microsoft.Scripting.Hosting;
using Physics.Box2D;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public class SceneContainer : Component<IServerEntity>, ISceneContainer
    {
        private readonly Dictionary<Maps, IGameSceneWrapper> scenes = new Dictionary<Maps, IGameSceneWrapper>();

        private IPythonScriptEngine pythonScriptEngine;
        private ScriptScope scriptScope;

        protected override void OnAwake()
        {
            base.OnAwake();

            pythonScriptEngine = Entity.Container.GetComponent<IPythonScriptEngine>().AssertNotNull();

            scriptScope = pythonScriptEngine.GetScriptEngine().CreateScope();
            scriptScope.SetVariable("sceneContainer", this);

            AddScenesViaPython();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var scenesTemp = new List<IGameSceneWrapper>();
            scenesTemp.AddRange(scenes.Values);

            scenes.Clear();

            foreach (var gameSceneWrapper in scenesTemp)
            {
                gameSceneWrapper.Dispose();
            }
        }

        private void AddScenesViaPython()
        {
            const string PYTHON_SCRIPTS_PATH = "python/scenes";
            var pythonScripts = Directory.GetFiles(PYTHON_SCRIPTS_PATH, "*.py", SearchOption.TopDirectoryOnly);

            foreach (var pythonScript in pythonScripts)
            {
                pythonScriptEngine.GetScriptEngine().ExecuteFile(pythonScript, scriptScope);
            }
        }

        public void AddScene(byte map, Vector2 sceneSize, Vector2 regionSize)
        {
            scenes.Add((Maps) map, new GameSceneWrapper((Maps) map, sceneSize, regionSize));
        }

        public void AddPhysics(byte map, Vector2 lowerBound, Vector2 upperBound, Vector2 gravity, bool doSleep, bool drawPhysics)
        {
            var gameScene = GetSceneWrapper((Maps)map);
            gameScene.Container.AddComponent(new PhysicsWorldCreator(lowerBound, upperBound, gravity, doSleep));
            gameScene.AddScenePhysicsData();

            if (drawPhysics)
            {
                gameScene.Container.AddComponent(new PhysicsSimulationWindowCreator(((Maps)map).ToString()));
            }
        }

        public IGameSceneWrapper GetSceneWrapper(Maps map)
        {
            if (scenes.TryGetValue(map, out var scene))
            {
                return scene;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene with map {map}"), LogMessageType.Error);
            return null;
        }
    }
}