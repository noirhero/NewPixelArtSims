using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimStatePreset : MonoBehaviour {
    private static Dictionary<string, int> stateNameAtHashes;
    public static int GetHash(string stateName) {
        stateNameAtHashes.TryGetValue(stateName, out var hash);
        return hash;
    }

    public List<string> stateNames;
    private void Start() {
        stateNameAtHashes = new Dictionary<string, int>();

        foreach (var stateName in stateNames) {
            stateNameAtHashes.Add(stateName, stateName.Sum(Convert.ToInt32));
        }
    }
}