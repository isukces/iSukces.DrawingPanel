namespace iSukces.DrawingPanel.Paths.Test;

public class TestBaseClass
{
    static TestBaseClass()
    {
        PathCalculationConfig.CheckRadius = true;
            
        VariablesDictionary.GetVarName = type =>
        {
            if (type == typeof(PathRay))
                return "ray";
                
            if (type == typeof(ArcDefinition))
                return "arc";
            if (type == typeof(LinePathElement))
                return "line";
            if (type == typeof(WayPoint))
                return "waypoint";
                
                
                
            return null;
        };
    }
}