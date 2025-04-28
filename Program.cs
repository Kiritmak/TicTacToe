using System.Diagnostics;
using System.Reflection;

namespace Game
{
  public static class IO
  {
    public static T ReadInput<T>() where T : IParsable<T>
    {
      T variable;
      do if (T.TryParse(Console.ReadLine(), null, out variable)) break; while (true);
      return variable;
    }
    public static int ReadInputRange(int mn = int.MinValue, int mx = int.MaxValue)
    {
      int variable = ReadInput<int>();
      while(variable>mx && variable < mn)
      {
        Console.WriteLine("Invalid Input");
        variable = ReadInput<int>();
      }
      return variable;
    }
    public static int CPUInput(List<int> lista)
    {
      Random rand = new Random();
      int selection;
      selection = rand.Next(0, lista.Count-1);
      return lista[selection];
    }
    public static int PlayerInput(List<int> lista)
    {
      int variable = 0;
      do
      {
        variable = ReadInput<int>();
        if (lista.Contains(variable)) break;
        else Console.WriteLine("Invalid Position");
      } while(true);
      return variable;
    }
  }

  public class TicTacToe : ICloneable
  {
    private char[,] status;
    public List<int> PossibleMoves;
    public TicTacToe(bool p1IsBot, bool p2IsBot)
    {
      status = new char[3, 3]
      {
        { ' ', ' ', ' ' },
        { ' ', ' ', ' ' },
        { ' ', ' ', ' ' }
      };
      P1IsBot = p1IsBot;
      P2IsBot = p2IsBot;
      PossibleMoves = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9};
    }

    public bool P1IsBot { get; set; }
    public bool P2IsBot { get; set; }

    public Func<List<int>, int> P1Func { get => P1IsBot ? IO.CPUInput : IO.PlayerInput; }
    public Func<List<int>, int> P2Func { get => P2IsBot ? IO.CPUInput : IO.PlayerInput; }
    public object Clone()
    {
      TicTacToe aux = new TicTacToe(false, true);
      for(int i = 0; i<3; i++)
        for(int j = 0; j < 3; j++)
          aux.status[i, j] = this.status[i, j];
      return aux;
    }

    public void Start()
    {
      int selection = 0;

      Console.Write("Select 1 for P1 to be controled by a player, or anything else to be controled by a bot: ");
      selection = IO.ReadInput<int>();
      P1IsBot = selection != 1;

      Console.Write("Select 1 for P2 to be controled by a player, or anything else to be controled by a bot: ");
      selection = IO.ReadInput<int>();
      P2IsBot = selection != 1;

      while (!Tie())
        {

          this.ShowTable();

          if(!P1IsBot) Console.WriteLine("Select a place to play");
          do
          {
            if(P1IsBot) Debug.WriteLine("P1 is thinking");
            selection = P1Func(this.PossibleMoves);
            if (status[(selection - 1) / 3, (selection - 1) % 3] == ' ') break;
            else if (!P1IsBot) Console.WriteLine("That Position is currently occupied\n");
          } while (true);

          status[(selection - 1) / 3, (selection - 1) % 3] = 'X';
          PossibleMoves.Remove(selection);

          if (!P1IsBot) this.ShowTable();

          if (Wins())
          {
            if(P1IsBot) this.ShowTable();
            Console.WriteLine("Player 1 has won!\n*********************************");
            return;
          }
          if (Tie())
          {
            if(P1IsBot)this.ShowTable();
            break;
          }

          if (!P2IsBot) Console.WriteLine("Select a place to play");

          do
          {
            if (P1IsBot) Debug.WriteLine("P2 is thinking");
            selection = P2Func(this.PossibleMoves);
            if (status[(selection - 1) / 3, (selection - 1) % 3] == ' ') break;
            else if (!P2IsBot) Console.WriteLine("That Position is currently occupied\n");
          } while (true);
          status[(selection - 1) / 3, (selection - 1) % 3] = 'O';
          PossibleMoves.Remove(selection);

        if (Wins())
          {
            this.ShowTable();
            Console.WriteLine("Player 2 has won!\n*********************************");
            return;
          }
        }
      Console.WriteLine("It's a Tie :)!\n*********************************");
    }
    public void ShowTable()
    {
      Console.WriteLine();
      for (int i = 0; i < 3; i++)
      {
        Console.Write(" ");
        for (int j = 0; j < 3; j++)
          Console.Write(status[i, j] + (j==2 ? "" : " | "));

        if(i!=2)Console.Write("\n-----------\n");
      }
      Console.WriteLine("\n");
    }
    bool Wins()
    {
      for(int i = 0;i < 3;i++)
      {
        char winner = status[i, 0];
        for(int j = 1; j<3; j++)
        {
          if (winner != status[i, j] || winner == ' ') break; 
          if (j == 2) return true;
        }
        winner = status[0, i];
        for (int j = 1; j < 3; j++)
        {
          if (winner != status[j, i] || winner == ' ') break;
          if (j == 2) return true;
        }
      }
      char winner2 = status[0, 0];
      for(int i = 0; i<3; i++)
      {
        if (winner2 != status[i, i] || winner2 == ' ') break;
        if(i==2) return true;
      }
      winner2 = status[2, 0];
      for (int i = 0; i < 3; i++)
      {
        if (winner2 != status[2-i, i] || winner2 == ' ') break;
        if (i == 2) return true;
      }
      return false;
    }
    bool Tie()
    {
      int c = 0;
      for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++) c += status[i, j] == ' ' ? 0 : 1; 
      return c==9;
    }
  }


  class GameStart
  {
    static void Main(string[] args)
    {
      TicTacToe MyGame = new TicTacToe(false, true);
      Console.WriteLine("Welcome to TicTacToe, let's play!");
      while (true)
      {
        int opcion;

        Console.WriteLine("Select an option:");
        Console.WriteLine("1 => Start new game");

        opcion = IO.ReadInput<int>();

        if(opcion == 1 ) MyGame = new TicTacToe(false, true); 
        if (opcion <= 0 || opcion >= 2) break;

        MyGame.Start();

      }
      Console.WriteLine("Thanks for playing!");
      Console.Read();
    }
  }
}
