using BeatBoards.Replays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatBoards.Utilities
{
    public class Conversions
    {
        public static int FindClosestIndex(List<PositionData> infos, float targetProgress) //From Andruzzzhka
        {
            for (int i = 0; i < infos.Count - 1; i++)
            {
                if ((infos[i].SongTime < targetProgress && infos[i + 1].SongTime > targetProgress) || Mathf.Abs(infos[i].SongTime - targetProgress) < float.Epsilon)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
