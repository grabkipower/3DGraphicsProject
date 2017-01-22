using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Camera
    {

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
       
        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;
        const float rotationSpeed = 0.3f;
        const float moveSpeed = 30.0f;
        MouseState originalMouseState;

        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }

            set
            {
                projectionMatrix = value;
            }
        }
        public Matrix ViewMatrix
        {
            get
            {
                return viewMatrix;
            }

            set
            {
                viewMatrix = value;
            }
        }
        public Matrix WorldMatrix
        {
            get
            {
                return worldMatrix;
            }

            set
            {
                worldMatrix = value;
            }
        }
        public Vector3 CamTarget
        {
            get
            {
                return camTarget;
            }

            set
            {
                camTarget = value;
            }
        }
        public Vector3 CamPosition
        {
            get
            {
                return camPosition;
            }

            set
            {
                camPosition = value;
            }
        }

        public void Initialize(GraphicsDeviceManager graphics, MouseState _originalMouseState)
        {
            originalMouseState = _originalMouseState;

            CamTarget = new Vector3(0f, 0f, 0f);
            CamPosition = new Vector3(0f, 0f, -5);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                   MathHelper.ToRadians(45f), graphics.
                                   GraphicsDevice.Viewport.AspectRatio,
                    1f, 1000f);
            ViewMatrix = Matrix.CreateLookAt(CamPosition, CamTarget,
                         new Vector3(1f, 1f, 0f));// Y up
            WorldMatrix = Matrix.CreateWorld(CamTarget, Vector3.
                          Forward, Vector3.Up);
        }

        public void ProcessInput(float amount, GraphicsDeviceManager graphics)
        {
            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                leftrightRot -= rotationSpeed * xDifference * amount;
                updownRot -= rotationSpeed * yDifference * amount;
                Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
                UpdateViewMatrix();
            }

            Vector3 moveVector = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            AddToCameraPosition(moveVector * amount);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            camPosition += moveSpeed * rotatedVector;
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = camPosition + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            ViewMatrix = Matrix.CreateLookAt(camPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

    }
}
