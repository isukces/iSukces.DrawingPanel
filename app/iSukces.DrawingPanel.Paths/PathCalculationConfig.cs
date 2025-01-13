#nullable disable
namespace iSukces.DrawingPanel.Paths;

public class PathCalculationConfig
{
    #region Fields

    /// <summary>
    ///     Maximum h of arc when arc is replaced by line
    ///     <see cref="https://raw.githubusercontent.com/isukces/iSukces.DrawingPanel/main/doc/vector_for_arc_compare.jpg">vector_for_arc_compare.jpg</see>
    /// </summary>
    public static double MaximumSagitta = 0.001;

    public static double UseLineWhenDistanceLowerThan = 1e-5;
    public static bool CheckRadius;


    public static double MinimumLinearEquationSystemDeterminantToUseLine = 1e-5;

    #endregion
}
