public class TicTacToeLogic
{
    public bool WinCondition(int[] arr, int player)
    {
        if (((arr[0] == arr[1]) && (arr[1] == arr[2]) && (arr[0] == player)) || //Row 1
            ((arr[3] == arr[4]) && (arr[4] == arr[5]) && (arr[3] == player)) || //Row 2
            ((arr[6] == arr[7]) && (arr[7] == arr[8]) && (arr[6] == player)) || //Row 3
            ((arr[0] == arr[3]) && (arr[3] == arr[6]) && (arr[0] == player)) || //Column 1
            ((arr[1] == arr[4]) && (arr[4] == arr[7]) && (arr[1] == player)) || //Column 2
            ((arr[2] == arr[5]) && (arr[5] == arr[8]) && (arr[2] == player)) || //Column 3
            ((arr[0] == arr[4]) && (arr[4] == arr[8]) && (arr[0] == player)) || //Diagonal "\"
            ((arr[2] == arr[4]) && (arr[4] == arr[6]) && (arr[2] == player))    //Diagonal "/"
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}