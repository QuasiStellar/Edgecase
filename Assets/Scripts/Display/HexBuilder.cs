using System.Linq;
using UnityEngine;
using Utils;

namespace Display
{
    public static class HexBuilder
    {
        private const int PillarSize = 10;

        private static readonly int Radius = Shader.PropertyToID("_Radius");
        private static readonly int Height = Shader.PropertyToID("_Height");
        private static readonly int Thickness = Shader.PropertyToID("_Thickness");
        private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");

        public static GameObject Hex
        (
            int aPos,
            int bPos,
            int height,
            Material hexMaterial,
            float gameScale
        )
        {
            var hex = new GameObject("Hex");

            var hexSize = gameScale * 1f;
            var stairHeight = gameScale * 1.5f;
            var pillarHeight = gameScale * PillarSize;

            var topVertices = new[]
            {
                new Vector3(0, pillarHeight, hexSize / 2),
                new Vector3(hexSize * Numbers.Sqrt3By4, pillarHeight, hexSize / 4),
                new Vector3(hexSize * Numbers.Sqrt3By4, pillarHeight, -hexSize / 4),
                new Vector3(0, pillarHeight, -hexSize / 2),
                new Vector3(-hexSize * Numbers.Sqrt3By4, pillarHeight, -hexSize / 4),
                new Vector3(-hexSize * Numbers.Sqrt3By4, pillarHeight, hexSize / 4)
            };
            var bottomVertices = new[]
            {
                new Vector3(0, 0, hexSize / 2),
                new Vector3(hexSize * Numbers.Sqrt3By4, 0, hexSize / 4),
                new Vector3(hexSize * Numbers.Sqrt3By4, 0, -hexSize / 4),
                new Vector3(0, 0, -hexSize / 2),
                new Vector3(-hexSize * Numbers.Sqrt3By4, 0, -hexSize / 4),
                new Vector3(-hexSize * Numbers.Sqrt3By4, 0, hexSize / 4)
            };

            hex.AddComponent<MeshFilter>().mesh = new Mesh
            {
                vertices = topVertices
                    .Concat(topVertices)
                    .Concat(bottomVertices)
                    .Concat(topVertices)
                    .Concat(bottomVertices)
                    .ToArray(),
                normals = new[]
                {
                    Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up,
                    Vectors.FRight, Vectors.FRight, Vectors.BRight, Vectors.BRight, Vector3.left, Vector3.left,
                    Vectors.FRight, Vectors.FRight, Vectors.BRight, Vectors.BRight, Vector3.left, Vector3.left,
                    Vectors.FLeft, Vector3.right, Vector3.right, Vectors.BLeft, Vectors.BLeft, Vectors.FLeft,
                    Vectors.FLeft, Vector3.right, Vector3.right, Vectors.BLeft, Vectors.BLeft, Vectors.FLeft
                },
                triangles = new[]
                {
                    // Top
                    0, 1, 2,
                    0, 2, 3,
                    0, 3, 5,
                    3, 4, 5,
                    // Sides
                    6, 12, 7,
                    7, 12, 13,
                    19, 25, 20,
                    20, 25, 26,
                    8, 14, 9,
                    9, 14, 15,
                    21, 27, 22,
                    22, 27, 28,
                    10, 16, 11,
                    11, 16, 17,
                    23, 29, 18,
                    18, 29, 24
                },
                bounds = new Bounds(new Vector3(
                        0, pillarHeight / 2, 0),
                    new Vector3(Numbers.Sqrt3By2 * hexSize, pillarHeight, hexSize)
                )
            };

            hexMaterial = new Material(hexMaterial);
            hexMaterial.SetFloat(Radius, hexSize / 2);
            hexMaterial.SetFloat(Height, pillarHeight);
            hexMaterial.SetFloat(Thickness, hexSize / 5);
            hexMaterial.SetColor(EdgeColor, new Color(42 / 256f, 42 / 256f, 42 / 256f));
            // hexMaterial.SetColor(EdgeColor, new Color(52 / 256f, 204 / 256f, 255 / 256f));
            hex.AddComponent<MeshRenderer>().material = hexMaterial;

            hex.AddComponent<MeshCollider>();

            hex.AddComponent<HexController>();

            hex.transform.position = new Vector3
            (
                (aPos - bPos / 2f) * hexSize * Numbers.Sqrt3By2,
                height * stairHeight,
                bPos * hexSize * 3 / 4
            );

            return hex;
        }
    }
}