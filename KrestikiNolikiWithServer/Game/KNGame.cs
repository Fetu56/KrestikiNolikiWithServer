namespace Server.Game
{
    class KNGame
    {
        public Field[,] map { get; private set; }
        bool KTurn;
        private static int FieldSize = 3;
        Field lastTurn = Field.UKN;
        public Field GameStatus { get; private set; }
        public KNGame()
        {
            map = new Field[FieldSize, FieldSize];
            KTurn = true;
        }

        public bool SetToField(int i, int j)
        {
            bool res = false;
            if (i >= 0 && i < FieldSize && j >= 0 && j < FieldSize)
            {
                if (map[i, j] == Field.Null)
                {
                    map[i, j] = KTurn ? Field.Krest : Field.Nolik;
                    res = true;
                    lastTurn = KTurn ? Field.Krest : Field.Nolik;
                    KTurn = !KTurn;
                }
                GameStatus = CheckGameEnded(i, j);
            }
            return res;
        }

        private Field CheckGameEnded(int x, int y)
        {
            Field res = Field.Null;
            bool haveNull = true;
            for (int i = 0; i < FieldSize; i++)
            {
                if (map[i, y] != lastTurn)
                {
                    break;
                }
                if (i + 1 == FieldSize)
                {
                    res = map[i, y];
                }
            }
            if (res == Field.Null)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    if (map[x, j] != lastTurn)
                    {
                        break;
                    }
                    if (j + 1 == FieldSize)
                    {
                        res = map[x, j];
                    }
                }

                if (res == Field.Null)
                {
                    if (x == y)
                    {
                        for (int i = 0; i < FieldSize; i++)
                        {
                            if (map[i, i] != lastTurn)
                            {
                                break;
                            }
                            if (i + 1 == FieldSize)
                            {
                                res = map[i, i];
                            }
                        }
                    }
                    if (res == Field.Null)
                    {
                        if (x + y + 1 == FieldSize)
                        {
                            for (int i = 0; i < FieldSize; i++)
                            {
                                if (map[i, FieldSize - 1 - i] != lastTurn)
                                {
                                    break;
                                }
                                if (i + 1 == FieldSize)
                                {
                                    res = map[i, FieldSize - 1 - i];
                                }
                            }
                        }
                        if (res == Field.Null)
                        {
                            haveNull = false;
                            for (int i = 0; i < FieldSize; i++)
                            {
                                for (int j = 0; j < FieldSize; j++)
                                {
                                    if (map[i, j] == Field.Null)
                                    {
                                        haveNull = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }


                }
            }


            return haveNull ? res : Field.UKN;
        }
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    str += map[j, i] == Field.Null ? '_' : (map[j, i] == Field.Krest ? 'X' : 'O');
                }
                str += '\n';
            }
            return str;
        }
    }
}
