public class CubeMapNode
{
    public int Index;
    public CubeColor CubeColor;
    public CubeMapNode RightNode;
    public CubeMapNode LeftNode;
    public CubeMapNode UpNode;
    public CubeMapNode DownNode;
    public CubeCtlr Obj;

    public CubeMapNode(CubeCtlr ctlr)
    {
        Index = ctlr.CubeID;
        Obj = ctlr;
        CubeColor = new CubeColor();
    }

}
