namespace iSukces.DrawingPanel.Paths.Test
{
    public class TestBaseClass
    {
        static TestBaseClass()
        {
            VariablesDictionary.GetVarName = type =>
            {
                if (type == typeof(PathRay))
                    return "ray";
                
                if (type == typeof(ArcDefinition))
                    return "arc";
                if (type == typeof(LinePathElement))
                    return "line";
                
                
                
                return null;
            };
        }
    }
}
