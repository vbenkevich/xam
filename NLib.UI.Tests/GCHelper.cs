﻿﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NLib.UI.Tests
{
    public class GCHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T CreateCollectable<T>(Func<T> create)
        {
            return create();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static async Task ForceCollect()
        {
            await Task.Yield();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static WeakReference Kill<T>(ref T target) where T : class
        {
            var reference = CreateWeakReference(ref target);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            return reference;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference CreateWeakReference<T>(ref T target) where T : class
        {
            var weakReference = new WeakReference(target);
            target = null;
            return weakReference;
        }
    }
}
