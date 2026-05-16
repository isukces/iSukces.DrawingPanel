namespace iSukces.DrawingPanel.Paths.Test;

public class TestName
{
    public TestName(int testNumber, string groupName, string title)
    {
        TestNumber = testNumber;
        GroupName  = groupName;
        Title      = title;
    }

    public string GetDescription()
    {
        return $"{GroupName} {TestNumber:00},  {Title}";
    }

    public string GetFileName()
    {
        var s = $"{GroupName}_{TestNumber:00}_{Title}";
        s = s.ToFileName();
        return s;
    }


    public string Title { get; }

    public string GroupName { get; }

    public int TestNumber { get; }
}
