using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.ShallowFluid
{
    public class RelativePositionCalculator<T>
    {
        /// <summary>
        /// The relative position of neighbouring nodes in terms of the tangent space at a graph node on a sphere. 
        /// </summary>
        public Dictionary<T, VectorField<T>> RelativePositions 
        { 
            get
            {
                return _relativePositions ?? (_relativePositions = CalculateRelativePositions(_graph, _positions));
            } 
        }
        private Dictionary<T, VectorField<T>> _relativePositions; 
    
        private readonly Graph<T> _graph;
        private readonly VectorField<T> _positions; 

        public RelativePositionCalculator(Graph<T> graph, VectorField<T> positions)
        {
            _graph = graph;
            _positions = positions;
        }

        // This backs the lazy getter for the relative position field.
        private Dictionary<T, VectorField<T>> CalculateRelativePositions(Graph<T> graph, VectorField<T> positions)
        {
            var allRelativePositions = new Dictionary<T, VectorField<T>>();

            foreach (var nodeAndNeighbours in graph)
            {
                var node = nodeAndNeighbours.Key;
                var neighbours = nodeAndNeighbours.Value;

                var positionsRelativeToNode = new VectorField<T>();

                foreach (var neighbour in neighbours)
                {
                    var relativePosition = CalculateLocalRelativePosition(node, neighbour, positions);
                    positionsRelativeToNode.Add(neighbour, relativePosition);
                }
                allRelativePositions.Add(node, positionsRelativeToNode);    
            }

            return allRelativePositions;
        }

        // Calculates the position of target with respect to a Euclidean coordinate system centered at origin.
        private Vector3 CalculateLocalRelativePosition(T origin, T target, VectorField<T> positions)
        {
            var originPosition = positions[origin];
            var targetPosition = positions[target];
            var globalRelativePosition = targetPosition - originPosition;
            
            // This is (approximately) the distance from the origin to the target if you travelled along a great circle.
            // Approximate because it doesn't account for the origin & target being at different heights.
            var angularSeparation = Vector3.Angle(targetPosition, originPosition) * Mathf.PI / 180;
            var greatCircleDistance =  originPosition.magnitude*angularSeparation;

            // This is the vector from origin to target when constrained to the tangent plane at origin.
            var vectorTowardsTarget = PerpendicularComponent(globalRelativePosition, originPosition);
            
            var localRelativePosition = greatCircleDistance * vectorTowardsTarget.normalized;

            return localRelativePosition;
        }

        // Returns the component of the vector u that's perpendicular to v
        private Vector3 PerpendicularComponent(Vector3 u, Vector3 v)
        {
            return u - Vector3.Dot(u, v.normalized)*v;
        }
    }
}
