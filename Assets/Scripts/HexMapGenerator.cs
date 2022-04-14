using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class HexMapGenerator
{
    private static Vector3 UpLeft => new Vector3(-0.5f, 0, Numbers.Sqrt3By2);
    private static Vector3 UpRight => new Vector3(0.5f, 0, Numbers.Sqrt3By2);
    private static Vector3 DownLeft => new Vector3(-0.5f, 0, -Numbers.Sqrt3By2);
    private static Vector3 DownRight => new Vector3(0.5f, 0, -Numbers.Sqrt3By2);

    public static GameObject HexMap(
        int mapSize,
        float hexSize,
        float stairHeight,
        HeightMapGenerator heightMapGenerator)
    {
        var heightMap = heightMapGenerator.HeightMap(mapSize);
        var mapGameObject = new GameObject("HexMap");

        // TODO: replace materials
        mapGameObject.AddComponent<MeshRenderer>().materials = new[]
            { Resources.Load<Material>("HexMaterial"), Resources.Load<Material>("HexMaterial") };

        var verticesList = new List<Vector3>();
        var upwardIndicesList = new List<int>();
        var sidewardIndicesList = new List<int>();
        var uvList = new List<Vector2>();

        var vertexCount = (mapSize * (mapSize - 1) * 3 + 1) * 6;
        var allVertexCount = vertexCount * 3;

        var vertexCounter = 0;
        var hexCounter = 0;
        
        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;

                // Creating hexes
                var height = heightMap[i, j];
                var upperY = height * stairHeight;

                var shiftAPos = (i - j / 2f) * hexSize * Numbers.Sqrt3By2;
                var shiftBPos = j * hexSize * 3 / 4;

                verticesList.AddRange(new[]
                {
                    new Vector3(shiftAPos, upperY, shiftBPos + hexSize / 2),
                    new Vector3(shiftAPos + hexSize * Numbers.Sqrt3By4, upperY, shiftBPos + hexSize / 4),
                    new Vector3(shiftAPos + hexSize * Numbers.Sqrt3By4, upperY, shiftBPos - hexSize / 4),
                    new Vector3(shiftAPos, upperY, shiftBPos - hexSize / 2),
                    new Vector3(shiftAPos - hexSize * Numbers.Sqrt3By4, upperY, shiftBPos - hexSize / 4),
                    new Vector3(shiftAPos - hexSize * Numbers.Sqrt3By4, upperY, shiftBPos + hexSize / 4)
                });

                upwardIndicesList.AddRange(new[]
                {
                    0 + vertexCounter, 1 + vertexCounter, 2 + vertexCounter,
                    0 + vertexCounter, 2 + vertexCounter, 3 + vertexCounter,
                    0 + vertexCounter, 3 + vertexCounter, 5 + vertexCounter,
                    3 + vertexCounter, 4 + vertexCounter, 5 + vertexCounter
                });

                uvList.AddRange(new[]
                {
                    new Vector2(0.5f, 1 / 6f + 0.01f),
                    new Vector2(0.75f - 0.01f, 1 / 3f + 0.01f),
                    new Vector2(0.75f - 0.01f, 2 / 3f - 0.01f),
                    new Vector2(0.5f, 5 / 6f - 0.01f),
                    new Vector2(0.25f + 0.01f, 2 / 3f - 0.01f),
                    new Vector2(0.25f + 0.01f, 1 / 3f + 0.01f)
                });

                vertexCounter += 6;

                // Creating quadrilaterals
                var shiftGrowing = (mapSize + 1) * 6 + (hexCounter + i) * 6;
                var shiftDecreasing = (mapSize + 1) * 6 + (hexCounter - i + (mapSize * 2 - 3)) * 6;

                // Top-left
                if (j < (mapSize - 1) * 2 && j - i < mapSize - 1)
                {
                    sidewardIndicesList.AddRange(new[]
                    {
                        0 + hexCounter * 6 + vertexCount,
                        9 + hexCounter * 6 + vertexCount * 2,
                        8 + hexCounter * 6 + vertexCount * 2,
                        5 + hexCounter * 6 + vertexCount,
                        9 + hexCounter * 6 + vertexCount * 2,
                        0 + hexCounter * 6 + vertexCount
                    });
                }

                var shift = i < mapSize - 1 ? shiftGrowing : shiftDecreasing;

                // Top-right
                if (j < (mapSize - 1) * 2 && i < (mapSize - 1) * 2)
                {
                    sidewardIndicesList.AddRange(new[]
                    {
                        1 + hexCounter * 6 + vertexCount * 2,
                        0 + hexCounter * 6 + vertexCount * 2,
                        4 + shift + vertexCount,
                        4 + shift + vertexCount,
                        3 + shift + vertexCount,
                        1 + hexCounter * 6 + vertexCount * 2
                    });
                }

                // Right
                if (i < (mapSize - 1) * 2 && i - j < mapSize - 1)
                {
                    sidewardIndicesList.AddRange(new[]
                    {
                        1 + hexCounter * 6 + vertexCount,
                        5 + shift - 6 + vertexCount * 2,
                        2 + hexCounter * 6 + vertexCount,
                        2 + hexCounter * 6 + vertexCount,
                        5 + shift - 6 + vertexCount * 2,
                        4 + shift - 6 + vertexCount * 2
                    });
                }

                hexCounter++;
            }
        }

        verticesList = verticesList.Concat(verticesList).Concat(verticesList).ToList();

        var normalsArray = new Vector3[allVertexCount];
        for (var i = 0; i < allVertexCount; i++)
        {
            normalsArray[i] = Vector3.up;
        }

        uvList.AddRange(Enumerable.Repeat(Vector2.zero, vertexCount * 2));

        var mesh = new Mesh
        {
            vertices = verticesList.ToArray(),
            normals = RecalculateNormals(normalsArray, heightMap, mapSize, vertexCount),
            uv = uvList.ToArray()
        };

        var indicesList = upwardIndicesList.Concat(sidewardIndicesList).ToList();

        mesh.SetIndexBufferParams(indicesList.Count, IndexFormat.UInt32);
        mesh.SetIndexBufferData(indicesList, 0, 0, indicesList.Count);

        var upwardIndexCount = vertexCount * 2; // 4 triangles (12 indices) per 6 vertices
        var sidewardIndexCount = indicesList.Count - upwardIndexCount;

        mesh.subMeshCount = 2;

        mesh.SetSubMesh(0, new SubMeshDescriptor(0, upwardIndexCount));
        mesh.SetSubMesh(1, new SubMeshDescriptor(upwardIndexCount, sidewardIndexCount));

        mapGameObject.AddComponent<MeshFilter>().mesh = mesh;

        mapGameObject.AddComponent<HexMapController>();

        return mapGameObject;
    }

    private static Vector3[] RecalculateNormals(Vector3[] normalsArray, int[,] heightMap, int mapSize, int vertexCount)
    {
        var hexCounter = 0;
        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;
                var verticesShiftGrowing = (mapSize + 1) * 6 + (hexCounter + i) * 6;
                var verticesShiftDecreasing = (mapSize + 1) * 6 + (hexCounter - i + (mapSize * 2 - 3)) * 6;

                // Top-left
                if (j < (mapSize - 1) * 2 && j - i < mapSize - 1)
                {
                    var direction = heightMap[i, j] > heightMap[i, j + 1] ? UpLeft : DownRight;
                    normalsArray[0 + hexCounter * 6 + vertexCount] = direction;
                    normalsArray[9 + hexCounter * 6 + vertexCount * 2] = direction;
                    normalsArray[8 + hexCounter * 6 + vertexCount * 2] = direction;
                    normalsArray[5 + hexCounter * 6 + vertexCount] = direction;
                }

                var shift = i < mapSize - 1 ? verticesShiftGrowing : verticesShiftDecreasing;

                // Top-right
                if (j < (mapSize - 1) * 2 && i < (mapSize - 1) * 2)
                {
                    var direction = heightMap[i, j] > heightMap[i + 1, j + 1] ? UpRight : DownLeft;
                    normalsArray[1 + hexCounter * 6 + vertexCount * 2] = direction;
                    normalsArray[0 + hexCounter * 6 + vertexCount * 2] = direction;
                    normalsArray[4 + shift + vertexCount] = direction;
                    normalsArray[3 + shift + vertexCount] = direction;
                }

                // Right
                if (i < (mapSize - 1) * 2 && i - j < mapSize - 1)
                {
                    var direction = heightMap[i, j] > heightMap[i + 1, j] ? Vector3.right : Vector3.left;
                    normalsArray[1 + hexCounter * 6 + vertexCount] = direction;
                    normalsArray[5 + shift - 6 + vertexCount * 2] = direction;
                    normalsArray[2 + hexCounter * 6 + vertexCount] = direction;
                    normalsArray[4 + shift - 6 + vertexCount * 2] = direction;
                }

                hexCounter++;
            }
        }

        return normalsArray;
    }
}