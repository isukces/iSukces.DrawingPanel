using System.Globalization;

namespace iSukces.DrawingPanel.Paths.Test;

public class VariablesDictionary
{
    public string GetName(Type t, out bool first)
    {
        var cnt = 0;
        foreach (var i in _list)
        {
            if (i.Typ != t) continue;
            cnt++;
            if (i.Busy)
                continue;
            i.Busy = true;
            first  = false;
            return i.Name;
        }

        var name = GetVarName?.Invoke(t);
        if (string.IsNullOrEmpty(name))
        {
            var nr = _list.Count + 1;
            name = "tmp" + nr.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            if (cnt > 0)
                name += cnt.ToString(CultureInfo.InvariantCulture);
        }

        var info = new Info
        {
            Busy = true,
            Name = name,
            Typ  = t
        };
        _list.Add(info);
        first = true;
        return info.Name;
    }

    public void Release(string s)
    {
        foreach (var i in _list)
        {
            if (i.Name == s)
            {
                i.Busy = false;
                return;
            }
        }
    }

    public static Func<Type, string?> GetVarName { get; set; }

    private readonly List<Info> _list = new();

    private class Info
    {
        public bool   Busy { get; set; }
        public Type   Typ  { get; set; }
        public string Name { get; set; }
    }
}
