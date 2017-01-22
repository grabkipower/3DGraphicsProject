﻿using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetroProject
{

    public class MetroGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch renderTargetBatch;
        SpriteBatch GuiBatch;
        Texture2D checkerboardTexture;
        Texture2D stationTexture;
        Texture2D platformTexture;
        Texture2D robotTexture;
        Texture2D guiTexture;
        Texture2D checkboxCheckedTexture;
        Texture2D checkboxUncheckedTexture;
        Texture2D additionalTexture;

        Texture2D EnvironmentMap;
        TextureCube EnvironmentCube;

        RenderTarget2D renderTarget;

        RenderTargetCube RefCubeMap;
        RenderTarget2D renderTarget2;

        // Fog
        FogController fog = new FogController();
        // Gui
        GUIController gui;
        //Camera
        Camera camera;

        //Geometric info
        public const bool DrawTestCube = true;
        Model model;
        List<IPrimitive> primitives;
        Model Bench;
        Screen screen;

        //Sprite
        SpriteFont spriteText;

        // Effects
        Effect LightEffect;
        Effect GausianBlur;
        Effect Custom;
        Effect Glass;


        // Program controls
        public bool MultiSampling = true;
        bool PreviouslyPressedU = false;
        bool PreviouslyPressedB = false;
        bool PreviouslyPressedN = false;
        bool PreviouslyPressedSpace = false;
        float MipMapDepthLevels = 0.0f;
        public bool MagFilter = false;
        public bool MipMapFilter = false;
        bool GUIActive = false;
        public Rozmiar rozdzielczosc = ResolutionProvider.sredni;

        public MetroGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            SetResolution();
            InitializeCamera();
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


            var ball = new BallPrimitive();
            ball.Initialize(graphics, new Vector3(24.0f, 6.0f, 102.0f), platformTexture);
            ball.LightingEffect = Custom;
            primitives.Add(ball);


            screen = new Screen();
            screen.Initialize(graphics, new Vector3(1.0f, 6.0f, 12.0f), renderTarget);
            //      screen.LightingEffect = GausianBlur;
            //     primitives.Add(screen);



            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);


            renderTarget2 = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            //RefCubeMap = new RenderTargetCube(
            //GraphicsDevice,
            //256, //this can be changed
            //true,
            //GraphicsDevice.DisplayMode.Format,
            //DepthFormat.Depth16); //this can be changed
            //TextureCube scene = new TextureCube(
            //GraphicsDevice,
            //256, //use whatever you like
            //true,
            //GraphicsDevice.DisplayMode.Format);

            RefCubeMap = new RenderTargetCube(this.GraphicsDevice, 256, true, SurfaceFormat.Color, DepthFormat.Depth16, 1, RenderTargetUsage.PreserveContents);

                //     GraphicsDevice.DisplayMode.Format); new RenderTargetCube(this.GraphicsDevice, 256, 1, SurfaceFormat.Color);

            }

        public void InitializeCamera()
        {
            //Setup Camera
            camera = new Camera();
            Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            camera.Initialize(graphics, Mouse.GetState());
        }

        public void SetResolution()
        {
            graphics.PreferredBackBufferWidth = rozdzielczosc.x;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = rozdzielczosc.y;   // set this value to the desired height of your window
            if (rozdzielczosc == ResolutionProvider.duzy)
                graphics.IsFullScreen = true;
            else
                graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            InitializeCamera();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderTargetBatch = new SpriteBatch(GraphicsDevice);
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
            additionalTexture = Content.Load<Texture2D>("metro2");
            GausianBlur = Content.Load<Effect>("gaussianblur");
            Custom = Content.Load<Effect>("custom");
            Glass = Content.Load<Effect>("GlassEffect");
            gui = new GUIController(GuiBatch, guiTexture, checkboxCheckedTexture, checkboxUncheckedTexture, this, spriteText);
            //   stationTexture = CreateStaticMap(1000);
        }

        private Texture2D CreateStaticMap(int resolution)
        {
            Random rand = new Random();
            Color[] noisyColors = new Color[resolution * resolution];
            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    noisyColors[x + y * resolution] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, (float)rand.Next(1000) / 1000.0f, (float)rand.Next(1000) / 1000.0f));

            Texture2D noiseImage = new Texture2D(GraphicsDevice, resolution, resolution);//, 1, TextureUsage.None, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
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
            else
            {
                gui.HandleGUIInput();
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

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, GameTime gameTime)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);



            // Draw the scene
            //   GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawScene(gameTime);

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetRenderTarget(renderTarget2);
            //       var ss = SetSamplerState();
            //         graphics.PreferMultiSampling = MultiSampling;
            foreach (var item in primitives)
            {
                item.Draw(camera, graphics);
            }
             //  DrawCube();

            DoTheCubeMapThingy();
            this.EnvironmentMap = renderTarget2;
            //this.EnvironmentCube = RefCubeMap;
            Glass.Parameters["ReflectionCubeMap"].SetValue(EnvironmentMap);

            GraphicsDevice.SetRenderTarget(null);
            foreach (var item in primitives)
            {
                item.Draw(camera, graphics);
            }
         //   DrawCube();
            DrawGlassRobot(camera, new Vector3(3, 3, 3));

            //GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            //DrawSceneToTexture(renderTarget, gameTime);

            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //renderTargetBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
            //     SamplerState.LinearClamp, DepthStencilState.Default,
            //     RasterizerState.CullNone, GausianBlur);
            //renderTargetBatch.Draw(renderTarget, destinationRectangle: new Rectangle(0, 0,rozdzielczosc.x, rozdzielczosc.y), color: Color.White);
            //renderTargetBatch.End();
            //var tmp = new Texture2D(GraphicsDevice, rozdzielczosc.x, rozdzielczosc.y);
            //DrawScene(gameTime);
            //screen.Draw(camera, graphics, tmp);


            //base.Draw(gameTime);
            //ShaderHelper.ChangeColor(0.01f);
        }

        private void DrawScene(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var ss = SetSamplerState();
            graphics.PreferMultiSampling = MultiSampling;
            foreach (var item in primitives)
            {
                item.Draw(camera, graphics);
            }
            DrawCube();
            DrawShipsShifted();

            //    base.Draw(gameTime);

            DrawInfoText(ss);
            if (GUIActive) gui.DrawGUI(); else IsMouseVisible = false;
        }

        private SamplerState SetSamplerState()
        {
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
            return ss;
        }

        private void DoTheCubeMapThingy()
        {
            Matrix viewMatrix;

            CubeMapFace cubeMapFace = CubeMapFace.NegativeX;



            Matrix world = Matrix.CreateTranslation(new Vector3(3, 3, 3));
            Glass.Parameters["World"].SetValue(world);

            Glass.Parameters["Projection"].SetValue(camera.ProjectionMatrix);

            for (int i = 0; i < 6; i++)
            {
                // render the scene to all cubemap faces
                cubeMapFace = (CubeMapFace)i;
                // Glass.Parameters["View"].SetValue(Camera.ViewMatrix);
                switch (cubeMapFace)
                {
                    case CubeMapFace.NegativeX:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Left, Vector3.Up);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                    case CubeMapFace.NegativeY:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Down, Vector3.Forward);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                    case CubeMapFace.NegativeZ:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Backward, Vector3.Up);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                    case CubeMapFace.PositiveX:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Right, Vector3.Up);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                    case CubeMapFace.PositiveY:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Up, Vector3.Backward);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                    case CubeMapFace.PositiveZ:
                        {
                            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
                            this.GraphicsDevice.SetRenderTarget(RefCubeMap, cubeMapFace);
                            this.GraphicsDevice.Clear(Color.White);
                            Glass.Parameters["View"].SetValue(viewMatrix);
                            GraphicsDevice.SetRenderTarget(null);
                            break;
                        }
                }
            }

            // Set the cubemap render target, using the selected face

        }
        public void DrawGlassRobot(Camera camera, Vector3 position)
        {
            foreach (var mesh in Bench.Meshes)
            {

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = Glass;

                    Matrix world = Matrix.CreateTranslation(position);
                    Glass.Parameters["World"].SetValue(mesh.ParentBone.Transform * world);
                    Glass.Parameters["View"].SetValue(camera.ViewMatrix);
                    Glass.Parameters["Projection"].SetValue(camera.ProjectionMatrix);

                }
                mesh.Draw();


            }
        }
        private void DrawCube()
        {
             


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
                        fog.HandleFog(effect);
                    }
                    mesh.Draw();
                }
            }
        }
        private void DrawShipsShifted()
        {
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
                    // HandleFog(LightEffect);
                }
                mesh.Draw();
            }
        }
        private void DrawInfoText(SamplerState ss)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(spriteText, "camPos:" + camera.CamPosition.ToString() +
                " | AntiAliasing: " + MultiSampling +
                " | MipMapLevelOfDetails: " + MipMapDepthLevels +
                " | FilterType: " + ss.Filter.ToString()


                , new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }

    }
}