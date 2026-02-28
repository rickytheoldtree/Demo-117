using System.Collections.Generic;
using UnityEngine;

namespace Demo_117.SO
{
    [CreateAssetMenu(fileName = "SpriteList", menuName = "ScriptableObject/SpriteList")]
    public class SpriteList : ScriptableObject
    {
        public List<Sprite> sprites;
    }
}