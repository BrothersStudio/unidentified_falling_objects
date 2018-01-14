using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneIndices
{
    public static int GetIndex(string input)
    {
        switch (input)
        {
            case "Pyramids":
                return 1;
            case "Stonehenge":
                return 2;
            case "EiffelTower":
                return 3;
            case "Pisa":
                return 4;
        }
        return 0;
    }
}
