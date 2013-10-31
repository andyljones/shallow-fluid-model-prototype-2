using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ClimateSim.Grids
{
    /// <summary>
    /// Comparer for ordering vectors according to which is further clockwise around a point, judged from global 
    /// north (0, 0, 1). Uses a left-hand coordinate system.
    /// </summary>
    public class CompareVectorsClockwiseAround : IComparer<Vector3>
    {
        private Vector3 _localX;
        private Vector3 _localY;
        private Vector3 _localZ;

        private readonly Vector3 _center;

        public CompareVectorsClockwiseAround(Vector3 center)
        {
            _center = center;

            SetLocalCoordinateSystem();
        }

        public void SetLocalCoordinateSystem()
        {
            _localZ = _center.normalized;
            _localX = Vector3.Cross(_center, new Vector3(0, 0, 1));
            _localY = Vector3.Cross(_localX, _localZ);
        }

        /// <summary>
        /// Calculates whether the first argument is further around from north than the second. Returns 1 if so, 0 or -1 otherwise. 
        /// </summary>
        /// <param name="u">First vector</param>
        /// <param name="v">Second vector</param>
        /// <returns></returns>
        public int Compare(Vector3 u, Vector3 v)
        {
            // This is all doable with crossproducts, but projecting onto the local coordinate system and calculating 
            // angles is conceptually simpler.
            float u_x = Vector3.Dot(u - _center, _localX);
            float u_y = Vector3.Dot(u - _center, _localY);
            float v_x = Vector3.Dot(v - _center, _localX);
            float v_y = Vector3.Dot(v - _center, _localY);

            // Although Atan2 has arguments (y, x), here it's provided (x, y) because we want the angle from the positive Y axis. 
            float uAngleFromPositiveY = MathMod(Mathf.Atan2(u_x, u_y), 2*Mathf.PI);
            float vAngleFromPositiveY = MathMod(Mathf.Atan2(v_x, v_y), 2*Mathf.PI);

            return Math.Sign(uAngleFromPositiveY - vAngleFromPositiveY);
        }

        private float MathMod(float x, float m)
        {
            return ((x % m) + m) % m;
        }
    }
}
