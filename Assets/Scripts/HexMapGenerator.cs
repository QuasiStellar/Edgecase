using System;
using System.Collections.Generic;
using UnityEngine;

public static class HexMapGenerator
{
    private const float Sqrt3By2 = 0.866025403784439f; // Mathf.Pow(3, 0.5f) / 2
    private const float Sqrt3By4 = 0.433012701892219f; // Mathf.Pow(3, 0.5f) / 4

    public static GameObject HexMap(
        int mapSize,
        int smoothness,
        int heightVariation,
        int hexSize,
        int stairHeight)
    {
        var heightMap = HeightMapGenerator.HeightMap(mapSize, smoothness, heightVariation);
        var mapGameObject = new GameObject("HexMap");
        mapGameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("HexMaterial");

        var verticesList = new List<Vector3>();
        var trianglesList = new List<int>();
        var uvList = new List<Vector2>();
        var vertexCounter = 0;

        // Creating hexes
        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;

                var aPos = i - mapSize + 1;
                var bPos = j - mapSize + 1;

                var height = heightMap[aPos + mapSize - 1, bPos + mapSize - 1];
                var upperY = height * stairHeight;

                var shiftAPos = (aPos - bPos / 2f) * hexSize * Sqrt3By2;
                var shiftBPos = bPos * hexSize * 3 / 4;

                verticesList.AddRange(new[]
                {
                    new Vector3(shiftAPos + 0, upperY, shiftBPos + hexSize / 2),
                    new Vector3(shiftAPos + hexSize * Sqrt3By4, upperY, shiftBPos + hexSize / 4),
                    new Vector3(shiftAPos + hexSize * Sqrt3By4, upperY, shiftBPos - hexSize / 4),
                    new Vector3(shiftAPos + 0, upperY, shiftBPos - hexSize / 2),
                    new Vector3(shiftAPos - hexSize * Sqrt3By4, upperY, shiftBPos - hexSize / 4),
                    new Vector3(shiftAPos - hexSize * Sqrt3By4, upperY, shiftBPos + hexSize / 4)
                });

                trianglesList.AddRange(new[]
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
            }
        }

        // Creating triangles between hexes
        var hexCounter = 0;
        for (var i = 0; i < 2 * mapSize - 1; i++)
        {
            for (var j = 0; j < 2 * mapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= mapSize) continue;
                var verticesShiftGrowing = (mapSize + 1) * 6 + (hexCounter + i) * 6;
                var verticesShiftDecreasing = (mapSize + 1) * 6 + (hexCounter - i + (mapSize * 2 - 3)) * 6;

                // Creating quadrilateral
                if (j + 1 < 2 * mapSize - 1 && Math.Abs(i - (j + 1)) < mapSize)
                {
                    trianglesList.AddRange(new[]
                    {
                        0 + hexCounter * 6,
                        9 + hexCounter * 6,
                        8 + hexCounter * 6,
                        5 + hexCounter * 6,
                        9 + hexCounter * 6,
                        0 + hexCounter * 6
                    });
                }

                if (j + 1 < 2 * mapSize - 1 && i + 1 < 2 * mapSize - 1)
                {
                    if (i < mapSize - 1)
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            0 + hexCounter * 6,
                            4 + verticesShiftGrowing,
                            4 + verticesShiftGrowing,
                            3 + verticesShiftGrowing,
                            1 + hexCounter * 6
                        });
                    else
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            0 + hexCounter * 6,
                            4 + verticesShiftDecreasing,
                            4 + verticesShiftDecreasing,
                            3 + verticesShiftDecreasing,
                            1 + hexCounter * 6
                        });
                }

                if (i < 2 * mapSize - 2 && Math.Abs((i + 1) - j) < mapSize)
                {
                    if (i < mapSize - 1)
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            5 + verticesShiftGrowing - 6,
                            2 + hexCounter * 6,
                            2 + hexCounter * 6,
                            5 + verticesShiftGrowing - 6,
                            4 + verticesShiftGrowing - 6
                        });
                    else
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            5 + verticesShiftDecreasing - 6,
                            2 + hexCounter * 6,
                            2 + hexCounter * 6,
                            5 + verticesShiftDecreasing - 6,
                            4 + verticesShiftDecreasing - 6
                        });
                }

                hexCounter++;
            }
        }

        mapGameObject.AddComponent<MeshFilter>().mesh = new Mesh
        {
            vertices = verticesList.ToArray(),
            triangles = trianglesList.ToArray(),
            uv = uvList.ToArray(),
        };

        return mapGameObject;
    }
}