using System.IO;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
   
    public static class FrameExtractor
    {
        public static bool[,] LoadFrameFromFolder(string folderPath, int frameCount, float threshold = 0.5f)
        {
            string path = $"{folderPath}{frameCount}.jpg";
            if (!File.Exists(path))
            {
                Debug.Log($"File {path} does not exist");
                return null;
            }

            Texture2D texture = LoadAndResizeImage(path, 120, 90);
            bool[,] result = ConvertToBoolFrame(texture, threshold);

            // Cleanup to prevent out of memory...
            Object.Destroy(texture);

            return result;
        }

        private static Texture2D LoadAndResizeImage(string path, int targetWidth, int targetHeight)
        {
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D original = new Texture2D(2, 2);
            original.LoadImage(imageData);

            RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 0);
            RenderTexture.active = rt;
            Graphics.Blit(original, rt);

            Texture2D resized = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
            resized.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resized.Apply();

            // Cleanup
            RenderTexture.active = null;
            rt.Release();
            Object.Destroy(rt);
            Object.Destroy(original);

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
