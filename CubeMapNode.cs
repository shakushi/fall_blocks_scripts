namespace CubeOperater
{
    public class CubeMapNode
    {
        public enum CubeState
        {
            active,
            goto_vanish,
            sleep
        }
        public CubeState State;
        public int Index;
        public CubeColor CubeColor;
        public CubeMapNode RightNode;
        public CubeMapNode LeftNode;
        public CubeMapNode UpNode;
        public CubeMapNode DownNode;
        public CubeCtlr Obj;

        public CubeMapNode(CubeCtlr ctlr)
        {
            State = CubeState.active;
            Index = ctlr.CubeID;
            Obj = ctlr;
            CubeColor = new CubeColor();
            Obj.SetColor(CubeColor.Color);
        }

    }
}