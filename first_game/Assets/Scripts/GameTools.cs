using System.Collections.Generic;
using UnityEngine;

public static class GameTools
{
    public const int FRONT_SIDE = 0;
    public const int LEFT_SIDE = 1;
    public const int BACK_SIDE = 2;
    public const int RIGHT_SIDE = 3;

    public const int BRICK_WOOD = 0;
    public const int BRICK_STONE = 1;

    public struct position
    {
        int side;
        int pos;

        public position(int side, int pos)
        {
            this.side = side;
            this.pos = pos;
        }

        public int GetSide()
        {
            return side;
        }

        public int GetPosition()
        {
            return pos;
        }
    }

    public struct brickInf
    {
        private position position;
        private int type;
        private int wave;

        public brickInf(position position, int type, int wave)
        {
            this.position = position;
            this.type = type;
            this.wave = wave;
        }

        public brickInf(int side, int position, int type, int wave)
        {
            this.position = new position(side, position);
            this.type = type;
            this.wave = wave;
        }

        public int GetSide()
        {
            return position.GetSide();
        }

        public int GetPosition()
        {
            return position.GetPosition();
        }

        public int GetBrickType()
        {
            return type;
        }

        public int GetWave()
        {
            return wave;
        }
    }

    public struct level
    {
        private List<brickInf> poolBricks;

        public void Initializate()
        {
            poolBricks = new List<brickInf>();
        }

        public void Add(brickInf brk)
        {
            poolBricks.Add(brk);
        }

        public void Add(int side, int position, int type, int wave)
        {
            poolBricks.Add(new brickInf(side, position, type, wave));
        }

        public List<brickInf> GetPoolBricks()
        {
            return poolBricks;
        }

    }

}
