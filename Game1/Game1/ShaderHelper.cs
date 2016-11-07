using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class ShaderHelper
    {
        static Vector3[] directions = { new Vector3(0, -1, 0), new Vector3(0, -1, 0.5f) };
        static Vector3[] colors = { new Vector3(1, 1, 1), new Vector3(1, 1, 1) };
        static Vector3[] positions = { new Vector3(0, -10, 0), new Vector3(10, -10, 10) };
        static Vector3[] PointColors = { new Vector3(1, 1, 1), new Vector3(1, 1, 1) };
        static Vector3[] PointPositions = { new Vector3(0, 10, 80), new Vector3(0, 10, -80) };

        static bool adding = false;

        public static void InitializeShader(Effect effect, Texture2D texture, Camera camera, Matrix? ParentTransform, Matrix? world)
        {
            if (world == null)
                world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            if (ParentTransform != null)
                effect.Parameters["World"].SetValue(world.Value * ParentTransform.Value);
            else
                effect.Parameters["World"].SetValue(world.Value);
            effect.Parameters["View"].SetValue(camera.ViewMatrix);
            effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            effect.Parameters["BasicTexture"].SetValue(texture);

            effect.Parameters["SpotLightDirections"].SetValue(directions);
            effect.Parameters["SpotLightColors"].SetValue(colors);
            effect.Parameters["SpotLightPositions"].SetValue(positions);

            effect.Parameters["PointLightColors"].SetValue(PointColors);
            effect.Parameters["PointLightPositions"].SetValue(PointPositions);
        }

        public static void ChangeColor(float amount)
        {
            if (adding)
            {
                colors[0].X += amount;
                if (colors[0].X > 1)
                {

                    colors[0].X = 1;
                    adding = false;
                }

            }
            else
            {
                colors[0].X -= amount;
                if (colors[0].X < 0)
                {
                    adding = true;
                    colors[0].X = 0;
                }
            }

        }
    }
}
