using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class FogController
    {
        Color FogColor = Color.CornflowerBlue;//.ToVector3(); // For best results, ake this color whatever your background is.
        float FogStart = 9.75f;
        float FogEnd = 10.25f;

        public void HandleFog(BasicEffect effect)
        {
            effect.FogEnabled = true;
            effect.FogColor = FogColor.ToVector3();
            effect.FogStart = FogStart;
            effect.FogEnd = FogEnd;
        }
    }
}
