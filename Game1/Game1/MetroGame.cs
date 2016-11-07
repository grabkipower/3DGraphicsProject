using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace MetroProject
{

    public class MetroGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D checkerboardTexture;
        Texture2D stationTexture;
        Texture2D platformTexture;
        Texture2D robotTexture;

        //Camera
        Camera camera;

        //Geometric info
        public const bool DrawTestCube = true;
        Model model;
        List<IPrimitive> primitives;
        Model Bench;

        //Sprite
        SpriteFont spriteText;

        // Effects
        Effect LightEffect;


        public MetroGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Setup Camera
            camera = new Camera();
            Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            camera.Initialize(graphics, Mouse.GetState());
            //UpdateViewMatrix();

            //Setup Primitives
            primitives = new List<IPrimitive>();
            var station = new MetroStation();
            station.Initialize(graphics, new Vector3(36.0f, 16.0f, 102.0f), stationTexture);
            station.LightingEffect = LightEffect;
            primitives.Add(station);
            var platform = new Platform();
            platform.Initialize(graphics, new Vector3(24.0f, 6.0f, 102.0f), platformTexture);
            platform.LightingEffect = LightEffect;
            primitives.Add(platform);

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("MonoCube");
            checkerboardTexture = Content.Load<Texture2D>("check");
            stationTexture = Content.Load<Texture2D>("brick2");
            platformTexture = Content.Load<Texture2D>("dark");
            spriteText = Content.Load<SpriteFont>("square");
            Bench = Content.Load<Model>("robot");
            LightEffect = Content.Load<Effect>("lighting");
            robotTexture = Content.Load<Texture2D>("robottexture_0");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2000.0f;
            camera.ProcessInput(timeDifference, graphics);

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            foreach (var item in primitives)
            {
                item.Draw(camera, graphics);
            }
            if (DrawTestCube)
            {

                Matrix world = Matrix.CreateTranslation(new Vector3(3, 3, 3));
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        //effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.View = camera.ViewMatrix;
                        effect.World = world;
                        effect.Projection = camera.ProjectionMatrix;
                        //effect.Texture = stationTexture;
                    }
                    mesh.Draw();
                }
            }

            //Matrix world2 = Matrix.CreateTranslation(new Vector3(1, 2, 1));
            //foreach (ModelMesh mesh in Bench.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world2;
            //        effect.View = camera.ViewMatrix;
            //        effect.Projection = camera.ProjectionMatrix;
            //    }

            //    mesh.Draw();
            //}

            Matrix world3 = Matrix.CreateTranslation(new Vector3(8.0f, -10.0f, 1.0f));
            foreach (ModelMesh mesh in Bench.Meshes)
            {
                foreach( ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = LightEffect;
                    ShaderHelper.InitializeShader(LightEffect, robotTexture, camera, mesh.ParentBone.Transform,world3);                    
                }
                mesh.Draw();
            }
            Matrix world5 = Matrix.CreateTranslation(new Vector3(18.0f, 10.0f, 5.0f));
            foreach (ModelMesh mesh in Bench.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = LightEffect;
                    ShaderHelper.InitializeShader(LightEffect, robotTexture, camera, mesh.ParentBone.Transform, world5);
                }
                mesh.Draw();
            }


            base.Draw(gameTime);


            spriteBatch.Begin();
            spriteBatch.DrawString(spriteText, "camPos:" + camera.CamPosition.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.End();

            
            ShaderHelper.ChangeColor(0.01f);
        }
    }
}