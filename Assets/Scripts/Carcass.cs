public class Carcass
{
    private const int MeshSize = MapGenerator.MapSize;
    private readonly int[,] _heights = new int[MeshSize * 2 - 1, MeshSize * 2 - 1];

    public void SetHeight(int aPos, int bPos, int height)
    {
        _heights[aPos + MeshSize - 1, bPos + MeshSize - 1] = height;
    }

    public int GetHeight(int aPos, int bPos)
    {
        return _heights[aPos + MeshSize - 1, bPos + MeshSize - 1];
    }
}