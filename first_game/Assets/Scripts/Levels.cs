using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Level
{

    public static GameTools.level GetLevel(int level)
    {
        switch (level)
        {
            case 1:
                return GetLevel1();
            default:
                return new GameTools.level();
        }
    }

    private static GameTools.level GetLevel1()
    {
        GameTools.level level = new GameTools.level();
        level.Initializate();
        level.Add(GameTools.FRONT_SIDE, 1, GameTools.BRICK_STONE, 0);
        level.Add(GameTools.RIGHT_SIDE, 5, GameTools.BRICK_STONE, 1);
        return level;
    }

}
