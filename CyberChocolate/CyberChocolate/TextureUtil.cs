using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CyberChocolate
{
   public static class TextureUtil
    {
        static Texture2D _pointTexture;
        public static Texture2D CreateCircle(GraphicsDevice graphics, int radius, Color color)
        {
            int diameter = radius * 2;
            Vector2 center = new Vector2(radius, radius);

            Texture2D circle = new Texture2D(graphics, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];

            for (int i = 0; i < colors.Length; i++)
            {
                int x = (i + 1) % diameter;
                int y = (i + 1) / diameter;
                Vector2 distance = new Vector2(Math.Abs(center.X - x), Math.Abs(center.Y - y));
                colors[i] = distance.Length() > radius ? Color.Transparent : color;
            }

            circle.SetData<Color>(colors);
            return circle;
        }

        public static Texture2D CreateRectangle(GraphicsDevice graphics, int width, int height, Color color)
        {
            Texture2D rect = new Texture2D(graphics, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            return rect;
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }
}
