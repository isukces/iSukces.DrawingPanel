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
                return null;
            };
        }
    }
}
