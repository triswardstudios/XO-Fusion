using System.Linq;
using UnityEngine;

public class EasyBotLogic 
{

    public int MakeMove(int[] arr)
    {
        int pos = Random.Range(0, 8);
        if (arr.Contains(0))
        {
            while (arr[pos] != 0)
            {
                pos = Random.Range(0, 8);
            }
            return pos;
        }
        else return -1;
    }
}
