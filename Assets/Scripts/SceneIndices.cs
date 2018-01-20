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
        Debug.LogWarning("Just returned 0 for scene ind. I wasn't expecting that.");
        return 0;
    }

    public static string GetName(int input)
    {
        // Hacky way of switching between scene names and scene numbers
        switch (input)
        {
            case 1:
                return "Pyramids";
            case 2:
                return "Stonehenge";
            case 3:
                return "EiffelTower";
            case 4:
                return "Pisa";
            case 5:
                return "SpaceNeedle";
            case 6:
                return "Taj";
            case 7:
                return "GoldenGate";
            case 8:
                return "Babel";
        }
        Debug.LogWarning("Just returned empty string for scene name. I wasn't expecting that.");
        return "";
    }
}
