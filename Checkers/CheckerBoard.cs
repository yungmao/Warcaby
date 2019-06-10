using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersBoard
{
    //Oznaczenia pionków
    // -1 nieprawidłowe pole
    // 0 - puste
    // 1 - czerwony
    // 2 - czarny
    // 3 - czerwony król
    // 4  - czarny król
    class CheckerBoard
    {
        public int[,] board = new int[8, 8];

        //tworzenie planszy
        public CheckerBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    board[r, c] = -1;
                }
            }
        }

        //ustawienie pola
        public bool SetState(int r, int c, int state)
        {
            if ((state > 4) || (state < -1))
                return false;
            board[r, c] = state;
            return true;
        }

        //sprawdzenie wartosci pola
        public int GetState(int r, int c)
        {
            if ((r > 7) || (r < 0) || (c > 7) || (c < 0))
                return -1;
            return board[r, c];
        }

        //sprawdzanie mozliwych atakow/ruchow
        public List<Move> checkJumps(string color)
        {
            List<Move> jumps = new List<Move>();

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (color == "Red")
                    {
                        //dla króla
                        if (GetState(r, c) == 3)
                        {
                            if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 2) || (GetState(r - 1, c - 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c - 2)));
                            }
                            if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 2) || (GetState(r - 1, c + 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c + 2)));
                            }
                            if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 2) || (GetState(r + 1, c - 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c - 2)));
                            }
                            if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 2) || (GetState(r + 1, c + 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c + 2)));
                            }
                        }        
                        //dla pionka
                        if (GetState(r, c) == 1)
                        {
                            if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 2) || (GetState(r + 1, c - 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c - 2)));
                            }
                            if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 2) || (GetState(r + 1, c + 1) == 4)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c + 2)));
                            }
                        }
                    }
                 //dla gracza
                    if (color == "Black")
                    {
                        //dla króla
                        if (GetState(r, c) == 4)
                        {
                            if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 1) || (GetState(r - 1, c - 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c - 2)));
                            }
                            if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 1) || (GetState(r - 1, c + 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c + 2)));
                            }
                            if ((GetState(r + 2, c - 2) == 0) && ((GetState(r + 1, c - 1) == 1) || (GetState(r + 1, c - 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c - 2)));
                            }
                            if ((GetState(r + 2, c + 2) == 0) && ((GetState(r + 1, c + 1) == 1) || (GetState(r + 1, c + 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r + 3, c + 2)));
                            }
                        }
                        //dla pionka
                        if (GetState(r, c) == 2)
                        {
                            if ((GetState(r - 2, c - 2) == 0) && ((GetState(r - 1, c - 1) == 1) || (GetState(r - 1, c - 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c - 2)));
                            }
                            if ((GetState(r - 2, c + 2) == 0) && ((GetState(r - 1, c + 1) == 1) || (GetState(r - 1, c + 1) == 3)))
                            {
                                jumps.Add(new Move(new Piece(r + 1, c), new Piece(r - 1, c + 2)));
                            }
                        }
                    }
                }
            }

            return jumps;
        }
    }
}
