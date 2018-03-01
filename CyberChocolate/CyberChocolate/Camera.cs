using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CyberChocolate
{
    public class Camera
    {
        protected float _zoom;
        protected Matrix _transform;
        protected Matrix _inverseTransform;
        protected Vector2 _pos;
        protected float _rotation;
        protected Viewport _viewport;
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }
        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        public Matrix InverseTransform
        {
            get { return _inverseTransform; }
        }
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public static Camera GetInstance(Viewport viewport)
        {
            if (instance == null)
            {
                instance = new Camera(viewport);
            }
            return instance;
        }
        private static Camera instance;

        private Camera(Viewport viewport) {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewport = viewport;
        }

        public void Update(Vector2 charpos)
        {
            _zoom = MathHelper.Clamp(_zoom, 0.0f, 10.0f);
            _rotation = ClampAngle(_rotation);
            _transform =
                            Matrix.CreateTranslation(-charpos.X + (_viewport.Width / 2), -charpos.Y + (_viewport.Height / 2), 0) *
                            Matrix.CreateRotationZ(_rotation) *
                            Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));
        }      
        protected float ClampAngle(float radians)
        {
            while (radians< -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }


    }
}
