using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageData", menuName = "ScriptableObjects/LanguageData", order = 1)]
public class LanguageData : ScriptableObject
{
    public int Index;
    public TextAsset wordsText;
}
