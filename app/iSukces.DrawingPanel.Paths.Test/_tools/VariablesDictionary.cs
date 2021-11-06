using System;
using System.Collections.Generic;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class VariablesDictionary
    {
        public string GetName(Type t, out bool first)
        {
            foreach (var i in _list)
            {
                if (i.Busy)
                    continue;
                if (i.Typ != t) continue;
                i.Busy = true;
                first  = false;
                return i.Name;
            }

            var nr = _list.Count + 1;
            var info = new Info
            {
                Busy = true,
                Name = "tmp" + nr.ToCs(),
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

        private readonly List<Info> _list = new();

        private class Info
        {
            public bool   Busy { get; set; }
            public Type   Typ  { get; set; }
            public string Name { get; set; }
        }
    }
}
