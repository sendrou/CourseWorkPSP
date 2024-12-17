using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace GameLibrary
{
    public class CreateTexture
    {
        public static int LoadTexture(string texturePath)
        {
            string fullPath = Path.Combine("Sprites", texturePath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found at '{fullPath}'");
            }

            int textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            using (Bitmap bmp = new Bitmap(fullPath))
            {
                BitmapData bmpData = bmp.LockBits(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format32bppArgb
                );

                GL.TexImage2D(
                    TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    bmpData.Width, bmpData.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte,
                    bmpData.Scan0
                );

                bmp.UnlockBits(bmpData);
            }

            SetTextureParameters();

            return textureID;
        }

        private static void SetTextureParameters()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
    }
}
