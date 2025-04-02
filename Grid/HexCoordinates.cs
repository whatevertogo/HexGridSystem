using UnityEngine;
using System;

namespace Core.Grid
{
    [Serializable]
    public struct HexCoordinates : IEquatable<HexCoordinates>
    {
        [SerializeField]
        private int x, y;

        public int X => x;
        public int Y => y;
        public int Z => -X - Y;

        public HexCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static HexCoordinates FromOffsetCoordinates(int x, int y)
        {
            return new HexCoordinates(x - y / 2, y);
        }

        public static HexCoordinates FromPosition(Vector3 position)
        {
            float x = position.x / (HexMetrics.innerRadius * 2f);
            float y = position.y / (HexMetrics.outerRadius * 1.5f);
            
            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY)
                {
                    iZ = -iX - iY;
                }
            }

            return new HexCoordinates(iX, iY);
        }

        public HexCoordinates GetNeighbor(HexDirection direction)
        {
            return direction switch
            {
                HexDirection.NE => new HexCoordinates(X + 1, Y - 1),
                HexDirection.E => new HexCoordinates(X + 1, Y),
                HexDirection.SE => new HexCoordinates(X, Y + 1),
                HexDirection.SW => new HexCoordinates(X - 1, Y + 1),
                HexDirection.W => new HexCoordinates(X - 1, Y),
                HexDirection.NW => new HexCoordinates(X, Y - 1),
                _ => throw new ArgumentException("Invalid direction")
            };
        }

        public bool Equals(HexCoordinates other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is HexCoordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static bool operator ==(HexCoordinates a, HexCoordinates b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(HexCoordinates a, HexCoordinates b)
        {
            return !a.Equals(b);
        }
    }

    public enum HexDirection
    {
        NE, E, SE, SW, W, NW
    }
}