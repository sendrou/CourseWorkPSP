using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace GameLibrary
{
    public class ObjectRender
    {
        public static void RenderObjects(int textureID, Vector2[] objectPosition)
        {
            SetupRenderingContext();

            Vector2[] vertices =
            {
                new Vector2(0, 0),  // top-left
                new Vector2(1, 0),  // top-right
                new Vector2(1, 1),  // bottom-right
                new Vector2(0, 1)   // bottom-left
            };

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.Begin(PrimitiveType.Quads);

            for (int i = 0; i < vertices.Length; i++)
            {
                GL.TexCoord2(vertices[i]);
                GL.Vertex2(objectPosition[i]);
            }

            GL.End();
        }

        private static void SetupRenderingContext()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            // Setting up orthogonal projection: left, right, bottom, top, near, far
            GL.Ortho(-1f, 1f, 1f, -1f, 0f, 1f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
    }
}
