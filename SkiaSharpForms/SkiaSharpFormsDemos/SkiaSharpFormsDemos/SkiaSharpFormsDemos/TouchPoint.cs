﻿using System;
using SkiaSharp;
using TouchTracking;

namespace SkiaSharpFormsDemos
{
    class TouchPoint
    {
        // For painting
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Fill
        };

        // For dragging
        bool isBeingDragged;
        long touchId;
        SKPoint previousPoint;

        public SKPoint Center { set; get; }

        public float Radius { set; get; }

        public SKColor Color { set; get; }

        public void Paint(SKCanvas canvas)
        {
            paint.Color = Color;
            canvas.DrawCircle(Center.X, Center.Y, Radius, paint);
        }

        public bool ProcessTouchEvent(long id, TouchActionType type, SKPoint location)
        {
            bool centerMoved = false;

            // Assumes Capture property of TouchEffect is true!
            switch (type)
            {
                case TouchActionType.Pressed:
                    if (!isBeingDragged && PointInCircle(location))
                    {
                        isBeingDragged = true;
                        touchId = id;
                        previousPoint = location;
                        centerMoved = false;
                    }
                    break;

                case TouchActionType.Moved:
                    if (isBeingDragged && touchId == id)
                    {
                        Center += location - previousPoint;
                        previousPoint = location;
                        centerMoved = true;
                    }
                    break;

                case TouchActionType.Released:
                    if (isBeingDragged && touchId == id)
                    {
                        Center += location - previousPoint;
                        isBeingDragged = false;
                        centerMoved = true;
                    }
                    break;

                case TouchActionType.Cancelled:
                    isBeingDragged = false;
                    break;
            }
            return centerMoved;
        }

        bool PointInCircle(SKPoint pt)
        {
            return (Math.Pow(pt.X - Center.X, 2) + Math.Pow(pt.Y - Center.Y, 2)) < (Radius * Radius);
        }
    }
}
