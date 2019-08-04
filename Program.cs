using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEngine;
#endif

namespace ConnectAnimals
{
    public class Point
    {
        public int x, y, n_d, d;

        public Point(int x_, int y_, int n_d_, int d_)
        {
            x = x_;
            y = y_;
            n_d = n_d_;
            d = d_;
        }
    }

    public class PikachuMap
    {
        int[,] a; // map status
        int n, m; // map size
        int noc; // number of characters
        int not; // number of twin character
        int nh; // number of suggestion
        int level;
        private List<int> map = new List<int>();


        public PikachuMap(int n_, int m_, int noc_, int nh_)
        {
            n = n_;
            m = m_;
            noc = noc_;
            a = new int[n + 2, m + 2];
            // padding
            for (int i = 0; i <= n + 1; i++)
            {
                a[i, 0] = -1;
                a[i, m + 1] = -1;
            }
            for (int j = 0; j <= m + 1; j++)
            {
                a[0, j] = -1;
                a[n + 1, j] = -1;
            }
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    map.Add(i * m + j);
            nh = nh_;
        }

        public void SetLevel(int lv)
        {
            level = lv;
        }


        public void CreateMap()
        {
#if UNITY_EDITOR
            System.Random r1 = new System.Random();
            System.Random r2 = new System.Random(r1.Next(100));
#endif
#if !UNITY_EDITOR
            Random r1 = new Random();
            Random r2 = new Random(r1.Next(100));
#endif
            int last = n * m;
            not = last / 2;
            for (int i = 0; i < not; i++)
            {
                int id = r1.Next(noc);
                int p = r2.Next(last);
                a[map[p] / m + 1, map[p] % m + 1] = id;
                int tmp = map[p];
                map[p] = map[last - 1];
                map[last - 1] = tmp;
                last -= 1;
                p = r2.Next(last);
                a[map[p] / m + 1, map[p] % m + 1] = id;
                tmp = map[p];
                map[p] = map[last - 1];
                map[last - 1] = tmp;
                last -= 1;

            }

        }

        public void ReCreateMap()
        {
            System.Random r1 = new System.Random();
            System.Random r2 = new System.Random(r1.Next(100));
            int last = 0;
            List<int> rmap = new List<int>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (a[i + 1, j + 1] != -1)
                    {
                        rmap.Add(i * m + j);
                        last += 1;
                    }
            not = last / 2;
            for (int i = 0; i < not; i++)
            {
                int id = r1.Next(noc);
                // Console.WriteLine("Turn {0}, id = {1}", i, id);
                int p = r2.Next(last);
                a[rmap[p] / m + 1, rmap[p] % m + 1] = id;
                int tmp = rmap[p];
                rmap[p] = rmap[last - 1];
                rmap[last - 1] = tmp;
                last -= 1;
                p = r2.Next(last);
                a[rmap[p] / m + 1, rmap[p] % m + 1] = id;
                tmp = rmap[p];
                rmap[p] = rmap[last - 1];
                rmap[last - 1] = tmp;
                last -= 1;

            }
        }

        public Tuple<int, int, int, int> SwapMaxX(int x1, int y1, int x2, int y2)
        {
            if (x1 < x2)
                return new Tuple<int, int, int, int>(x2, y2, x1, y1);
            else
                return new Tuple<int, int, int, int>(x1, y1, x2, y2);
        }

        public Tuple<int, int, int, int> SwapMinX(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
                return new Tuple<int, int, int, int>(x2, y2, x1, y1);
            else
                return new Tuple<int, int, int, int>(x1, y1, x2, y2);
        }

        public Tuple<int, int, int, int> SwapMinY(int x1, int y1, int x2, int y2)
        {
            if (y1 > y2)
                return new Tuple<int, int, int, int>(x2, y2, x1, y1);
            else
                return new Tuple<int, int, int, int>(x1, y1, x2, y2);
        }

        public Tuple<int, int, int, int> SwapMaxY(int x1, int y1, int x2, int y2)
        {
            if (y1 < y2)
                return new Tuple<int, int, int, int>(x2, y2, x1, y1);
            else
                return new Tuple<int, int, int, int>(x1, y1, x2, y2);
        }


        public void Update()
        {
            not -= 1;
        }

        /*
        return Tuple<bool, int, int, int, int>
            - Item1: number of suggestion > 0 or not (co con luot suggest nua hay ko)
            - Item2, Item3, Item4, Item5: x1, y1, x2, y2
        */
        public Tuple<bool, int, int, int, int> Hint()
        {
            // TODO
            if (nh < 0)
                return new Tuple<bool, int, int, int, int>(false, -1, -1, -1, -1);
            return ExistPair();

        }

        /*
        return Tuple<bool, int, int, int, int>
            - Item1: Exist Pair or not
            - Item2, Item3, Item4, Item5: x1, y1, x2, y2
        */
        public Tuple<bool, int, int, int, int> ExistPair()
        {
            // TODO
            Dictionary<int, List<int>> L = new Dictionary<int, List<int>>();
            for (int k = 0; k < noc; k++)
            {
                List<int> P = new List<int>();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        if (a[i + 1, j + 1] == k)
                            P.Add(i * m + j);
                L.Add(k, P);
            }
            int x1, y1, x2, y2;
            x1 = y1 = x2 = y2 = -1;
            for (int k = 0; k < noc; k++)
            {
                List<int> P = L[k];
                foreach (int p1 in P)
                    foreach (int p2 in P)
                        if (p1 != p2)
                        {
                            x1 = p1 / m + 1;
                            y1 = p1 % m + 1;
                            x2 = p2 / m + 1;
                            y2 = p2 % m + 1;
                            Tuple<bool, List<Tuple<int, int>>> checkpath = HasPath(x1, y1, x2, y2);
                            if (checkpath.Item1)
                                return new Tuple<bool, int, int, int, int>(true, x1, y1, x2, y2);
                        }
            }
            return new Tuple<bool, int, int, int, int>(false, -1, -1, -1, -1);
        }

        /*
        return Tuple<bool, List<Tuple<int, int>>,bool, bool>
            Item1: Has path or not
            Item2: List of position from (x1, y1) to (x1, y2), return empty list if Item = False
            Item3: true if create new map otherwise false
            Item3: game finish or not
        */
        public Tuple<bool, List<Tuple<int, int>>, bool, bool> Move(int x1, int y1, int x2, int y2)
        {
            Tuple<bool, List<Tuple<int, int>>> checkPath = HasPath(x1, y1, x2, y2);
            if (checkPath.Item1)
                Update();
            switch (level)
            {
                case 1:
                    if (checkPath.Item1) {
                        a[x1, y1] = -1;
                        a[x2, y2] = -1;
                    }
                    break;
                case 2:
                    if (checkPath.Item1) {
                        a[x1, y1] = -1;
                        a[x2, y2] = -1;
                    }
                    break;
                case 3:
                    if (checkPath.Item1)
                    {
                        MoveUp(x1, y1, x2, y2);
                    }
                    break;
                case 4:
                    if (checkPath.Item1)
                    {
                        MoveUp(x1, y1, x2, y2);
                    }
                    break;
                case 5:
                    if (checkPath.Item1)
                    {
                        MoveDown(x1, y1, x2, y2);
                    }
                    break;
                case 6:
                    if (checkPath.Item1)
                    {
                        MoveDown(x1, y1, x2, y2);
                    }
                    break;
                case 7:
                    if (checkPath.Item1)
                    {
                        MoveLeft(x1, y1, x2, y2);
                    }
                    break;
                case 8:
                    if (checkPath.Item1)
                    {
                        MoveLeft(x1, y1, x2, y2);
                    }
                    break;
                case 9:
                    if (checkPath.Item1)
                    {
                        MoveRight(x1, y1, x2, y2);
                    }
                    break;
                case 10:
                    if (checkPath.Item1)
                    {
                        MoveRight(x1, y1, x2, y2);
                    }
                    break;
                case 11:
                    if (checkPath.Item1)
                    {
                        MoveCenterUpDown(x1, y1, x2, y2);
                    }
                    break;
                case 12:
                    if (checkPath.Item1)
                    {
                        MoveCenterUpDown(x1, y1, x2, y2);
                    }
                    break;
                case 13:
                    if (checkPath.Item1)
                    {
                        MoveCenterLeftRight(x1, y1, x2, y2);
                    }
                    break;
                case 14:
                    if (checkPath.Item1)
                    {
                        MoveCenterLeftRight(x1, y1, x2, y2);
                    }
                    break;
            }
            bool createNewMap = false;
            if (IsFinish())
            {
                return new Tuple<bool, List<Tuple<int, int>>, bool, bool>(checkPath.Item1, checkPath.Item2, createNewMap, true);
            }
            if (!(ExistPair().Item1)) {
                ReCreateMap();
                createNewMap = true;
            }
            return new Tuple<bool, List<Tuple<int, int>>, bool, bool>(checkPath.Item1, checkPath.Item2, createNewMap,false);
        }


        /* 
        Checking path from (x1, y1) to (x2, y2)
        return Tuple<bool, List<Tuple<int, int>>>
                Item1: Has path or not
                Item2: List of position from (x1, y1) to (x1, y2), return empty list if Item = False
        */
        public Tuple<bool, List<Tuple<int, int>>> HasPath(int x1, int y1, int x2, int y2)
        {

            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            if (a[x1, y1] != a[x2, y2])
                return new Tuple<bool, List<Tuple<int, int>>>(false, path);
            Dictionary<Tuple<int, int, int, int>, Tuple<int, int, int, int>> trace = new Dictionary<Tuple<int, int, int, int>, Tuple<int, int, int, int>>();
            int[] dx = new int[] { 0, -1, 0, 1 };
            int[] dy = new int[] { -1, 0, 1, 0 };
            int[,,,] flag = new int[n + 2, m + 2, 3, 4];
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                    if (a[i, j] != -1)
                        for (int k = 0; k < 4; k++)
                        {
                            flag[i, j, 0, k] = 1;
                            flag[i, j, 1, k] = 1;
                            flag[i, j, 2, k] = 1;
                        }
            for (int k = 0; k < 4; k++)
            {
                flag[x2, y2, 0, k] = 0;
                flag[x2, y2, 1, k] = 0;
                flag[x2, y2, 2, k] = 0;
            }
            Queue q = new Queue();
            for (int i = 0; i < 4; i++)
            {
                int x = x1 + dx[i];
                int y = y1 + dy[i];
                if (x >= 0 && x <= n + 1 && y >= 0 && y <= m + 1 && flag[x, y, 0, i] == 0)
                {
                    q.Enqueue(new Point(x, y, 0, i));
                    trace.Add(new Tuple<int, int, int, int>(x, y, 0, i), new Tuple<int, int, int, int>(x1, y1, 0, i));
                }
            }
            while (q.Count != 0)
            {
                Point u = (Point)q.Dequeue();
                if (u.x == x2 && u.y == y2)
                {
                    int x = u.x;
                    int y = u.y;
                    Tuple<int, int, int, int> key = new Tuple<int, int, int, int>(x, y, u.n_d, u.d);
                    path.Add(new Tuple<int, int>(key.Item1, key.Item2));
                    while (trace.ContainsKey(key))
                    {
                        key = trace[key];
                        path.Add(new Tuple<int, int>(key.Item1, key.Item2));
                    }
                    return new Tuple<bool, List<Tuple<int, int>>>(true, path);
                }
                for (int i = 0; i < 4; i++)
                {
                    int x = u.x + dx[i];
                    int y = u.y + dy[i];
                    if (x >= 0 && x <= n + 1 && y >= 0 && y <= m + 1)
                    {
                        if (u.d == i && flag[x, y, u.n_d, i] == 0)
                        {
                            flag[x, y, u.n_d, u.d] = 1;
                            q.Enqueue(new Point(x, y, u.n_d, u.d));
                            trace.Add(new Tuple<int, int, int, int>(x, y, u.n_d, i), new Tuple<int, int, int, int>(u.x, u.y, u.n_d, u.d));
                        }
                        else if (u.n_d + 1 <= 2 && flag[x, y, u.n_d + 1, i] == 0)
                        {
                            flag[x, y, u.n_d + 1, i] = 1;
                            q.Enqueue(new Point(x, y, u.n_d + 1, i));
                            trace.Add(new Tuple<int, int, int, int>(x, y, u.n_d + 1, i), new Tuple<int, int, int, int>(u.x, u.y, u.n_d, u.d));
                        }
                    }
                }

            }

            return new Tuple<bool, List<Tuple<int, int>>>(false, path);
        }

        public bool MoveDown(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMinX(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            for (int i = x1; i >= 1; i--)
            {
                a[i, y1] = a[i - 1, y1];
            }
            for (int i = x2; i >= 1; i--)
            {
                a[i, y2] = a[i - 1, y2];
            }
            return IsFinish();
        }

        public bool MoveUp(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMaxX(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            for (int i = x1; i <= n; i++)
            {
                a[i, y1] = a[i + 1, y1];
            }
            for (int i = x2; i <= n; i++)
            {
                a[i, y2] = a[i + 1, y2];
            }
            return IsFinish();
        }

        public bool MoveLeft(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMaxY(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            for (int j = y1; j <= m; j++)
            {
                a[x1, j] = a[x1, j + 1];
            }
            for (int j = y2; j <= m; j++)
            {
                a[x2, j] = a[x2, j + 1];
            }
            return IsFinish();
        }

        public bool MoveRight(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMinY(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            for (int j = y1; j >= 1; j--)
            {
                a[x1, j] = a[x1, j - 1];
            }
            for (int j = y2; j >= 1; j--)
            {
                a[x2, j] = a[x2, j - 1];
            }
            return IsFinish();
        }


        public bool MoveCenterUpDown(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMinX(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            int mid = (int)n / 2;
            if (x1 > mid)
            {
                MoveUp(x1, y1, x2, y2);
            }
            else if (x2 <= mid)
            {
                MoveDown(x1, y1, x2, y2);
            }
            else
            {
                MoveDown(x1, y1, n + 1, 0);
                MoveUp(x2, y2, 0, 0);
            }
            return IsFinish();
        }

        public bool MoveCenterLeftRight(int x1, int y1, int x2, int y2)
        {
            Tuple<int, int, int, int> tmp = SwapMinY(x1, y1, x2, y2);
            x1 = tmp.Item1;
            y1 = tmp.Item2;
            x2 = tmp.Item3;
            y2 = tmp.Item4;
            int mid = (int)m / 2;
            if (y1 > mid)
            {
                MoveLeft(x1, y1, x2, y2);
            }
            else if (y2 <= mid)
            {
                MoveRight(x1, y1, x2, y2);
            }
            else
            {
                MoveRight(x1, y1, 0, m + 1);
                MoveLeft(x2, y2, 0, 0);
            }
            return IsFinish();
        }

        public bool IsFinish()
        {
            if (not == 0)
                return true;
            return false;
        }

        public void LoadMap(string filename)
        {

            StreamReader dataStream = new StreamReader(filename);
            string datasample;
            int row = 1;
            while ((datasample = dataStream.ReadLine()) != null)
            {
                string[] line = datasample.Trim().Split('\t');
                for (int i = 0; i < line.Length; i++)
                    a[row, i + 1] = Convert.ToInt32(line[i]);
                row += 1;

            }
        }

        public void ShowMap()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    Console.Write(a[i, j]);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
        }

        public static void Main()
        {
            PikachuMap p = new PikachuMap(9, 16, 30, 3);
            p.SetLevel(1);
            p.CreateMap();
            Console.WriteLine("Level 1 map");
            p.ShowMap();
            p.SetLevel(2);
            p.CreateMap();
            Console.WriteLine("Level 2 map");
            p.ShowMap();
            // p.Move(1, 11, 1, 13);
            // Console.WriteLine("New map");
            // p.ShowMap();
            // Tuple<bool, int, int, int, int> hint = p.Hint();
            // Console.WriteLine("ExistPair {0}", p.ExistPair().Item1);
            // Tuple<bool, List<Tuple<int, int>>, bool> move = p.Move(1, 1, 1, 2);
            // Console.WriteLine(move.Item3);
            // Console.WriteLine("New map");
            // p.ShowMap();

        }

        public int[,] GetMap()
        {
#if UNITY_EDITOR
            for (int i = 0; i < a.GetLength(0); i++)
            {
                string tmp = "";
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    tmp += a[i, j];
                }
                Debug.Log(tmp);
            }
#endif
            return a;
        }
    }
}