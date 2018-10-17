using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Utils;

namespace Instructions
{
    public class InstructionManager : MonoBehaviour
    {
        private Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
        private List<Color> colors = new List<Color>();

        private void Start()
        {
        }

        private void SaveColors()
        {
            // Generate 10 separate colors
            for (int i = 0; i < 10; i++)
            {
                colors.Add(Color.HSVToRGB(i * 0.1f, 0.5f, 1.0f));
            }
        }

        /// <summary>
        /// Returns a unique random color and saves it as the given id
        /// The given id must be unique
        /// </summary>
        public Color GenerateColor(string id)
        {
            if (colors.Count < 1) SaveColors();

            var color = colors[RandomGenerator.GetNext(10)];
            while (colorDict.Values.Contains(color) && colorDict.Count < 10)
            {
                color = colors[RandomGenerator.GetNext(10)];
            }

            colorDict.Add(id, color);
            return color;
        }

        /// <summary>
        /// Unregisters all colors under the given id
        /// </summary>
        public void RemoveColor(string id)
        {
            colorDict.Remove(id);
        }

    }
}