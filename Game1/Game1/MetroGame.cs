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

        //Camera
        Camera camera;

        //Geometric info
        public const bool DrawTestCube = false;
        Model model;
        List<IPrimitive> primitives;
        Model Bench;

        //Sprite
        SpriteFont spriteText;

        
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
            primitives.Add(station);
            var platform = new Platform();
            platform.Initialize(graphics, new Vector3(24.0f, 4.0f, 102.0f), platformTexture);
            primitives.Add(platform);
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("MonoCube");
            checkerboardTexture = Content.Load<Texture2D>("check");
            stationTexture = Content.Load<Texture2D>("metro2");
            platformTexture = Content.Load<Texture2D>("dark");
            spriteText = Content.Load<SpriteFont>("square");
       //    Bench = Content.Load<Model>("LargeAsteroid");
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

            if (DrawTestCube)
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        //effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.View = camera.ViewMatrix;
                        effect.World = camera.WorldMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }



            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = camera.WorldMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                mesh.Draw();
            }


            foreach (var item in primitives)
            {
                item.Draw(camera, graphics);
            }

            base.Draw(gameTime);


            spriteBatch.Begin();
            spriteBatch.DrawString(spriteText, "camPos:" + camera.CamPosition.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }
}