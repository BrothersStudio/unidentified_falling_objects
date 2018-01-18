using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneIndices
{
    public static int GetIndex(string input)
    {
        // Hacky way of switching between scene names and scene numbers
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
            case "SpaceNeedle":
                return 5;
            case "Taj":
                return 6;
            case "GoldenGate":
                return 7;
            case "Babel":
                return 8;
        }
        return 0;
    }
}
