using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private readonly GameObject _hexGameObject;

    private const float HexSize = MapGenerator.HexSize;
    private const int MapSize = MapGenerator.MapSize;
    private const int TriangleSize = MapGenerator.TriangleSize;
    private const int StairHeight = MapGenerator.StairHeight;

    private const float Sqrt3By2 = 0.866025403784439f; // Mathf.Pow(3, 0.5f) / 2
    private const float Sqrt3By4 = 0.433012701892219f; // Mathf.Pow(3, 0.5f) / 4

    private const int HeightUnderZero = 0;

    private const int LowerY = HeightUnderZero * StairHeight;

    private static readonly Dictionary<int, Color> ColorMap = new Dictionary<int, Color>
    {
        {0, new Color(1, 0, 0)},
        {1, new Color(1, 0.5f, 0)},
        {2, new Color(1, 1, 0)},
        {3, new Color(0, 1, 0)},
        {4, new Color(0, 1, 1)},
        {5, new Color(0, 0, 1)},
        {6, new Color(1, 0, 1)}
    };

    public Map(Carcass carcass)
    {
        _hexGameObject = new GameObject("Hex");
        _hexGameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("HexMaterial");

        var verticesList = new List<Vector3>();
        var trianglesList = new List<int>();
        var uvList = new List<Vector2>();
        var verticesCounter = 0;

        // Creating hexes
        for (var i = 0; i < 2 * MapSize - 1; i++)
        {
            for (var j = 0; j < 2 * MapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= MapSize) continue;

                var aPos = i - MapSize + 1;
                var bPos = j - MapSize + 1;

                var height = carcass.GetHeight(aPos, bPos);
                var upperY = height * StairHeight;

                var coordinateShiftAPos = (aPos - bPos / 2f) * HexSize * Sqrt3By2;
                var coordinateShiftBPos = bPos * HexSize * 3 / 4;
                coordinateShiftAPos += aPos * TriangleSize;
                coordinateShiftBPos += bPos * TriangleSize * Sqrt3By2;
                coordinateShiftAPos -= bPos * (TriangleSize / 2f);

                verticesList.AddRange(new[]
                {
                    new Vector3(coordinateShiftAPos + 0, upperY,
                        coordinateShiftBPos + HexSize / 2),
                    new Vector3(coordinateShiftAPos + HexSize * Sqrt3By4, upperY,
                        coordinateShiftBPos + HexSize / 4),
                    new Vector3(coordinateShiftAPos + HexSize * Sqrt3By4, upperY,
                        coordinateShiftBPos - HexSize / 4),
                    new Vector3(coordinateShiftAPos + 0, upperY,
                        coordinateShiftBPos - HexSize / 2),
                    new Vector3(coordinateShiftAPos - HexSize * Sqrt3By4, upperY,
                        coordinateShiftBPos - HexSize / 4),
                    new Vector3(coordinateShiftAPos - HexSize * Sqrt3By4, upperY,
                        coordinateShiftBPos + HexSize / 4)
                });

                trianglesList.AddRange(new[]
                {
                    0 + verticesCounter, 1 + verticesCounter, 2 + verticesCounter,
                    0 + verticesCounter, 2 + verticesCounter, 3 + verticesCounter,
                    0 + verticesCounter, 3 + verticesCounter, 5 + verticesCounter,
                    3 + verticesCounter, 4 + verticesCounter, 5 + verticesCounter
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

                verticesCounter += 6;
            }
        }

        // Creating triangles between hexes
        var hexCounter = 0;
        for (var i = 0; i < 2 * MapSize - 1; i++)
        {
            for (var j = 0; j < 2 * MapSize - 1; j++)
            {
                if (Math.Abs(i - j) >= MapSize) continue;
                var verticesShiftGrowing = (MapSize + 1) * 6 + (hexCounter + i) * 6;
                var verticesShiftDecreasing = (MapSize + 1) * 6 + (hexCounter - i + (MapSize * 2 - 3)) * 6;

                // Creating up-down triangles
                if (j + 1 < 2 * MapSize - 1 && i < 2 * MapSize - 2 && Math.Abs(i - (j + 1)) < MapSize)
                {
                    if (i < MapSize - 1)
                        trianglesList.AddRange(new[]
                        {
                            0 + hexCounter * 6,
                            8 + hexCounter * 6,
                            4 + verticesShiftGrowing
                        });
                    else
                        trianglesList.AddRange(new[]
                        {
                            0 + hexCounter * 6,
                            8 + hexCounter * 6,
                            4 + verticesShiftDecreasing
                        });
                }

                // Creating upright triangles
                if (j + 1 < 2 * MapSize - 1 && i < 2 * MapSize - 2 && Math.Abs((i + 1) - j) < MapSize)
                {
                    if (i < MapSize - 1)
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            3 + verticesShiftGrowing,
                            -1 + verticesShiftGrowing
                        });
                    else
                        trianglesList.AddRange(new[]
                        {
                            1 + hexCounter * 6,
                            3 + verticesShiftDecreasing,
                            -1 + verticesShiftDecreasing
                        });
                }

                // Creating quadrilateral
                if (j + 1 < 2 * MapSize - 1 && Math.Abs(i - (j + 1)) < MapSize)
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

                if (j + 1 < 2 * MapSize - 1 && i + 1 < 2 * MapSize - 1)
                {
                    if (i < MapSize - 1)
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

                if (i < 2 * MapSize - 2 && Math.Abs((i + 1) - j) < MapSize)
                {
                    if (i < MapSize - 1)
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

        _hexGameObject.AddComponent<MeshFilter>().mesh = new Mesh
        {
            vertices = verticesList.ToArray(),
            triangles = trianglesList.ToArray(),
            uv = uvList.ToArray(),
        };

        _hexGameObject.GetComponent<MeshRenderer>().material.color = ColorMap[3];
    }
}