
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections.Generic;

using Random = UnityEngine.Random;
using UnityTime = UnityEngine.Time;
using UnityObject = UnityEngine.Object;

namespace Ikonoclast.Globals
{
    using static Constants.CommonConstants;

    /// <summary>
    /// Provides global access to extension, and otherwise useful, methods.
    /// </summary>
    public static class Extensions
    {
        #region Enum

        public static int GetSetBitCount(this Enum _enum)
        {
            if (_enum == null)
                return 0;

            int count = 0;

            long value = Convert.ToInt64(_enum);

            while (value != 0)
            {
                value &= (value - 1);

                count++;
            }

            return count;
        }

        #endregion

        #region String

        public static string UniquePath(this string str, string ext)
        {
            int i = 1;

            string temp = str.Replace(ext, "");

            while (File.Exists(str))
            {
                str = $"{temp} ({i++}){ext}";
            }

            return str;
        }

        public static bool Contains(this string str, string other, StringComparison comparison)
        {
            return str?.IndexOf(other, comparison) >= 0;
        }

        public static bool ContainsAny(this string str, params string[] strings)
        {
            if (strings == null)
                return false;

            for (int i = 0; i < strings.Length; i++)
                if (str.Contains(strings[i]))
                    return true;

            return false;
        }

        public static bool ContainsAny(this string str, string string1, string string2)
        {
            return str.Contains(string1) || str.Contains(string2);
        }

        public static bool ContainsAny(this string str, string string1, string string2, string string3)
        {
            return str.Contains(string1) || str.Contains(string2) || str.Contains(string3);
        }

        #endregion

        #region Queue

        public static T SafePeek<T>(this Queue<T> queue) =>
            queue.Count == 0 ? default : queue.Peek();

        public static T SafeDequeue<T>(this Queue<T> queue) =>
            queue.Count == 0 ? default : queue.Dequeue();

        #endregion

        #region HashSet

        /// <summary>
        /// Add multiple values to a hashset.
        /// </summary>
        public static void AddRange<T>(this HashSet<T> hashset, IEnumerable<T> values)
        {
            if (hashset == null || values == null)
                return;

            foreach (var value in values)
            {
                hashset.Add(value);
            }
        }

        /// <summary>
        /// LINQ-extension method for HashSet, using .Any query.
        /// </summary>
        /// <param name="hashset"></param>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static bool ContainsKey<TKey, TValue>(this HashSet<KeyValuePair<TKey, TValue>> hashset, TKey key) =>
            hashset.Any(x => x.Key.Equals(key));

        #endregion

        #region Vector2

        public static Vector2 VectorParse(string value)
        {
            var stringedVector = value.Substring(1, value.Length - 1 - 1);

            var stringedVectorValues = stringedVector.Split(',');

            System.Globalization.NumberStyles parseStyle =
                System.Globalization.NumberStyles.Float |
                System.Globalization.NumberStyles.AllowThousands;

            return new Vector2
            (
                float.Parse(stringedVectorValues[0], parseStyle),
                float.Parse(stringedVectorValues[1], parseStyle)
            );
        }

        /// <summary>
        /// Returns whether the distance between vectors is less than range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        /// <useful>
        /// 43.82 is the approximate sqrt of 1920 (laptop fullscreen width).
        /// 32.86 is the approximate sqrt of 1080 (laptop fullscreen height).
        /// </useful>
        public static bool InRange(this Vector2 a, Vector2 b, int range)
        {
            if (Mathf.Abs(a.x - b.x) > range || Mathf.Abs(a.y - b.y) > range)
                return false;

            return range << 2 > ((int)(a.x - b.x) << 2) + ((int)(a.y - b.y) << 2);
        }

        /// <summary>
        /// Returns whether the distance between vectors is less than range (precise).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        /// <useful>
        /// 43.82 is the approximate sqrt of 1920 (not laptop fullscreen width).
        /// 32.86 is the approximate sqrt of 1080 (not laptop fullscreen height).
        /// </useful>
        public static bool InRange(this Vector2 a, Vector2 b, float range)
        {
            if (Mathf.Abs(a.x - b.x) > range || Mathf.Abs(a.y - b.y) > range)
                return false;

            return Mathf.Pow(range, 2) > Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2);
        }

        /// <summary>
        /// Returns whether the distance between vectors is less than range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        /// <useful>
        /// 43.82 is the approximate sqrt of 1920 (laptop fullscreen width).
        /// 32.86 is the approximate sqrt of 1080 (laptop fullscreen height).
        /// </useful>
        public static bool InRange(this Vector3 a, Vector3 b, int range) =>
            ((Vector2)a).InRange(b, range);

        /// <summary>
        /// Returns whether the distance between vectors is less than range (precise).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        /// <useful>
        /// 43.82 is the approximate sqrt of 1920 (not laptop fullscreen width).
        /// 32.86 is the approximate sqrt of 1080 (not laptop fullscreen height).
        /// </useful>
        public static bool InRange(this Vector3 a, Vector3 b, float range) =>
            ((Vector2)a).InRange(b, range);

        #endregion

        #region Time

        private class Time : UnityTime
        {
            public static bool Elapsed(double cache, double interval)
            {
                return time - cache > interval;
            }
        }

        public static bool TimeElapsed(double timeCache, double timeInterval) =>
            Time.Elapsed(timeCache, timeInterval);

        #endregion

        #region Random

        /// <summary>
        /// Return a random integer number between min [inclusive] and max [exclusive], with exclusion.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="exclude">Number to exclude.</param>
        public static int XRange(int min, int max, int exclude)
        {
            int random = Random.Range(min, max);

            for (; exclude == random; random = Random.Range(min, max)) ;

            return random;
        }

        /// <summary>
        /// Return a random integer number between min [inclusive] and max [exclusive], with exclusion.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="exclude">Numbers to exclude.</param>
        /// <returns></returns>
        public static int XRange(int min, int max, HashSet<int> exclude)
        {
            int random = Random.Range(min, max);

            for (; exclude.Contains(random); random = Random.Range(min, max)) ;

            return random;
        }

        #endregion

        #region Texture2D

        public static bool IsTransparent(this Texture2D tex, RectInt block)
        {
            if (tex == null)
                throw new ArgumentNullException(nameof(tex));

            var pixels = tex.GetPixels(block.x, block.y, block.width, block.height);

            for (int i = 0; i < pixels.Length; i++)
                if (pixels[i].a != 0)
                    return false;

            return true;
        }

        #endregion

        #region Visual Element

        /// <summary>
        /// Sets the minimum width of the visual element's style.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="width"></param>
        public static void SetMinWidth(this VisualElement elem, float width)
        {
            elem.style.minWidth = width;
        }

        /// <summary>
        /// Sets the maximum width of the visual element's style.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="width"></param>
        public static void SetMaxWidth(this VisualElement elem, float width)
        {
            elem.style.maxWidth = width;
        }

        /// <summary>
        /// Sets the minimum height of the visual element's style.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="width"></param>
        public static void SetMinHeight(this VisualElement elem, float height)
        {
            elem.style.minHeight = height;
        }

        /// <summary>
        /// Sets the maximum height of the visual element's style.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="width"></param>
        public static void SetMaxHeight(this VisualElement elem, float height)
        {
            elem.style.maxHeight = height;
        }

        #endregion

        #region Component

        public static bool TryGetComponentInParent<T>(this Component self, out T component, bool includeSelf = false)
        {
            component = default;

            if (self == null)
                return false;

            if (includeSelf && self.TryGetComponent(out component))
                return true;

            for (var parent = self.transform.parent; parent != null; parent = parent.parent)
                if (parent.TryGetComponent(out component))
                    return true;

            return false;
        }

        public static bool TryGetComponentInChildren<T>(this Component self, out T component, bool includeSelf = false)
        {
            component = default;

            if (self == null)
                return false;

            if (includeSelf && self.TryGetComponent(out component))
                return true;

            for (int i = 0; i < self.transform.childCount; ++i)
                if (self.transform.GetChild(i).TryGetComponentInChildren(out component, true))
                    return true;

            return false;
        }

        #endregion

        #region MonoBehaviour

        public static bool DestroysOnLoad(this MonoBehaviour self)
        {
            if (self == null)
                throw new ArgumentNullException();

            return self.gameObject.scene.buildIndex != -1;
        }

        public static void GetOrThrow<T>(this MonoBehaviour self, ref T component) where T : Component
        {
            if (component != null)
                return;

            if (self == null)
                throw new ArgumentNullException(nameof(self));

            component = self.GetComponent<T>();

            if (component == null)
                throw new MissingComponentException(typeof(T).FullName);
        }

        #endregion

        #region GameObject

        public static bool DestroysOnLoad(this GameObject self)
        {
            if (self == null)
                throw new ArgumentNullException();

            return self.scene.buildIndex != -1;
        }

        public static void SafeDestroy<T>(this GameObject gameObject) where T : UnityObject
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            if (!gameObject.TryGetComponent<T>(out var component))
            {
                Debug.LogWarning($"{Warnings.UnableToGetComponent<T>(gameObject)}.");

                return;
            }

            if (Application.isPlaying)
            {
                UnityObject.Destroy(component);
            }
            else
            {
                UnityObject.DestroyImmediate(component);
            }

            Debug.LogWarning($"{Warnings.RemovedComponent<T>(gameObject)}.");
        }

        public static void SafeDestroy(this GameObject gameObject, Type type)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            if (!gameObject.TryGetComponent(type, out var component))
            {
                Debug.LogWarning($"{Warnings.UnableToGetComponent(type, gameObject)}.");

                return;
            }

            if (Application.isPlaying)
            {
                UnityObject.Destroy(component);
            }
            else
            {
                UnityObject.DestroyImmediate(component);
            }

            Debug.LogWarning($"{Warnings.RemovedComponent(type, gameObject)}.");
        }

        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component, bool includeSelf = false)
        {
            component = default;

            if (gameObject == null)
                return false;

            if (includeSelf && gameObject.TryGetComponent(out component))
                return true;

            for (var parent = gameObject.transform.parent; parent != null; parent = parent.parent)
                if (parent.TryGetComponent(out component))
                    return true;

            return false;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component, bool includeSelf = false)
        {
            component = default;

            if (gameObject == null)
                return false;

            if (includeSelf && gameObject.TryGetComponent(out component))
                return true;

            for (int i = 0; i < gameObject.transform.childCount; ++i)
                if (gameObject.transform.GetChild(i).TryGetComponentInChildren(out component, true))
                    return true;

            return false;
        }

        #endregion

        #region Extra

        /// TODO: Apply force instead of modifying velocity directly.

        /// <summary>
        /// Returns an accelerated speed. Defaulted to the right.
        /// </summary>
        /// <param name="speed">The current speed.</param>
        /// <param name="max">The max speed.</param>
        /// <param name="accel">The acceleration.</param>
        /// <param name="right">Direction.</param>
        /// <returns></returns>
        public static float Accelerate(float speed, float max, float accel, bool right = true)
        {
            if (Mathf.Abs(speed) > max)
            {
                if (speed > max)
                {
                    if (right)
                    {
                        speed = max;
                    }
                    else
                    {
                        speed -= max * max * accel * UnityTime.deltaTime;
                    }
                }
                else
                {
                    if (!right)
                    {
                        speed = -max;
                    }
                    else
                    {
                        speed += max * max * accel * UnityTime.deltaTime;
                    }
                }

                return speed;
            }
            else
            {
                if (right)
                {
                    if (speed < max)
                    {
                        speed += max * accel * UnityTime.deltaTime;
                    }
                    else
                    {
                        speed = max;
                    }
                }
                else
                {
                    if (speed > -max)
                    {
                        speed += -max * accel * UnityTime.deltaTime;
                    }
                    else
                    {
                        speed = -max;
                    }
                }

                return speed;
            }
        }

        /// <summary>
        /// Returns a decelarated speed, or zero.
        /// </summary>
        /// <param name="speed">The current speed.</param>
        /// <param name="decel">The deceleration.</param>
        /// <returns></returns>
        public static float Decelerate(float speed, float decel)
        {
            if (Mathf.Abs(speed) < 0.01f)
            {
                return 0;
            }
            else
            {
                speed -= speed * decel * UnityTime.deltaTime;
            }

            return speed;
        }

        /// <summary>
        /// Provides an upward, jumping force to the given rigidbody using the provided jump speed.
        /// </summary>
        /// <param name="rb"></param>
        public static void Jump(Rigidbody2D rb, float jumpSpeed)
        {
            // Cancel out any velocity to avoid having the jump force be affected.
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            // Apply the jump force to the Player.
            rb.AddForce(new Vector2(0.0f, jumpSpeed), ForceMode2D.Impulse);
        }

        #endregion

        #region UNITY_EDITOR
#if UNITY_EDITOR

        #region AssetDatabase

        public static bool AssetExists<T>() where T : UnityObject
        {
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

            for (int j = 0; j < guids.Length; j++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);

                if (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
                    return true;
            }

            return false;
        }

        public static bool AssetExists(Type type)
        {
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", type));

            for (int j = 0; j < guids.Length; j++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);

                if (AssetDatabase.LoadAssetAtPath(assetPath, type) != null)
                    return true;
            }

            return false;
        }

        public static IList<T> FindAssetsOfType<T>(string filter, string[] searchInFolders = null) where T : UnityObject
        {
            string[] guids;

            if (searchInFolders == null)
            {
                guids = AssetDatabase.FindAssets(filter);
            }
            else
            {
                guids = AssetDatabase.FindAssets(filter, searchInFolders);
            }

            IList<T> list = new List<T>();

            for (int i = 0; i < guids.Length; ++i)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

                if (string.IsNullOrWhiteSpace(assetPath))
                    continue;

                list.Add(AssetDatabase.LoadAssetAtPath<T>(assetPath));
            }

            return list;
        }

        #endregion

#endif
        #endregion
    }
}