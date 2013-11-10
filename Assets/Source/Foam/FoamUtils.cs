using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Foam
{
    public static class FoamUtils
    {
        public static Cell NeighbourAcross(Face face, Cell cell)
        {
            return face.Cells.Single(neighbour => neighbour != cell);
        }

        public static float HorizontalAreaOf(Cell cell)
        {
            var areaOfTopFace = AreaOf(TopFaceOf(cell));
            var areaOfBottomFace = AreaOf(BottomFaceOf(cell));

            return (areaOfTopFace + areaOfBottomFace)/2;
        }

        public static float WidthOfVerticalFace(Face face)
        {
            var widthOfTopEdge = LengthOf(TopEdgeOf(face));
            var widthOfBottomEdge = LengthOf(BottomEdgeOf(face));

            return (widthOfTopEdge + widthOfBottomEdge)/2;
        }

        public static Edge BottomEdgeOf(Face face)
        {
            var verticesSortedByHeight = face.Vertices.OrderBy(vertex => vertex.Position.magnitude).ToList();

            var lowestVertex = verticesSortedByHeight[0];
            var secondLowestVertex = verticesSortedByHeight[1];

            var topEdge = lowestVertex.Edges.Intersect(secondLowestVertex.Edges).Single();

            return topEdge;
        }
        
        public static Edge TopEdgeOf(Face face)
        {
            var verticesSortedByHeight = face.Vertices.OrderByDescending(vertex => vertex.Position.magnitude).ToList();

            var highestVertex = verticesSortedByHeight[0];
            var secondHighestVertex = verticesSortedByHeight[1];

            var topEdge = highestVertex.Edges.Intersect(secondHighestVertex.Edges).Single();

            return topEdge;
        }

        public static float LengthOf(Edge edge)
        {
            return (edge.Vertices[0].Position - edge.Vertices[1].Position).magnitude;
        }

        public static float DistanceBetweenCellCentersAcross(Face face)
        {
            var centerOfCellA = CenterOf(face.Cells[0]);
            var centerOfCellB = CenterOf(face.Cells[1]);

            var distanceBetweenCellCenters = (centerOfCellB - centerOfCellA).magnitude;

            return distanceBetweenCellCenters;
        }

        public static float AreaOf(Face face)
        {
            var sumOfCrossProducts = new Vector3();

            for (int i = 0; i < face.Vertices.Count; i++)
            {
                var currentPosition = face.Vertices[i].Position;
                var nextPosition = face.Vertices[(i + 1) % face.Vertices.Count].Position;

                sumOfCrossProducts += Vector3.Cross(nextPosition, currentPosition);
            }

            var normalToFace = CenterOf(face).normalized;
            var area = Vector3.Dot(sumOfCrossProducts, normalToFace) / 2;

            return area;
        }

        public static Vector3 CenterOf(Cell cell)
        {
            var centerOfTopFace = CenterOf(TopFaceOf(cell));
            var centerOfBottomFace = CenterOf(BottomFaceOf(cell));

            var centerOfCell = (centerOfTopFace + centerOfBottomFace) / 2;

            return centerOfCell;
        }

        public static Vector3 CenterOf(Face face)
        {
            var sumOfVertexPositions = face.Vertices.Aggregate(new Vector3(), (position, vertex) => position + vertex.Position);
            var centerOfFace = sumOfVertexPositions / face.Vertices.Count;

            return centerOfFace;
        }

        public static List<Face> FacesWithNeighbours(Cell cell)
        {
            return cell.Faces.Where(face => face.Cells.Count > 1).ToList();
        }

        public static List<Edge> VerticalEdgesOf(Cell cell)
        {
            var topFace = TopFaceOf(cell);
            var bottomFace = BottomFaceOf(cell);

            var verticalEdges = cell.Edges.Except(topFace.Edges).Except(bottomFace.Edges);

            return verticalEdges.ToList();
        }

        public static Face TopFaceOf(Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderByDescending(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringHighestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondHighestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdHighestVertex = verticesOrderedByHeight[2].Faces;

            var highestFace =
                facesNeighbouringHighestVertex
                .Intersect(facesNeighbouringSecondHighestVertex)
                .Intersect(facesNeighbouringThirdHighestVertex);

            return highestFace.Single();
        }

        public static Face BottomFaceOf(Cell cell)
        {
            var verticesOrderedByHeight = cell.Vertices.OrderBy(vertex => vertex.Position.magnitude).ToList();

            var facesNeighbouringLowestVertex = verticesOrderedByHeight[0].Faces;
            var facesNeighbouringSecondLowestVertex = verticesOrderedByHeight[1].Faces;
            var facesNeighbouringThirdLowestVertex = verticesOrderedByHeight[2].Faces;

            var lowestFace =
                facesNeighbouringLowestVertex
                .Intersect(facesNeighbouringSecondLowestVertex)
                .Intersect(facesNeighbouringThirdLowestVertex);

            return lowestFace.Single();
        }
    }
}
