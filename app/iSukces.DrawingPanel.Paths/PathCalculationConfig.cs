namespace iSukces.DrawingPanel.Paths
{
    public class PathCalculationConfig
    {
        
        /// <summary>
        /// Maximum h of arc when arc is replaced by line
        /// <see cref="https://raw.githubusercontent.com/isukces/iSukces.DrawingPanel/main/doc/vector_for_arc_compare.jpg">vector_for_arc_compare.jpg</see>
        /// </summary>
        public static double MaximumSagitta = 0.001;

        public static bool CheckRadius;
    }
}
