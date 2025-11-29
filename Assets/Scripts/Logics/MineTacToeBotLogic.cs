using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class MineTacToeBotLogic : NormalBotLogic
{
    private static readonly int[][] WinningCombos = new int[][]
    {
        new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8}, // Rows
        new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8}, // Columns
        new[] {0, 4, 8}, new[] {2, 4, 6}                   // Diagonals
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int MakeMoveMTT(int count, int player, int[] arr)
    {

        int winningMove = CheckWinningMove(arr, player);
        if (winningMove != -1)
            return winningMove;

        // Try to block opponent
        //int opponentId = (botId == 1) ? 2 : 1;
        int blockingMove = GameModeMTT(arr, player, count);
        return blockingMove;
    }
    private int GameModeMTT(int[] arr, int player, int count)
    {
        if (arr[4] != 0)
        {
            if (count < 2)
            {
                int[] corners = new int[] { 0, 2, 6, 8 };
                int pos = Random.Range(0, 3);
                return corners[pos];
            }
            else
            {
                foreach (var combo in WinningCombos)
                {
                    int a = combo[0], b = combo[1], c = combo[2];
                    if (arr[a] == arr[b] && arr[a] != player && arr[c] == 0 && arr[a] != 0)
                        return c;
                    else if (arr[b] == arr[c] && arr[b] != player && arr[a] == 0 && arr[b] != 0)
                        return a;
                    else if (arr[a] == arr[c] && arr[a] != player && arr[b] == 0 && arr[a] != 0)
                        return b;
                    else
                        continue;
                }
                return MakeMove(arr);
            }
        }
        else
        {
            if (count < 2)
            {
                return MakeMove(arr);
            }
            else
                return 4;
        }
    }
}
