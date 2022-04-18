using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Utils;

public class Hex
{
    private const int IndexCount = 48;
    private const int UpwardIndexCount = 12;
    private const int SidewardIndexCount = 36;

    private const int PillarHeight = 1000;

    private readonly GameObject _go;
    private int _aPos;
    private int _bPos;
    private int _height;

    public Hex
    (
        int aPos,
        int bPos,
        int height,
        float hexSize,
        float stairHeight,
        Material hexTopMaterial,
        Material hexSideMaterial
    )
    {
        _aPos = aPos;
        _bPos = bPos;
        _height = height;

        _go = new GameObject("Hex");

        _go.AddComponent<MeshRenderer>().materials = new[] { hexTopMaterial, hexSideMaterial };

        var topY = height * stairHeight;

        var topVertices = new[]
        {
            new Vector3(0, topY, hexSize / 2),
            new Vector3(hexSize * Numbers.Sqrt3By4, topY, hexSize / 4),
            new Vector3(hexSize * Numbers.Sqrt3By4, topY, -hexSize / 4),
            new Vector3(0, topY, -hexSize / 2),
            new Vector3(-hexSize * Numbers.Sqrt3By4, topY, -hexSize / 4),
            new Vector3(-hexSize * Numbers.Sqrt3By4, topY, hexSize / 4)
        };
        var bottomVertices = new[]
        {
            new Vector3(0, topY - PillarHeight, hexSize / 2),
            new Vector3(hexSize * Numbers.Sqrt3By4, topY - PillarHeight, hexSize / 4),
            new Vector3(hexSize * Numbers.Sqrt3By4, topY - PillarHeight, -hexSize / 4),
            new Vector3(0, topY - PillarHeight, -hexSize / 2),
            new Vector3(-hexSize * Numbers.Sqrt3By4, topY - PillarHeight, -hexSize / 4),
            new Vector3(-hexSize * Numbers.Sqrt3By4, topY - PillarHeight, hexSize / 4)
        };

        var mesh = new Mesh
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
            }
        };

        var indexList = new[]
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
        };

        mesh.SetIndexBufferParams(IndexCount, IndexFormat.UInt32);
        mesh.SetIndexBufferData(indexList, 0, 0, IndexCount);

        mesh.subMeshCount = 2;

        mesh.SetSubMesh(0, new SubMeshDescriptor(0, UpwardIndexCount));
        mesh.SetSubMesh(1, new SubMeshDescriptor(UpwardIndexCount, SidewardIndexCount));

        _go.AddComponent<MeshFilter>().mesh = mesh;

        _go.transform.position = new Vector3
        (
            (aPos - bPos / 2f) * hexSize * Numbers.Sqrt3By2,
            0,
            bPos * hexSize * 3 / 4
        );
    }

    public void SetParent(GameObject parent)
    {
        _go.transform.SetParent(parent.transform);
    }
}