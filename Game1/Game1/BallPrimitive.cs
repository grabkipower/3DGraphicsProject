﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class BallPrimitive : Primitive
    {
        VertexPosition[] VerticesPosition;
      //  BasicEffect effect;
        public override void Initialize(GraphicsDeviceManager graphics, Vector3 _size, Texture2D _texture)
        {
            Sphere(3.0f, graphics.GraphicsDevice);
            //Size = _size;
            //texture = _texture;
            //TriangleNum = 12;
            //_vertices = new VertexPositionNormalTexture[36];
            //Vector3 Position = new Vector3(0, 0, 0);


            //VerticesPosition = new VertexPosition[]
            //{
            //    new VertexPosition(new Vector3(0.6045f, -0.9780f, 0.0000f)),
            //    new VertexPosition(new Vector3(0.9780f, -0.0000f, -0.6045f)),
            //    new VertexPosition(new Vector3(-0.0000f, -0.6045f, -0.9780f)),
            //    new VertexPosition(new Vector3(0.0000f, 0.6045f, -0.9780f)),
            //    new VertexPosition(new Vector3(-0.9780f, 0.0000f, -0.6045f)),
            //    new VertexPosition(new Vector3(-0.6045f, 0.9780f, -0.0000f)),
            //    new VertexPosition(new Vector3(-0.9780f, 0.0000f, 0.6045f)),
            //    new VertexPosition(new Vector3(-0.6045f, -0.9780f, -0.0000f)),
            //    new VertexPosition(new Vector3(0.6045f, 0.9780f, 0.0000f)),
            //    new VertexPosition(new Vector3(0.3568f, -0.0000f, 0.9342f)),
            //    new VertexPosition(new Vector3(0.5774f, 0.5774f, 0.5774f)),
            //    new VertexPosition(new Vector3(0.0000f, 0.9342f, 0.3568f)),
            //    new VertexPosition(new Vector3(-0.5774f, 0.5774f, 0.5774f)),
            //    new VertexPosition(new Vector3(-0.3568f, 0.0000f, 0.9342f)),
            //    new VertexPosition(new Vector3(-0.5774f, -0.5774f, 0.5774f)),
            //    new VertexPosition(new Vector3(-0.0000f, -0.9342f, 0.3568f)),
            //    new VertexPosition(new Vector3(0.5774f, -0.5774f, 0.5774f)),
            //    new VertexPosition(new Vector3(0.9342f, 0.3568f, 0.0000f)),
            //    new VertexPosition(new Vector3(0.9342f, -0.3568f, 0.0000f)),
            //    new VertexPosition(new Vector3(-0.0000f, -0.9342f, -0.3568f)),
            //    new VertexPosition(new Vector3(0.5774f, -0.5774f, -0.5774f)),
            //    new VertexPosition(new Vector3(0.5774f, 0.5774f, -0.5774f)),
            //    new VertexPosition(new Vector3(0.3568f, -0.0000f, -0.9342f)),
            //    new VertexPosition(new Vector3(-0.5774f, -0.5774f, -0.5774f)),
            //    new VertexPosition(new Vector3(-0.3568f, 0.0000f, -0.9342f)),
            //    new VertexPosition(new Vector3(0.0000f, 0.9342f, -0.3568f)),
            //    new VertexPosition(new Vector3(-0.5774f, 0.5774f, -0.5774f)),
            //    new VertexPosition(new Vector3(-0.9342f, -0.3568f, -0.0000f)),
            //    new VertexPosition(new Vector3(-0.9342f, 0.3568f, -0.0000f)),

            //};

            //effect = new BasicEffect(graphics.GraphicsDevice);

        }


        //public override void Draw(Camera camera, GraphicsDeviceManager graphics, Texture2D DynamicTexture = null)
        //{
        //    // The assignment of effect.View and effect.Projection
        //    // are nearly identical to the code in the Model drawing code.
        //    var cameraPosition = camera.CamPosition;
        //    var cameraLookAtVector = camera.CamTarget;
        //    var cameraUpVector = Vector3.UnitZ;

        //    effect.View = Matrix.CreateLookAt(
        //        cameraPosition, cameraLookAtVector, cameraUpVector);

        //    float aspectRatio =
        //        graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
        //    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
        //    float nearClipPlane = 1;
        //    float farClipPlane = 200;

        //    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
        //        fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

        //    // new code:
        //    effect.TextureEnabled = false;
        //    effect.Texture = DynamicTexture;

        //    foreach (var pass in effect.CurrentTechnique.Passes)
        //    {
        //        pass.Apply();

        //        graphics.GraphicsDevice.DrawUserPrimitives(
        //                    PrimitiveType.TriangleList,
        //            VerticesPosition,
        //            0,
        //            9);
        //    }
        //    return;



        //}


        VertexPositionColor[] vertices; //later, I will provide another example with VertexPositionNormalTexture
        VertexBuffer vbuffer;
        short[] indices; //my laptop can only afford Reach, no HiDef :(
        IndexBuffer ibuffer;
        float radius;
        int nvertices, nindices;
        BasicEffect effect;
        GraphicsDevice graphicd;
        public void Sphere(float Radius, GraphicsDevice graphics)
        {
            radius = Radius;
            graphicd = graphics;
            effect = new BasicEffect(graphicd);
            nvertices = 90 * 90; // 90 vertices in a circle, 90 circles in a sphere
            nindices = 90 * 90 * 6;
            vbuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);
            createspherevertices();
            createindices();
            vbuffer.SetData<VertexPositionColor>(vertices);
            ibuffer.SetData<short>(indices);
            effect.VertexColorEnabled = true;
        }
        void createspherevertices()
        {
            vertices = new VertexPositionColor[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            for (int x = 0; x < 90; x++) //90 circles, difference between each is 4 degrees
            {
                float difx = 360.0f / 90.0f;
                for (int y = 0; y < 90; y++) //90 veritces, difference between each is 4 degrees 
                {
                    float dify = 360.0f / 90.0f;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); //rotate vertex around z
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx));// rotate circle around y
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation

                    vertices[x + y * 90] = new VertexPositionColor(point, Color.White);
                }
            }
        }
        void createindices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < 90; x++)
            {
                for (int y = 0; y < 90; y++)
                {
                    int s1 = x == 89 ? 0 : x + 1;
                    int s2 = y == 89 ? 0 : y + 1;
                    short upperLeft = (short)(x * 90 + y);
                    short upperRight = (short)(s1 * 90 + y);
                    short lowerLeft = (short)(x * 90 + s2);
                    short lowerRight = (short)(s1 * 90 + s2);
                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight;
                }
            }
        }
        public override void Draw(Camera camera, GraphicsDeviceManager graphics, Texture2D DynamicTexture = null)
        {
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
  //          graphicd.RasterizerState = new RasterizerState() { FillMode = FillMode.Solid }; // Wireframe as in the picture
                                                                                            //     ShaderHelper.InitializeShader(LightingEffect, texture, camera, null, null);
            LightingEffect.Parameters["WorldViewProjection"].SetValue(camera.WorldMatrix * camera.ViewMatrix * camera.ProjectionMatrix);
            foreach (EffectPass pass in LightingEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicd.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3);
            }
        }
    }
}
