using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foam
{
    public static class DeepCopyMethods
    {
        public static List<Face> DeepCopy(this List<Face> oldFaces)
        {
            var faceDictionary = DeepCopyDictionary(oldFaces);

            return faceDictionary.Values.ToList();
        }

        // This deep copy method returns a dictionary relating the old faces to their copies.
        public static Dictionary<Face, Face> DeepCopyDictionary(this List<Face> oldFaces)
        {
            var oldVertices = oldFaces.SelectMany(face => face.Vertices).Distinct();
            var oldEdges = oldFaces.SelectMany(face => face.Edges).Distinct();

            // We associate each old polyhedron element with a new one by using the old element as a key.
            var vertexDictionary = oldVertices.ToDictionary(oldVertex => oldVertex, oldVertex => new Vertex { Position = oldVertex.Position });
            var edgeDictionary = oldEdges.ToDictionary(oldEdge => oldEdge, oldEdge => new Edge());
            var faceDictionary = oldFaces.ToDictionary(oldFace => oldFace, oldFace => new Face());

            Link(vertexDictionary, edgeDictionary);
            Link(edgeDictionary, faceDictionary);
            Link(faceDictionary, vertexDictionary);

            return faceDictionary;
        }

        // Suppose T is a Vertex and U is a Edge. This function links all the new Vertices stored in tDictionary.Values to 
        // all the new Edges stored in uDictionary.Values in the same way that the old Vertices in tDictionary.Keys are 
        // linked to the old Edges in uDictionary.Keys.
        //
        // Because it's a generic method, it can be reused for linking any two kinds of polyhedron element.
        private static void Link<T, U>(Dictionary<T, T> tDictionary, Dictionary<U, U> uDictionary)
        {
            // TODO: This will have issues if there's more than one field of a given type in a polyhedron element. Mark the relevant field with an attribute?
            var uPropertyInT = typeof(T).GetFields().Single(fieldInfo => fieldInfo.FieldType == typeof(List<U>));
            var tPropertyInU = typeof(U).GetFields().Single(fieldInfo => fieldInfo.FieldType == typeof(List<T>));

            foreach (var uPair in uDictionary)
            {
                var oldU = uPair.Key;
                var newU = uPair.Value;

                var tListInOldU = tPropertyInU.GetValue(oldU) as List<T>;
                var tListInNewU = tPropertyInU.GetValue(newU) as List<T>;

                foreach (var oldT in tListInOldU)
                {
                    var newT = tDictionary[oldT];
                    var uListInNewT = uPropertyInT.GetValue(newT) as List<U>;

                    tListInNewU.Add(newT);
                    uListInNewT.Add(newU);
                }
            }
        }
    }
}
