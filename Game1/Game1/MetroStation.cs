﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class MetroStation : Primitive
    {

        public override void Initialize(GraphicsDeviceManager graphics, Vector3 _size, Texture2D _texture)
        {
            Size = _size;
            texture = _texture;
            TriangleNum = 12;
            _vertices = new VertexPositionNormalTexture[36];
            Vector3 Position = new Vector3(0, 0, 0);

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = Position + new Vector3(-1.0f, 1.0f, -1.0f) * Size;
            Vector3 topLeftBack = Position + new Vector3(-1.0f, 1.0f, 1.0f) * Size;
            Vector3 topRightFront = Position + new Vector3(1.0f, 1.0f, -1.0f) * Size;
            Vector3 topRightBack = Position + new Vector3(1.0f, 1.0f, 1.0f) * Size;

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = Position + new Vector3(-1.0f, -1.0f, -1.0f) * Size;
            Vector3 btmLeftBack = Position + new Vector3(-1.0f, -1.0f, 1.0f) * Size;
            Vector3 btmRightFront = Position + new Vector3(1.0f, -1.0f, -1.0f) * Size;
            Vector3 btmRightBack = Position + new Vector3(1.0f, -1.0f, 1.0f) * Size;

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f) * Size;
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f) * Size;
            Vector3 normalTop = new Vector3(0.0f, -1.0f, 0.0f) * Size;
            Vector3 normalBottom = new Vector3(0.0f, 1.0f, 0.0f) * Size;
            Vector3 normalLeft = new Vector3(1.0f, 0.0f, 0.0f) * Size;
            Vector3 normalRight = new Vector3(-1.0f, 0.0f, 0.0f) * Size;

            // UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1.0f , 0.0f );
            Vector2 textureTopRight = new Vector2(0.0f , 0.0f );
            Vector2 textureBottomLeft = new Vector2(1.0f , 1.0f);
            Vector2 textureBottomRight = new Vector2(0.0f , 1.0f);

            // Add the vertices for the FRONT face.
            _vertices[0] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            _vertices[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront,  textureBottomLeft);
            _vertices[2] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);
            _vertices[3] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            _vertices[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            _vertices[5] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            // Add the vertices for the BACK face.
            _vertices[6] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            _vertices[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            _vertices[8] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            _vertices[9] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            _vertices[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            _vertices[11] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            // Add the vertices for the TOP face.
            _vertices[12] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            _vertices[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            _vertices[14] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            _vertices[15] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            _vertices[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            _vertices[17] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            // Add the vertices for the BOTTOM face. 
            _vertices[18] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            _vertices[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            _vertices[20] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            _vertices[21] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            _vertices[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            _vertices[23] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            // Add the vertices for the LEFT face.
            _vertices[24] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            _vertices[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            _vertices[26] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);
            _vertices[27] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            _vertices[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            _vertices[29] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            // Add the vertices for the RIGHT face. 
            _vertices[30] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            _vertices[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            _vertices[32] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
            _vertices[33] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            _vertices[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            _vertices[35] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            _vertices = _vertices.Reverse().ToArray();

            _basicEffect = new BasicEffect(graphics.GraphicsDevice);
        }
    }
}

      
        