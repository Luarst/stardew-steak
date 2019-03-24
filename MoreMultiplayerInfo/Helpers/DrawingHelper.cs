using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace MoreMultiplayerInfo
{
    public class DrawingHelper
    {

        public static Texture2D WhitePixel;

        static DrawingHelper()
        {
            WhitePixel = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            WhitePixel.SetData(new[] { Color.White });
        }

        public static int GetWidthInPlayArea()
        {
            int result = 0;

            if (Game1.isOutdoorMapSmallerThanViewport())
            {
                int right = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right;
                int totalWidth = Game1.currentLocation.map.Layers[0].LayerWidth * Game1.tileSize;
                int blackSpace = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right - totalWidth;
                result = right - blackSpace / 2;
            }
            else
            {
                result = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right;
            }

            return result;
        }

        public static void DrawBorder(SpriteBatch b, Rectangle dims, int borderWidth, Color color)
        {
            b.Draw(WhitePixel, new Rectangle(dims.Left, dims.Top, borderWidth, dims.Height), color); /* Left */
            b.Draw(WhitePixel, new Rectangle(dims.Left, dims.Top, dims.Width, borderWidth), color); /* Top */
            b.Draw(WhitePixel, new Rectangle(dims.Right, dims.Top, borderWidth, dims.Height), color); /* Right  */
            b.Draw(WhitePixel, new Rectangle(dims.Left, dims.Bottom, dims.Width, borderWidth), color); /* Bottom */
        }
    }
}