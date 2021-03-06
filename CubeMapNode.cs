public class CubeMapNode
{
    public int Index;
    public int Color;
    public CubeMapNode RightNode;
    public CubeMapNode LeftNode;
    public CubeMapNode UpNode;
    public CubeMapNode DownNode;
    public CubeCtlr Obj;

    public CubeMapNode(int index, int color)
    {
        Index = index;
        Color = color;
    }
}
