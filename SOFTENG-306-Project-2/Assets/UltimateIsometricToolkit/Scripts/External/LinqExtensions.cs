﻿using System;
using System.Collections.Generic;

namespace Ultimate_Isometric_Toolkit.Scripts.External
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
