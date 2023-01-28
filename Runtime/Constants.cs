using System;
using System.IO;
using UnityEngine;
using System.Reflection;

using UnityObject = UnityEngine.Object;

namespace Ikonoclast.Globals
{
    /// <summary>
    /// Provides globally accessed constants.
    /// </summary>
    public static class Constants
    {
        public static class TimeConstants
        {
            public const float
                HalfTimeScale = 0.5f,
                DefaultTimeScale = 1.0f;

            public const int DefaultFrameRate = 120;
        }

        public static class CommonConstants
        {
            public static class Errors
            {
                #region UNITY_EDITOR
#if UNITY_EDITOR
                public const string MultipleEditorInstance = "Multiple instance of EditorWindow not supported";
#endif
                #endregion
            }

            public static class Warnings
            {
                #region AttemptToCall

                public static string AttemptToCall(MethodBase method) =>
                    $"Attempt to call {method.Name}";
                public static string AttemptToCall(string methodName) =>
                    $"Attempt to call {methodName}";

                public static string AttemptToCall(MethodBase method, object without) =>
                    $"{AttemptToCall(method)} without {without}";
                public static string AttemptToCall(string methodName, object without) =>
                    $"{AttemptToCall(methodName)} without {without}";

                #endregion

                #region RemovedComponent

                public static string RemovedComponent(Type type) =>
                    $"Removed component of type {type}";
                public static string RemovedComponent(Type type, GameObject o) =>
                    $"{RemovedComponent(type)} from Object {o}";

                public static string RemovedComponent<T>() where T : UnityEngine.Object =>
                    RemovedComponent(typeof(T));
                public static string RemovedComponent<T>(T _) where T : UnityEngine.Object =>
                    RemovedComponent(typeof(T));
                public static string RemovedComponent<T>(GameObject o) where T : UnityEngine.Object =>
                    RemovedComponent(typeof(T), o);
                public static string RemovedComponent<T>(T _, GameObject o) where T : UnityEngine.Object =>
                    RemovedComponent(typeof(T), o);

                #endregion

                #region UnableToGetComponent

                public static string UnableToGetComponent(Type type) =>
                    $"Unable to get component of type {type}";
                public static string UnableToGetComponent(Type type, string name) =>
                    $"{UnableToGetComponent(type)} with name {name}";
                public static string UnableToGetComponent(Type type, UnityObject o) =>
                    $"{UnableToGetComponent(type)} on Object {o}";
                public static string UnableToGetComponent(Type type, UnityObject o, string name) =>
                    $"{UnableToGetComponent(type, name)} on Object {o.name}";

                public static string UnableToGetComponent<T>() where T : UnityObject =>
                    UnableToGetComponent(typeof(T));
                public static string UnableToGetComponent<T>(T _) where T : UnityObject =>
                    UnableToGetComponent(typeof(T));
                public static string UnableToGetComponent<T>(UnityObject o) where T : UnityObject =>
                    UnableToGetComponent(typeof(T), o);
                public static string UnableToGetComponent<T>(T _, UnityObject o) where T : UnityObject =>
                    UnableToGetComponent(typeof(T), o);
                public static string UnableToGetComponent<T>(UnityObject o, string name) where T : UnityObject =>
                    UnableToGetComponent(typeof(T), o, name);
                public static string UnableToGetComponent<T>(T _, UnityObject o, string name) where T : UnityObject =>
                    UnableToGetComponent(typeof(T), o, name);

                #endregion
            }

            public static class ValueType
            {
                public const char
                    FloatPrefix = 'f',
                    StringPrefix = 's',
                    IntegerPrefix = 'i',
                    BooleanPrefix = 'b',
                    Vector2Prefix = 'v';

                public const string
                    FloatDefault = "0",
                    StringDefault = "",
                    IntegerDefault = "0",
                    BooleanDefault = "False",
                    Vector2Default = "(0, 0)";
            }
        }

        public static class PlayerConstants
        {
            public static class Movement
            {
                public const float
                    ClimbForce = 30f,
                    DropForce = 12f,
                    MinimumSpeedBeforeIdle = 2.5f;
            }
        }

        public static class PhysicsConstants
        {
            public const float
                HalfGravity = 4.9f,
                DefaultGravity = 9.8f;

            public static class Rigidbody2D
            {
                public const float
                    Drag = 1f,
                    AngularDrag = 0.05f;
            }
        }

        #region UNITY_EDITOR 

#if UNITY_EDITOR

        public static class EditorConstants
        {
            public static class Window
            {
                public const string
                    GameViewTypeName = "GameView",
                    SceneViewTypeName = "SceneView",
                    InspectorTypeName = "InspectorWindow",
                    ConsoleWindowTypeName = "ConsoleWindow",
                    ProjectBrowserTypeName = "ProjectBrowser",
                    SceneHierarchyTypeName = "SceneHierarchyWindow";
            }
        }

        public static class DirectoryConstants
        {
            public const string
                Assets = "Assets",
                Actors = "Actors",
                Animations = "Animations",
                Definitions = "Definitions",
                Editor = "Editor",
                EditorDefaultResources = "Editor Default Resources",
                Externals = "Externals",
                JsonDotNet = "JsonDotNet",
                Materials = "Materials",
                NaughtyAttributes = "NaughtyAttributes",
                Packages = "Packages",
                Prefabs = "Prefabs",
                Presets = "Presets",
                Rendering = "Rendering",
                Resources = "Resources",
                Scenes = "Scenes",
                ScriptableObjects = "Scriptable Objects",
                Scripts = "Scripts",
                Sprites = "Sprites",
                NodeGraphs = "Node Graphs";

            public static string Combine(string path1, string path2, bool prefixAssets = true) =>
                prefixAssets
                    ? Path.Combine(Assets, path1, path2)
                    : Path.Combine(path1, path2);

            public static string Combine(string path1, string path2, string path3, bool prefixAssets = true) =>
                prefixAssets
                    ? Path.Combine(Assets, path1, path2, path3)
                    : Path.Combine(path1, path2, path3);

            public static string Combine(bool prefixAssets = true, params string[] paths) =>
                prefixAssets
                    ? Path.Combine(Assets, Path.Combine(paths))
                    : Path.Combine(paths);
        }

#endif
        #endregion
    }
}
