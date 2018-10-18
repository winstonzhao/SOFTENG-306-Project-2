using UnityEngine;

namespace Utils
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Search the given transform using DFS for the given name.
        ///
        /// By default Transform.Find only searches direct descendants
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform DeepFind(this Transform transform, string name)
        {
            return ParentDeepFind(transform, name);
        }

        private static Transform ParentDeepFind(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    return child;
                }

                var search = ParentDeepFind(child, name);

                if (search != null)
                {
                    return search;
                }
            }

            return null;
        }
    }
}
