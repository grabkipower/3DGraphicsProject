using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public abstract class Primitive : IPrimitive
    {
        protected VertexPositionTexture[] _vertices;
        protected BasicEffect _basicEffect;
        protected Vector3 Size;
        protected int TriangleNum;
        protected Texture2D texture;



        public void Draw(Camera camera, GraphicsDeviceManager graphics)
        {
            // _basicEffect.AmbientLightColor = new Vector3(1f, 0, 0);
            _basicEffect.View = camera.ViewMatrix;
                _basicEffect.World = camera.WorldMatrix;
            _basicEffect.Projection = camera.ProjectionMatrix;


            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = texture;

            //    EffectTechnique effectTechnique = _basicEffect.Techniques[0];
            //   EffectPassCollection effectPassCollection = effectTechnique.Passes;
            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    _vertices,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    TriangleNum);

            }

        }

        public abstract void Initialize(GraphicsDeviceManager graphics, Vector3 size, Texture2D texture);


    }
}
