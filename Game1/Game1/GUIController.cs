using MetroProject;
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
    public class GUIController
    {
        SpriteBatch GuiBatch;
        Texture2D guiTexture;
        Texture2D checkboxCheckedTexture;
        Texture2D checkboxUncheckedTexture;
        MetroGame MetroGame;
        SpriteFont spriteFont;

        private const int RectSize = 30;
        private const int NewLineSize = 40;
        private const int MarginTop = 20;
        private const int TextMarginLeft = 55;
        Rectangle MagFilterRect = new Rectangle(20, MarginTop + NewLineSize, RectSize, RectSize);
        Rectangle MipMapFilterRect = new Rectangle(20, MarginTop + (2 * NewLineSize), RectSize, RectSize);
        Rectangle AntiAliasingRect = new Rectangle(20, MarginTop + (3 * NewLineSize), RectSize, RectSize);
        Rectangle ListBoxInitialRect = new Rectangle(20, MarginTop + (4 * NewLineSize), 5 * RectSize, NewLineSize);
        Rectangle ListBoxChoice1 = new Rectangle(20, MarginTop + (5 * NewLineSize), 5 * RectSize, NewLineSize);
        Rectangle ListBoxChoice2 = new Rectangle(20, MarginTop + (6 * NewLineSize), 5 * RectSize, NewLineSize);
        Rectangle ListBoxChoice3 = new Rectangle(20, MarginTop + (7 * NewLineSize), 5 * RectSize, NewLineSize);

        bool MousePressedBefore = false;
        bool ListBoxVisible = false;
        bool HoverOverListBoxTitle = false;
        bool HoverOverChoice1 = false;
        bool HoverOverChoice2 = false;
        bool HoverOverChoice3 = false;

        public GUIController(SpriteBatch _GuiBatch, Texture2D texture, Texture2D checkboxChecked, Texture2D checkboxUnchecked, MetroGame gamePtr, SpriteFont font)
        {
            GuiBatch = _GuiBatch;
            guiTexture = texture;
            checkboxCheckedTexture = checkboxChecked;
            checkboxUncheckedTexture = checkboxUnchecked;
            MetroGame = gamePtr;
            spriteFont = font;
        }

        public void HandleGUIInput()
        {
            var a = Mouse.GetState();
            if (ListBoxInitialRect.Contains(a.Position))
            {
                HoverOverListBoxTitle = true;
                HoverOverChoice1 = HoverOverChoice2 = HoverOverChoice3 = false;
            }
            else
            {
                if (HoverOverListBoxTitle == true)
                {
                    HoverOverChoice1 = HoverOverChoice2 = HoverOverChoice3 = false;
                    if (ListBoxChoice1.Contains(a.Position))
                        HoverOverChoice1 = true;
                    else if (ListBoxChoice2.Contains(a.Position))
                        HoverOverChoice2 = true;
                    else if (ListBoxChoice3.Contains(a.Position))
                        HoverOverChoice3 = true;
                    else
                        HoverOverListBoxTitle = false;
                }
            }
            if (a.LeftButton == ButtonState.Pressed && !MousePressedBefore)
            {
                MousePressedBefore = true;
                if (MagFilterRect.Contains(a.Position))
                {
                    MetroGame.MagFilter = !MetroGame.MagFilter;
                    return;
                }
                if (MipMapFilterRect.Contains(a.Position))
                {
                    MetroGame.MipMapFilter = !MetroGame.MipMapFilter;
                    return;
                }
                if (AntiAliasingRect.Contains(a.Position))
                {
                    MetroGame.MultiSampling = !MetroGame.MultiSampling;
                    return;
                }

                if (ListBoxChoice1.Contains(a.Position))
                {
                    MetroGame.rozdzielczosc = ResolutionProvider.duzy;
                    MetroGame.SetResolution();
                    HoverOverListBoxTitle = HoverOverChoice1 = HoverOverChoice2 = HoverOverChoice3 = false;
                    return;
                }
                if (ListBoxChoice2.Contains(a.Position))
                {
                    MetroGame.rozdzielczosc = ResolutionProvider.sredni;
                    MetroGame.SetResolution();
                    HoverOverListBoxTitle = HoverOverChoice1 = HoverOverChoice2 = HoverOverChoice3 = false;
                    return;
                }
                if (ListBoxChoice3.Contains(a.Position))
                {
                    MetroGame.rozdzielczosc = ResolutionProvider.maly;
                    MetroGame.SetResolution();
                    HoverOverListBoxTitle = HoverOverChoice1 = HoverOverChoice2 = HoverOverChoice3 = false;
                    return;
                }


            }
            if (a.LeftButton == ButtonState.Released)
                MousePressedBefore = false;

        }

        public void DrawGUI()
        {

            GuiBatch.Begin();
            GuiBatch.Draw(guiTexture, new Vector2(0.0f), Color.White);

            GuiBatch.Draw(GetTexture(MetroGame.MagFilter), destinationRectangle: MagFilterRect);
            GuiBatch.DrawString(spriteFont, " Mag filter", new Vector2(TextMarginLeft, 65), Color.White);

            GuiBatch.Draw(GetTexture(MetroGame.MipMapFilter), destinationRectangle: MipMapFilterRect);
            GuiBatch.DrawString(spriteFont, " MipMap filter", new Vector2(TextMarginLeft, 65 + NewLineSize), Color.White);

            GuiBatch.Draw(GetTexture(MetroGame.MultiSampling), destinationRectangle: AntiAliasingRect);
            GuiBatch.DrawString(spriteFont, " Multi Sampling", new Vector2(TextMarginLeft, 65 + 2 * NewLineSize), Color.White);

            GuiBatch.DrawString(spriteFont, "Choose resolution", new Vector2(TextMarginLeft, 65 + 3 * NewLineSize), GetColor(HoverOverListBoxTitle));
            if (HoverOverListBoxTitle)
            {
                GuiBatch.DrawString(spriteFont, ResolutionProvider.duzy.ToString(), new Vector2(TextMarginLeft, 65 + 4 * NewLineSize), GetColor(HoverOverChoice1));
                GuiBatch.DrawString(spriteFont, ResolutionProvider.sredni.ToString(), new Vector2(TextMarginLeft, 65 + 5 * NewLineSize), GetColor(HoverOverChoice2));
                GuiBatch.DrawString(spriteFont, ResolutionProvider.maly.ToString(), new Vector2(TextMarginLeft, 65 + 6 * NewLineSize), GetColor(HoverOverChoice3));

            }

            GuiBatch.End();
                        
            MetroGame.IsMouseVisible = true;
        }

        private Color GetColor(bool value)
        {
            if (!value)
                return Color.White;
            return Color.DarkGreen;

        }

        private Texture2D GetTexture(bool value)
        {
            if (value)
                return checkboxCheckedTexture;
            return checkboxUncheckedTexture;
        }
    }
}
