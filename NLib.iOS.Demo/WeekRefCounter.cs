using System;
using System.Linq;
using System.Collections.Generic;

namespace NLib.iOS.Demo
{
    public static class WeekRefCounter
    {
        private static List<WeakReference> references;

        static WeekRefCounter()
        {
            references = new List<WeakReference>();
        }

        public static int Count => references.Count();

        public static void Add(object obj)
        {
            references.Add(new WeakReference(obj));
        }

        public static void Clear()
        {
            GC.Collect();

            references = new List<WeakReference>(references.Where(r => r.IsAlive));
        }
    }
}
