using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Threading;
using System.Numerics;

namespace animationTry2
{
    public class Physics
    {
        private static Animator _animator;
        private static List<Ball> _balls;
        private static object _obj = new();
        private static bool _wasCollision = false;

        public Physics(Animator animator)
        {
            Physics._animator = animator;
            Physics._balls = _animator.Balls;
        }

      

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public static bool MoveBall(Ball ball)
        {
            var p = new PointF(ball.Origin.X + ball.vX, ball.Origin.Y + ball.vY);

            if (Math.Sign(ball.vX) == 1)
                p.X += ball.Radius * 2;
            if (Math.Sign(ball.vY) == 1)
                p.Y += ball.Radius * 2;

            if (ball.ContainerRect.Contains(Point.Ceiling(p)))
            {
                ball.Origin.X += ball.vX;
                ball.Origin.Y += ball.vY;
            }
            else
            {
                if (p.X <= 0)
                    p.X = 1;
                if (p.Y <= 0)
                    p.Y = 1;
                if (p.X >= ball.ContainerSize.Width)
                    p.X = ball.ContainerSize.Width - 1;
                if (p.Y >= ball.ContainerSize.Height)
                    p.Y = ball.ContainerSize.Height - 1;
                
                if (Math.Sign(ball.vX) == 1)
                    p.X -= ball.Radius * 2;
                if (Math.Sign(ball.vY) == 1)
                    p.Y -= ball.Radius * 2;

                ball.Origin.X = p.X;
                ball.Origin.Y = p.Y;
            }

            BounceBallFromEdge(ball, ball.ContainerSize);
            
            return true;
        }

        private static bool BounceBallFromEdge(Ball ball, Size containerSize)
        {
            if (ball.Origin.X <= 1)
            {
                ball.vX = -ball.vX;
                return true;
            }

            if (ball.Origin.X + ball.Radius * 2 >= containerSize.Width - 1)
            {
                ball.vX = -ball.vX;
                return true;
            }

            if (ball.Origin.Y <= 1)
            {
                ball.vY = -ball.vY;
                return true;
            }

            if (ball.Origin.Y + ball.Radius * 2 >= containerSize.Height - 1)
            {
                ball.vY = -ball.vY;
                return true;
            }

            return false;
        }
        
    }
}