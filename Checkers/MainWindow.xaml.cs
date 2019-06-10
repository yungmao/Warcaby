using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace CheckersBoard
{
  
    public partial class MainWindow : Window
    {
        private Move currentMove;
        private String winner;
        private String turn;


        public MainWindow()
        {
            InitializeComponent();
            this.Title = "WARCABY - gracz gra czarnymi, tura gracza";
            currentMove = null;
            winner = null;
            turn = "Black";
            MakeBoard();
        }
        //czyszczenie tablicy
        private void ClearBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    CheckersGrid.Children.Remove(stackPanel);
                }
            }
        }
        //tworzenie tablicy
        private void MakeBoard()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = new StackPanel();
                    if (r % 2 == 1)
                    {
                        if (c % 2 == 0)
                            stackPanel.Background = Brushes.White;
                        else
                            stackPanel.Background = Brushes.Beige;
                    }
                    else
                    {
                        if (c % 2 == 0)
                            stackPanel.Background = Brushes.Beige;
                        else
                            stackPanel.Background = Brushes.White;
                    }
                    Grid.SetRow(stackPanel, r);
                    Grid.SetColumn(stackPanel, c);
                    CheckersGrid.Children.Add(stackPanel);
                }
            }

            MakeButtons();
        }
        //tworzenie przyciskow, zeby mozan bylo klikac na pionki
        private void MakeButtons()
        {
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(button_Click);
                    button.Height = 60;
                    button.Width = 60;
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    button.VerticalAlignment = VerticalAlignment.Center;
                    var redBrush = new ImageBrush();
                    redBrush.ImageSource = new BitmapImage(new Uri("Resources/red60p.png", UriKind.Relative));
                    var blackBrush = new ImageBrush();
                    blackBrush.ImageSource = new BitmapImage(new Uri("Resources/black60p.png", UriKind.Relative));
                    //cala plansza to 8x8
                    switch (r)
                    {
                        case 1:
                            if (c % 2 == 1)
                            {

                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 2:
                            if (c % 2 == 0)
                            {
                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 3:
                            if (c % 2 == 1)
                            {
                                button.Background = redBrush;
                                button.Name = "buttonRed" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 4:
                            if (c % 2 == 0)
                            {
                                button.Background = Brushes.Beige;
                                button.Name = "button" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 5:
                            if (c % 2 == 1)
                            {
                                button.Background = Brushes.Beige;
                                button.Name = "button" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 6:
                            if (c % 2 == 0)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 7:
                            if (c % 2 == 1)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        case 8:
                            if (c % 2 == 0)
                            {
                                button.Background = blackBrush;
                                button.Name = "buttonBlack" + r + c;
                                stackPanel.Children.Add(button);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //sprawdzanie siatki mozliwosci daneego pionka
        UIElement GetGridElement(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;
        }
        //okreslanie co sie dzieje po kliknieciu pionka
        public void button_Click(Object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)button.Parent;
            int row = Grid.GetRow(stackPanel);
            int col = Grid.GetColumn(stackPanel);
            Console.WriteLine("Row: " + row + " Column: " + col);
            if (currentMove == null)
                currentMove = new Move();
            if (currentMove.piece1 == null)
            {
                currentMove.piece1 = new Piece(row, col);
                stackPanel.Background = Brushes.Green;
            }
            else
            {
                currentMove.piece2 = new Piece(row, col);
                stackPanel.Background = Brushes.Green;
            }
            if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
            {
                if (CheckMove())
                {
                    MakeMove();
                    aiMakeMove();
                }
            }
        }
        //sprawdzaeni czy dobry pionek sie kliknelo
        private Boolean CheckMove()
        {
            StackPanel stackPanel1 = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
            StackPanel stackPanel2 = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
            Button button1 = (Button) stackPanel1.Children[0];
            Button button2 = (Button) stackPanel2.Children[0];
            stackPanel1.Background = Brushes.Beige;
            stackPanel2.Background = Brushes.Beige;

            if ((turn == "Black") && (button1.Name.Contains("Red")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                displayError("To jest tura przeciwnika.");
                return false;
            }
            if ((turn == "Red") && (button1.Name.Contains("Black")))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                displayError("To jest tura przeciwnika");
                return false;
            }
            if (button1.Equals(button2))
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                return false;
            }
            if(button1.Name.Contains("Black"))
            {
                return CheckMoveBlack(button1, button2);
            }
            else if (button1.Name.Contains("Red"))
            {
                return CheckMoveRed(button1, button2);
            }
            else
            {
                currentMove.piece1 = null;
                currentMove.piece2 = null;
                Console.WriteLine("False");
                return false;
            }
        }
        //sprawdzanie ruchow
        private bool CheckMoveRed(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Red");
            //jesli jest mozliwosc nalezy wykonac bicie
            if (jumpMoves.Count > 0)
            {
                bool invalid = true; //komputer musi wykonac bibice jak zobaczy
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    displayError("Należy wykonać bicie");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Console.WriteLine("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Red"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Black"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Red")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Red");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Black"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            displayError("Nieprawidłowy ruch");
            return false;
        }
        //sprawdzanie mozliwosci gracza
        private bool CheckMoveBlack(Button button1, Button button2)
        {
            CheckerBoard currentBoard = GetCurrentBoard();
            List<Move> jumpMoves = currentBoard.checkJumps("Black");

            if (jumpMoves.Count > 0)
            {
                bool invalid = false; //true, poniewaz na poczatku chcialem uzyc zasady zeby kazdy musiac wykonac bibice jesli musi, ale to chyba nie bbyl najlepszy pomysl
                foreach (Move move in jumpMoves)
                {
                    if (currentMove.Equals(move))
                        invalid = false;
                }
                if (invalid)
                {
                    displayError("Należy wykonać bicie");
                    currentMove.piece1 = null;
                    currentMove.piece2 = null;
                    Console.WriteLine("False");
                    return false;
                }
            }

            if (button1.Name.Contains("Black"))
            {
                if (button1.Name.Contains("King"))
                {
                    if ((currentMove.isAdjacent("King")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("King");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Red"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
                else
                {
                    if ((currentMove.isAdjacent("Black")) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                        return true;
                    Piece middlePiece = currentMove.checkJump("Black");
                    if ((middlePiece != null) && (!button2.Name.Contains("Black")) && (!button2.Name.Contains("Red")))
                    {
                        StackPanel middleStackPanel = (StackPanel)GetGridElement(CheckersGrid, middlePiece.Row, middlePiece.Column);
                        Button middleButton = (Button)middleStackPanel.Children[0];
                        if (middleButton.Name.Contains("Red"))
                        {
                            CheckersGrid.Children.Remove(middleStackPanel);
                            addBlackButton(middlePiece);
                            return true;
                        }
                    }
                }
            }
            currentMove = null;
            displayError("Nieprawidłowy ruch");
            return false;
       }
        //wykonywanie ruchu
        private void MakeMove()
        {
            if ((currentMove.piece1 != null) && (currentMove.piece2 != null))
            {
                Console.WriteLine("Piece1 " + currentMove.piece1.Row + ", " + currentMove.piece1.Column);
                Console.WriteLine("Piece2 " + currentMove.piece2.Row + ", " + currentMove.piece2.Column);
                StackPanel stackPanel1 = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece1.Row, currentMove.piece1.Column);
                StackPanel stackPanel2 = (StackPanel)GetGridElement(CheckersGrid, currentMove.piece2.Row, currentMove.piece2.Column);
                CheckersGrid.Children.Remove(stackPanel1);
                CheckersGrid.Children.Remove(stackPanel2);
                Grid.SetRow(stackPanel1, currentMove.piece2.Row);
                Grid.SetColumn(stackPanel1, currentMove.piece2.Column);
                CheckersGrid.Children.Add(stackPanel1);
                Grid.SetRow(stackPanel2, currentMove.piece1.Row);
                Grid.SetColumn(stackPanel2, currentMove.piece1.Column);
                CheckersGrid.Children.Add(stackPanel2);
                checkKing(currentMove.piece2);
                currentMove = null;
                if (turn == "Black")
                {
                    this.Title = "WARCABY - gracz gra czarnymi, tura komputera";
                    turn = "Red";
                }
                else if (turn == "Red")
                {
                    this.Title = "WARCABY - gracz gra czarnymi, tura gracza";
                    turn = "Black";
                }
                checkWin();
            }
        }
        //ruch komputera
        private void aiMakeMove()
        {
            currentMove = CheckersAI.GetMove(GetCurrentBoard());
            if (currentMove != null)
            {
                if (CheckMove())
                {
                    MakeMove();
                }
            }
        }

        private CheckerBoard GetCurrentBoard()
        {
            CheckerBoard board = new CheckerBoard();
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    if (stackPanel.Children.Count > 0)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        if (button.Name.Contains("Red"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 3);
                            else
                                board.SetState(r - 1, c, 1);
                        }
                        else if (button.Name.Contains("Black"))
                        {
                            if (button.Name.Contains("King"))
                                board.SetState(r - 1, c, 4);
                            else
                                board.SetState(r - 1, c, 2);
                        }
                        else
                            board.SetState(r - 1, c, 0);

                    }
                    else
                    {
                        board.SetState(r - 1, c, -1);
                    }

                }
            }
            return board;
        }

        private void checkKing(Piece tmpPiece)
        {
            StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, tmpPiece.Row, tmpPiece.Column);
            if (stackPanel.Children.Count > 0)
            {
                Button button = (Button)stackPanel.Children[0];
                var redBrush = new ImageBrush();
                redBrush.ImageSource = new BitmapImage(new Uri("Resources/red60p_king.png", UriKind.Relative));
                var blackBrush = new ImageBrush();
                blackBrush.ImageSource = new BitmapImage(new Uri("Resources/black60p_king.png", UriKind.Relative));
                if ((button.Name.Contains("Black")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 1)
                    {
                        button.Name = "button" + "Black" + "King" + tmpPiece.Row + tmpPiece.Column;
                        button.Background = blackBrush;
                    }
                }
                else if ((button.Name.Contains("Red")) && (!button.Name.Contains("King")))
                {
                    if (tmpPiece.Row == 8)
                    {
                        button.Name = "button" + "Red" + "King" + tmpPiece.Row + tmpPiece.Column;
                        button.Background = redBrush;
                    }
                }
            }
        }
        
        private void addBlackButton(Piece middleMove)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Background = Brushes.Beige;
            Button button = new Button();
            button.Click += new RoutedEventHandler(button_Click);
            button.Height = 60;
            button.Width = 60;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Background = Brushes.Beige;
            button.Name = "button" + middleMove.Row + middleMove.Column;
            stackPanel.Children.Add(button);
            Grid.SetColumn(stackPanel, middleMove.Column);
            Grid.SetRow(stackPanel, middleMove.Row);
            CheckersGrid.Children.Add(stackPanel);
        }

        private void checkWin()
        {
            int totalBlack = 0, totalRed = 0;
            for (int r = 1; r < 9; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                    if (stackPanel.Children.Count > 0)
                    {
                        Button button = (Button)stackPanel.Children[0];
                        if (button.Name.Contains("Red"))
                            totalRed++;
                        if (button.Name.Contains("Black"))
                            totalBlack++;
                    }
                }
            }
            if (totalBlack == 0)
                winner = "Czerwony";
            if (totalRed == 0)
                winner = "Czarny";
            if (winner != null)
            {
                for (int r = 1; r < 9; r++)
                {
                    for (int c = 0; c < 8; c++)
                    {
                        StackPanel stackPanel = (StackPanel)GetGridElement(CheckersGrid, r, c);
                        if (stackPanel.Children.Count > 0)
                        {
                            Button button = (Button)stackPanel.Children[0];
                            button.Click -= new RoutedEventHandler(button_Click);
                        }
                    }
                }
                MessageBoxResult result = MessageBox.Show(winner + " jest wygranym, chcesz zagrać jeszcze raz?", "Winner", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    newGame();
            }
        }

        private void newGame()
        {
            currentMove = null;
            winner = null;
            this.Title = "WARCABY - gracz gra czarnymi, tura gracza";
            turn = "Black";
            ClearBoard();
            MakeBoard();
        }

        private void displayError(string error)
        {
            MessageBox.Show(error, "Nieprawidłowy ruch", MessageBoxButton.OK);
        }

        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
