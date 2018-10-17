using UnityEngine;

namespace Utils
{
    public static class UnityExtensions
    {
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
