using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Mines",menuName ="MineType")]
public class MinesScriptable : ScriptableObject
{

    public List<Mines> minesList;
   
    [System.Serializable]
    public class Mines
    {
        public string mineName;
        public float mineValue;
        public Material mineMaterial;
    }
}
