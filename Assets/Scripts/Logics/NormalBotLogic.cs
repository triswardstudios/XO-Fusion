using UnityEngine;

public class NormalBotLogic : EasyBotLogic
{
    private static readonly int[][] WinningCombos = new int[][]
    {
        new[] {0, 1, 2}, new[] {3, 4, 5}, new[] {6, 7, 8}, // Rows
        new[] {0, 3, 6}, new[] {1, 4, 7}, new[] {2, 5, 8}, // Columns
        new[] {0, 4, 8}, new[] {2, 4, 6}                   // Diagonals
    };

    public int MakeMoveNormal(int count, int player, int[] arr) //Replace player later with turn
    {
        int winningMove = CheckWinningMove(arr, player);
        if (winningMove != -1)
            return winningMove;

        // Try to block opponent
        //int opponentId = (botId == 1) ? 2 : 1;
        int blockingMove = CheckOpponentGPT(arr, player, count);
        return blockingMove;
    }

    private int CheckOpponent(int[] arr, int player)
    {
        //Row 1 Logic
        if ((arr[0] == arr[1]) && (arr[0] != player) && (arr[2] == 0) && (arr[0] != 0))
        {
            return 2;
        }
        else if ((arr[1] == arr[2]) && (arr[1] != player) && (arr[0] == 0) && (arr[1] != 0))
        {
            return 0;
        }
        else if ((arr[0] == arr[2]) && (arr[0] != player) && (arr[1] == 0) && (arr[2] != 0))
        {
            return 1;
        }
        //Row 2 Logic
        else if ((arr[3] == arr[4]) && (arr[3] != player) && (arr[5] == 0) && (arr[3] != 0))
        {
            return 5;
        }
        else if ((arr[4] == arr[5]) && (arr[4] != player) && (arr[3] == 0) && (arr[4] != 0))
        {
            return 3;
        }
        else if ((arr[3] == arr[5]) && (arr[3] != player) && (arr[4] == 0) && (arr[5] != 0))
        {
            return 4;
        }
        //Row 3 Logic
        else if ((arr[6] == arr[7]) && (arr[6] != player) && (arr[8] == 0) && (arr[6] != 0))
        {
            return 8;
        }
        else if ((arr[7] == arr[8]) && (arr[7] != player) && (arr[6] == 0) && (arr[7] != 0))
        {
            return 6;
        }
        else if ((arr[6] == arr[8]) && (arr[6] != player) && (arr[7] == 0) && (arr[8] != 0))
        {
            return 7;
        }
        //Col 1 Logic
        else if ((arr[0] == arr[3]) && (arr[0] != player) && (arr[6] == 0) && (arr[0] != 0))
        {
            return 6;
        }
        else if ((arr[3] == arr[6]) && (arr[3] != player) && (arr[0] == 0) && (arr[3] != 0))
        {
            return 0;
        }
        else if ((arr[0] == arr[6]) && (arr[0] != player) && (arr[3] == 0) && (arr[6] != 0))
        {
            return 3;
        }
        //Col 2 Logic
        else if ((arr[1] == arr[4]) && (arr[1] != player) && (arr[7] == 0) && (arr[1] != 0))
        {
            return 7;
        }
        else if ((arr[4] == arr[7]) && (arr[4] != player) && (arr[1] == 0) && (arr[4] != 0))
        {
            return 1;
        }
        else if ((arr[1] == arr[7]) && (arr[1] != player) && (arr[4] == 0) && (arr[7] != 0))
        {
            return 4;
        }
        //Col 3 Logic
        else if ((arr[2] == arr[5]) && (arr[2] != player) && (arr[8] == 0) && (arr[2] != 0))
        {
            return 8;
        }
        else if ((arr[5] == arr[8]) && (arr[5] != player) && (arr[2] == 0) && (arr[5] != 0))
        {
            return 2;
        }
        else if ((arr[2] == arr[8]) && (arr[2] != player) && (arr[5] == 0) && (arr[8] != 0))
        {
            return 5;
        }
        //Diagonal "\" Logic
        else if ((arr[0] == arr[4]) && (arr[0] != player) && (arr[8] == 0) && (arr[0] != 0))
        {
            return 8;
        }
        else if ((arr[4] == arr[8]) && (arr[4] != player) && (arr[0] == 0) && (arr[4] != 0))
        {
            return 0;
        }
        else if ((arr[0] == arr[8]) && (arr[0] != player) && (arr[4] == 0) && (arr[8] != 0))
        {
            return 4;
        }
        //Diagonal "/" Logic
        else if ((arr[2] == arr[4]) && (arr[2] != player) && (arr[6] == 0) && (arr[2] != 0))
        {
            return 6;
        }
        else if ((arr[4] == arr[6]) && (arr[4] != player) && (arr[2] == 0) && (arr[4] != 0))
        {
            return 2;
        }
        else if ((arr[2] == arr[6]) && (arr[2] != player) && (arr[4] == 0) && (arr[6] != 0))
        {
            return 4;
        }
        else
        {
            if ((arr[4] == 0) && ((!CheckBox(arr, 0, player) && !CheckBox(arr, 0, 0)) || (!CheckBox(arr, 2, player) && !CheckBox(arr, 2, 0)) ||
                                (!CheckBox(arr, 6, player) && !CheckBox(arr, 6, 0)) || (!CheckBox(arr, 8, player) && !CheckBox(arr, 8, 0))))
            {
                return 4;
            }
            return MakeMove(arr);
        }
    }

    private int CheckOpponentGPT(int[] arr, int player, int count)
    {
        if (arr[4] != 0)
        {
            if (count < 2)
            {
                int[] corners = new int[] { 0, 2, 6, 8 };
                int pos = Random.Range(0, 4);
                return corners[pos];
            }
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
        else
        {
            //if ((arr[4] == 0) && ((!CheckBox(arr, 0, player) && !CheckBox(arr, 0, 0)) || (!CheckBox(arr, 2, player) && !CheckBox(arr, 2, 0)) ||
            //                (!CheckBox(arr, 6, player) && !CheckBox(arr, 6, 0)) || (!CheckBox(arr, 8, player) && !CheckBox(arr, 8, 0))))
            //{
            //    return 4;
            //}
            //return MakeMove(arr);
            return 4;
        }
    }

    public int CheckWinningMove(int[] arr, int botId)
    {
        foreach (var combo in WinningCombos)
        {
            int a = combo[0], b = combo[1], c = combo[2];

            if (arr[a] == arr[b] && arr[a] == botId && arr[c] == 0)
                return c;
            if (arr[b] == arr[c] && arr[b] == botId && arr[a] == 0)
                return a;
            if (arr[a] == arr[c] && arr[a] == botId && arr[b] == 0)
                return b;
        }

        return -1; // no winning move
    }

    public bool CheckBox(int[] arr, int pos, int value)
    {
        if (arr[pos] == value) { return true; }
        return false;
    }
}