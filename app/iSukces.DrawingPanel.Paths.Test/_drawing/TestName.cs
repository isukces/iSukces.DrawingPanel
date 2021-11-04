namespace iSukces.DrawingPanel.Paths.Test
{
    public class TestName
    {
        public TestName(int testNumber, string groupName, string title)
        {
            TestNumber = testNumber;
            GroupName  = groupName;
            Title      = title;
        }

        public string GetDescription() { return $"{GroupName} {TestNumber:00},  {Title}"; }

        public string GetFileName()
        {
            return $"{GroupName}_{TestNumber:00}_{Title}.png".Replace(" ", "_").Replace("__", "_");
        }

        public string Title { get; set; }

        public string GroupName { get; set; }

        public int TestNumber { get; set; }
    }
}
