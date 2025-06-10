using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct Point
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Point(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }   

    public static implicit operator Vector3(Point p)
    {
        return new Vector3(p.X, p.Y, p.Z);
    }
    
    public static implicit operator Point(Vector3 v)
    {
        return new Point(v.x, v.y, v.z);
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }

    public override readonly string ToString()
    {
        return $"Point({X}, {Y}, {Z})";
    }

    public override bool Equals(object obj)
    {
        return obj is Point point &&
               X == point.X &&
               Y == point.Y &&
               Z == point.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
