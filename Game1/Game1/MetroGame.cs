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
        SpriteBatch GuiBatch;
        Texture2D checkerboardTexture;
        Texture2D stationTexture;
        Texture2D platformTexture;
        Texture2D robotTexture;
        Texture2D guiTexture;
        Texture2D checkboxCheckedTexture;
        Texture2D checkboxUncheckedTexture;

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


        // Program controls
        bool MultiSampling = true;
        bool PreviouslyPressedU = false;
        bool PreviouslyPressedB = false;
        bool PreviouslyPressedN = false;
        bool PreviouslyPressedSpace = false;
        float MipMapDepthLevels = 0.0f;
        bool MagFilter = false;
        bool MipMapFilter = false;
        bool GUIActive = false;


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
            GuiBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("MonoCube");
            checkerboardTexture = Content.Load<Texture2D>("check");
            stationTexture = Content.Load<Texture2D>("brick2");
            platformTexture = Content.Load<Texture2D>("dark");
            spriteText = Content.Load<SpriteFont>("square");
            Bench = Content.Load<Model>("robot");
            LightEffect = Content.Load<Effect>("lighting");
            robotTexture = Content.Load<Texture2D>("robottexture_0");
            guiTexture = Content.Load<Texture2D>("gui");
            checkboxCheckedTexture = Content.Load<Texture2D>("checked");
            checkboxUncheckedTexture = Content.Load<Texture2D>("unchecked");
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

            if (!GUIActive)
            {
                float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2000.0f;
                camera.ProcessInput(timeDifference, graphics);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.U) && PreviouslyPressedU == false)
            {
                MultiSampling = !MultiSampling;
                PreviouslyPressedU = true;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.U))
                PreviouslyPressedU = false;


            if (!Keyboard.GetState().IsKeyDown(Keys.I))
                MipMapDepthLevels -= 0.05f;

            if (!Keyboard.GetState().IsKeyDown(Keys.L))
                MipMapDepthLevels += 0.05f;


            if (Keyboard.GetState().IsKeyDown(Keys.B) && PreviouslyPressedB == false)
            {
                MagFilter = !MagFilter;
                PreviouslyPressedB = true;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.B))
                PreviouslyPressedB = false;

            if (Keyboard.GetState().IsKeyDown(Keys.N) && PreviouslyPressedN == false)
            {
                MipMapFilter = !MipMapFilter;
                PreviouslyPressedN = true;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.N))
                PreviouslyPressedN = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && PreviouslyPressedSpace == false)
            {
                GUIActive = !GUIActive;
                PreviouslyPressedSpace = true;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.Space))
                PreviouslyPressedSpace = false;

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //GraphicsDevice.SamplerStates[0].MaxMipLevel
            //GraphicsDevice.SamplerStates[1].MipMapLevelOfDetailBias = -16.0f;

            // https://github.com/labnation/MonoGame/blob/master/Tools/2MGFX/SamplerStateInfo.cs

            SamplerState ss = new SamplerState();

            ss.Filter = TextureFilter.Point;
            ss.MaxMipLevel = 0;
            ss.AddressU = TextureAddressMode.Wrap;
            ss.AddressV = TextureAddressMode.Wrap;
            ss.MipMapLevelOfDetailBias = MipMapDepthLevels;
            if (MagFilter && MipMapFilter)
                ss.Filter = TextureFilter.Linear;
            else if (!MagFilter && MipMapFilter)
                ss.Filter = TextureFilter.PointMipLinear;
            else if (MagFilter && !MipMapFilter)
                ss.Filter = TextureFilter.LinearMipPoint;
            else if (!MagFilter && !MipMapFilter)
                ss.Filter = TextureFilter.Point;
            GraphicsDevice.SamplerStates[0] = ss;






            graphics.PreferMultiSampling = MultiSampling;


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
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = LightEffect;
                    ShaderHelper.InitializeShader(LightEffect, robotTexture, camera, mesh.ParentBone.Transform, world3);
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
            spriteBatch.DrawString(spriteText, "camPos:" + camera.CamPosition.ToString() +
                " | AntiAliasing: " + MultiSampling +
                " | MipMapLevelOfDetails: " + MipMapDepthLevels +
                " | FilterType: " + ss.Filter.ToString()


                , new Vector2(10, 10), Color.White);
            spriteBatch.End();
            if (GUIActive) HandleGui(); else IsMouseVisible = false;

            ShaderHelper.ChangeColor(0.01f);
        }

        private void HandleGui()
        {
            Rectangle MagFilterRect = new Rectangle();

            GuiBatch.Begin();
            GuiBatch.Draw(guiTexture, new Vector2(0.0f), Color.White);
            GuiBatch.Draw(checkboxCheckedTexture,  destinationRectangle: new Rectangle(50, 50, 30, 30));
            GuiBatch.End();

            this.IsMouseVisible = true;
        }
    }
}