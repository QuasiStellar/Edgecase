using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    private int _aPos;
    private int _bPos;
    private readonly int _height;

    private readonly GameObject _hexGameObject;

    private const float HexSize = 10;
    
    private const int HeightVariation = 4;
    private const int StairHeight = 3;

    private static readonly Dictionary<int, Color> ColorMap = new Dictionary<int, Color>
    {
        { 0, new Color(1, 0, 0) }, 
        { 1, new Color(1, 0.5f, 0) },
        { 2, new Color(1, 1, 0) },
        { 3, new Color(0, 1, 0) },
        { 4, new Color(0, 1, 1) },
        { 5, new Color(0, 0, 1) },
        { 6, new Color(1, 0, 1) }
    };
    
    public Hex(int aPos, int bPos, int height)
    {
        _aPos = aPos;
        _bPos = bPos;
        _height = height;
        
        _hexGameObject = new GameObject("Hex");
        _hexGameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("HexMaterial");
        _hexGameObject.AddComponent<MeshFilter>().mesh = new Mesh
        {
            vertices = new[]
            {
                new Vector3(0, height * StairHeight, HexSize / 2),
                new Vector3(HexSize * Mathf.Pow(3, 0.5f) / 4, height * StairHeight, HexSize / 4),
                new Vector3(HexSize * Mathf.Pow(3, 0.5f) / 4, height * StairHeight, - HexSize / 4),
                new Vector3(0, height * StairHeight, - HexSize / 2),
                new Vector3(- HexSize * Mathf.Pow(3, 0.5f) / 4, height * StairHeight, - HexSize / 4),
                new Vector3(- HexSize * Mathf.Pow(3, 0.5f) / 4, height * StairHeight, HexSize / 4),
                
                new Vector3(0, 0, HexSize / 2),
                new Vector3(HexSize * Mathf.Pow(3, 0.5f) / 4, 0, HexSize / 4),
                new Vector3(HexSize * Mathf.Pow(3, 0.5f) / 4, 0, - HexSize / 4),
                new Vector3(0, 0, - HexSize / 2),
                new Vector3(- HexSize * Mathf.Pow(3, 0.5f) / 4, 0, - HexSize / 4),
                new Vector3(- HexSize * Mathf.Pow(3, 0.5f) / 4, 0, HexSize / 4)
            },
            triangles = new[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 5,
                3, 4, 5,
                
                0, 6, 1,
                1, 6, 7,
                
                1, 7, 2,
                2, 7, 8,
                
                2, 8, 3,
                3, 8, 9,
                
                3, 9, 4,
                4, 9,10,
                
                4,10, 5,
                5,10,11,
                
                5,11, 0,
                0,11, 6
            },
            uv = new[]
            {
                new Vector2(0.5f, 1/6f + 0.01f),
                new Vector2(0.75f - 0.01f, 1/3f + 0.01f),
                new Vector2(0.75f - 0.01f, 2/3f - 0.01f),
                new Vector2(0.5f, 5/6f - 0.01f),
                new Vector2(0.25f + 0.01f, 2/3f - 0.01f),
                new Vector2(0.25f + 0.01f, 1/3f + 0.01f),
                new Vector2(0.5f, 0),
                new Vector2(1, 1/6f),
                new Vector2(1, 5/6f),
                new Vector2(0.5f, 1),
                new Vector2(0, 5/6f),
                new Vector2(0, 1/6f)
            }
        };
        _hexGameObject.GetComponent<Transform>().position = new Vector3(
            (aPos - bPos / 2f) * HexSize * Mathf.Pow(3, 0.5f) / 2,
            0,
            bPos * HexSize * 3 / 4
        );
    }

    public void UpdateColor()
    {
        _hexGameObject.GetComponent<MeshRenderer>().material.color = ColorMap[_height];
    }

    public static int GetRandomHeight()
    {
        return Random.Range(0, HeightVariation);
    }
}