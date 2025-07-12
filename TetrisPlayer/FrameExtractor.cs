using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
   
    public static class FrameExtractor
    {
        public static bool[,] LoadFramesFromFolder(string folderPath, int frameCount, float threshold = 0.5f)
        {
            string path = $"{folderPath}{frameCount}.jpg";
            if (!File.Exists(path))
            {
                Debug.Log($"File {path} does not excist");
                return null;
            }
            // Make 10x smaller than original // 480x360
            Texture2D texture = LoadAndResizeImage(path, 40, 30);

            return ConvertToBoolFrame(texture, threshold);
        }

        private static Texture2D LoadAndResizeImage(string path, int targetWidth, int targetHeight)
        {
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D original = new Texture2D(2, 2);
            original.LoadImage(imageData);

            RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0);
            RenderTexture.active = rt;
            Graphics.Blit(original, rt);

            Texture2D resized = new Texture2D(targetWidth, targetHeight);
            resized.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resized.Apply();

            RenderTexture.active = null;
            rt.Release();

            return resized;
        }

        private static bool[,] ConvertToBoolFrame(Texture2D texture, float threshold)
        {
            int width = texture.width;
            int height = texture.height;
            bool[,] frame = new bool[width, height];

            Color[] pixels = texture.GetPixels();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = pixels[y * width + x];
                    float luminance = color.grayscale;
                    frame[x, y] = luminance < threshold;
                }
            }

            return frame;
        }
    }

}
