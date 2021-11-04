namespace iSukces.DrawingPanel
{
    internal struct HolderWrapper
    {
        public HolderWrapper(INewHandler handler, int order)
        {
            Handler = handler;
            Order   = order;
        }

        public override string ToString()
        {
            return Handler?.ToString() ?? base.ToString();
        }

        public INewHandler Handler { get; }
        public int      Order   { get; }
    }
}