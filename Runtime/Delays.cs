using System;
using UnityEngine;
using System.Collections.Generic;

namespace Ikonoclast.Globals
{
    /// <summary>
    /// Provides global caching and retrieval of commonly used yield instructions.
    /// </summary>
    public static class Delays
    {
        #region Inner

        /// ---
        /// Compares two floating-point integers.
        private sealed class FloatComparer : IEqualityComparer<float>
        {
            public bool Equals(float x, float y)
            {
                return x == y;
            }

            public int GetHashCode(float obj) => obj.GetHashCode();
        }

        /// ---
        /// Compares two floating-points and their associated predicates.
        private sealed class FloatPredicateComparer : IEqualityComparer<(float flt, Func<bool> predicate)>
        {
            public bool Equals((float flt, Func<bool> predicate) x, (float flt, Func<bool> predicate) y)
            {
                return x.flt == y.flt && x.predicate.Method == y.predicate.Method;
            }

            public int GetHashCode((float, Func<bool>) obj) => obj.GetHashCode();
        }

        #endregion

        #region Fields

        private static readonly WaitForEndOfFrame
            _endOfFrame = new WaitForEndOfFrame();

        private static readonly WaitForFixedUpdate
            _fixedUpdate = new WaitForFixedUpdate();

        private static readonly Dictionary<float, WaitForSeconds>
            _waitForSeconds = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());

        private static readonly Dictionary<float, WaitForSecondsRealtime>
            _waitForRealtimes = new Dictionary<float, WaitForSecondsRealtime>(100, new FloatComparer());

        private static readonly Dictionary<(float, Func<bool>), WaitForDone>
            _waitForDones = new Dictionary<(float, Func<bool>), WaitForDone>(100, new FloatPredicateComparer());

        #endregion

        #region Properties

        public static WaitForEndOfFrame EndOfFrame =>
            _endOfFrame;

        public static WaitForFixedUpdate FixedUpdate =>
            _fixedUpdate;

        #endregion

        #region Methods

        /// <summary>
        /// Returns a preexisting or newly created yield instruction (cached).
        /// </summary>
        /// <param name="seconds">The amount of time to yield for.</param>
        public static WaitForSeconds Get(float seconds)
        {
            if (!_waitForSeconds.TryGetValue(seconds, out WaitForSeconds wfs))
            {
                _waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            }

            return wfs;
        }

        /// <summary>
        /// Returns a preexisting or newly created yield instruction (cached).
        /// </summary>
        /// <param name="seconds">The amount of time (unscaled) to yield for.</param>
        public static WaitForSecondsRealtime GetUnscaled(float seconds)
        {
            if (!_waitForRealtimes.TryGetValue(seconds, out var wfsr))
            {
                _waitForRealtimes.Add(seconds, wfsr = new WaitForSecondsRealtime(seconds));
            }

            return wfsr;
        }

        /// <summary>
        /// Returns a preexisting or newly created yield instruction (cached).
        /// </summary>
        /// <param name="seconds">The amount of time to yield for.</param>
        /// <param name="predicate">The boolean callback under which to stop yielding.</param>
        public static WaitForDone Get(float seconds, Func<bool> predicate)
        {
            if (!_waitForDones.TryGetValue((seconds, predicate), out WaitForDone wfd))
            {
                _waitForDones.Add((seconds, predicate), wfd = new WaitForDone(seconds, predicate));
            }

            return wfd;
        }

        #endregion
    }

    /// <summary>
    /// Suspends the coroutine execution for the given amount of seconds using scaled time 
    /// and breaks if the supplied delegate ever evaluates to true.
    /// </summary>
    public sealed class WaitForDone : CustomYieldInstruction
    {
        #region Fields

        /// <summary>
        /// Evaluated timeout float.
        /// </summary>
        private float timeout;

        /// <summary>
        /// Evaluated predicate.
        /// </summary>
        private Func<bool> predicate;

        /// <summary>
        /// Internal, non-changing timeout float.
        /// </summary>
        private readonly float iTimeout;

        #endregion

        #region Properties

        public override bool keepWaiting
        {
            get
            {
                if ((timeout -= Time.deltaTime) <= 0 || predicate())
                {
                    timeout = iTimeout;

                    return false;
                }

                return true;
            }
        }

        #endregion

        #region Constructors

        public WaitForDone(float timeout, Func<bool> predicate)
        {
            iTimeout = timeout;

            this.timeout = iTimeout;
            this.predicate = predicate;
        }

        #endregion
    }
}
