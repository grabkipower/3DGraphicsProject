using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public interface IPrimitive
    {
        void Initialize(GraphicsDeviceManager graphics,  Vector3 size, Texture2D texture);
        void Draw(Camera camera, GraphicsDeviceManager graphics, Texture2D DynamicTexture = null);
    }
}
