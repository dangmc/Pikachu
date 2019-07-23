using System;
using System.Collections; 
using System.Collections.Generic;
using System.IO;

public class Point 
{
	public int x, y, n_d, d;
	
	public Point(int x_, int y_, int n_d_, int d_) {
		x = x_;
		y = y_;
		n_d = n_d_;
		d = d_;
	}
}

public class PikachuMap
{
	int[,] a;
	int n, m;
	int noc; // number of characters

	public PikachuMap(int n_, int m_, int noc_) {
		n = n_;
		m = m_;
		noc = noc_;
		a = new int[n + 2, m + 2];
	}

	public void CreateMap() {
		List<int> map = new List<int>();
		Random r1 = new Random(1);
		Random r2 = new Random(100);
		Random r3 = new Random(50);
		for (int i = 0; i < n; i++)
			for (int j = 0; j < m; j++) 
				map.Add(i * m + j);
		int last = n*m;
		for (int i = 0; i < last/2; i++) {
			int id = r1.Next(noc);
			int p = r2.Next(last);
			a[map[p]/m + 1, map[p]%m + 1] = id;
			int tmp = map[p];
			map[p] = map[last - 1];
			map[last - 1] = tmp;
			last -= 1;

			for (int k = 0; k < last; k++) {
				int id1 = r3.Next(last);
				int id2 = r3.Next(last);
				tmp = map[id1];
				map[id1] = map[id2];
				map[id2] = tmp;
			}

			p = r2.Next(last);
			a[map[p]/m + 1, map[p]%m + 1] = id;
			tmp = map[p];
			map[p] = map[last - 1];
			map[last - 1] = tmp;
			last -= 1;

		}

	}


	public Tuple<int, int, int, int> SwapMaxX(int x1, int y1, int x2, int y2) {
		if (x1 < x2)
			return new Tuple<int, int, int, int> (x2, y2, x1, y1);
		else
			return new Tuple<int, int, int, int> (x1, y1, x2, y2);
	}

	public Tuple<int, int, int, int> SwapMinX(int x1, int y1, int x2, int y2) {
		if (x1 > x2)
			return new Tuple<int, int, int, int> (x2, y2, x1, y1);
		else
			return new Tuple<int, int, int, int> (x1, y1, x2, y2);
	}

	public Tuple<int, int, int, int> SwapMinY(int x1, int y1, int x2, int y2) {
		if (y1 > y2)
			return new Tuple<int, int, int, int> (x2, y2, x1, y1);
		else
			return new Tuple<int, int, int, int> (x1, y1, x2, y2);
	}

	public Tuple<int, int, int, int> SwapMaxY(int x1, int y1, int x2, int y2) {
		if (y1 < y2)
			return new Tuple<int, int, int, int> (x2, y2, x1, y1);
		else
			return new Tuple<int, int, int, int> (x1, y1, x2, y2);
	}


	/* 
	Checking path from (x1, y1) to (x2, y2)
	return Tuple<bool, List<Tuple<int, int>>>
			Item1: Has path or not
			Item2: List of position from (x1, y1) to (x1, y2), return empty list if Item = False
	*/
	public Tuple<bool, List<Tuple<int, int>>> HasPath(int x1, int y1, int x2, int y2) {

		List<Tuple<int, int>> path = new List<Tuple<int, int>>();
		if (a[x1, y1] != a[x2, y2])
			return new Tuple<bool, List<Tuple<int, int>>> (false, path);
		Dictionary<Tuple<int, int>, Tuple<int, int>> trace = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
		int[] dx = new int[] {0, -1, 0, 1};
		int[] dy = new int[] {-1, 0, 1, 0};
		int[,] flag = new int[n + 2, m + 2];
		for (int i = 1; i <= n; i++)
			for (int j = 1; j <= m; j++)
				if (a[i, j] != - 1)
					flag[i, j] = 1;
		flag[x2, y2] = 0;
		Queue q = new Queue();
		for (int i = 0; i < 4; i++) {
			int x = x1 + dx[i];
			int y = y1 + dy[i];
			if (x >= 0 && x <= n && y >= 0 && y <= m && flag[x, y] == 0) {
				q.Enqueue(new Point(x, y, 0, i));
				trace.Add(new Tuple<int, int>(x, y), new Tuple<int, int>(x1, y1));
			}
		}
		while (q.Count != 0) {
			Point u = (Point) q.Dequeue();
			flag[u.x, u.y] = 1;
			if (u.x == x2 && u.y == y2) {
				int x = u.x;
				int y = u.y;
				Tuple<int, int> key = new Tuple<int ,int> (x, y);
				path.Add(key);
				while (trace.ContainsKey(key)) {
					key = trace[key];
					path.Add(key);
				}
				return new Tuple<bool, List<Tuple<int, int>>> (true, path);
			}
			for (int i = 0; i < 4; i++) {
				int x = u.x + dx[i];
				int y = u.y + dy[i];
				if (x >= 0 && x <= n && y >= 0 && y <= m && flag[x, y] == 0) { 
					if (u.d == i) {
						q.Enqueue(new Point(x, y, u.n_d, u.d));
						trace.Add(new Tuple<int, int>(x, y), new Tuple<int, int>(u.x, u.y));
					}
					else if (u.n_d + 1 <= 2) {
						q.Enqueue(new Point(x, y, u.n_d + 1, i));
						trace.Add(new Tuple<int, int>(x, y), new Tuple<int, int>(u.x, u.y));
					}
				}
			}

		}

		return new Tuple<bool, List<Tuple<int, int>>> (false, path);
	}
	
	public bool MoveDown(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMinX(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		for (int i = x1; i >= 1; i--) {
			a[i, y1] = a[i-1, y1];
		}
		for (int i = x2; i >= 1; i--) {
			a[i, y2] = a[i-1, y2];
		}
		return IsFinish();
	}

	public bool MoveUp(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMaxX(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		for (int i = x1; i <= n; i++) {
			a[i, y1] = a[i+1, y1];
		}
		for (int i = x2; i <= n; i++) {
			a[i, y2] = a[i+1, y2];
		}
		return IsFinish();
	}

	public bool MoveLeft(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMaxY(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		for (int j = y1; j <= m; j++) {
			a[x1, j] = a[x1, j + 1];
		}
		for (int j = y2; j <= m; j++) {
			a[x2, j] = a[x2, j + 1];
		}
		return IsFinish();
	}

	public bool MoveRight(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMinY(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		for (int j = y1; j >= 1; j--) {
			a[x1, j] = a[x1, j - 1];
		}
		for (int j = y2; j >= 1; j--) {
			a[x2, j] = a[x2, j - 1];
		}
		return IsFinish();
	}


	public bool MoveCenterUpDown(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMinX(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		int mid = (int) n / 2;
		if (x1 > mid) {
			MoveUp(x1, y1, x2, y2);
		} else if (x2 <= mid){
			MoveDown(x1, y1, x2, y2);
		} else {
			MoveDown(x1, y1, n + 1, 0);
			MoveUp(x2, y2, 0, 0);
		} 
		return IsFinish();
	}

	public bool MoveCenterLeftRight(int x1, int y1, int x2, int y2) {
		Tuple<int, int, int, int> tmp = SwapMinY(x1, y1, x2, y2);
		x1 = tmp.Item1;
		y1 = tmp.Item2;
		x2 = tmp.Item3;
		y2 = tmp.Item4;
		int mid = (int) m / 2;
		if (y1 > mid) {
			MoveLeft(x1, y1, x2, y2);
		} else if (y2 <= mid){
			MoveRight(x1, y1, x2, y2);
		} else {
			MoveRight(x1, y1, 0, m + 1);
			MoveLeft(x2, y2, 0, 0);
		} 
		return IsFinish();
	}
	public bool IsFinish() {
		for (int i = 1; i <= n; i++)
			for (int j = 1; j <= m; j++)
				if (a[i, j] != -1)
					return false;
		return true;
	}

	public void LoadMap(string filename) {
  
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
		for (int i = 0; i <= n+1; i++) {
			a[i, 0] = -1;
			a[i, m+1] = -1;
		}
		for (int j = 0; j <= m+1; j++) {
			a[0, j] = -1;
			a[n+1, j] = -1;
		}
	}

	public void ShowMap() {
		for (int i = 1; i <= n; i++) {
			for (int j = 1; j <= m; j++) {
				Console.Write(a[i, j]);
				Console.Write("\t");
			}
			Console.WriteLine();
		}
	}

	public static void Main()
	{
		PikachuMap p = new PikachuMap(9, 16, 30);
		p.LoadMap("map.txt");
		p.ShowMap();
		Tuple<bool, List<Tuple<int, int>>> res = p.HasPath(7, 9, 3, 10);
		Console.WriteLine(res.Item1);
		// foreach (Tuple<int, int> pos in res.Item2) {
		// 	Console.WriteLine(pos);
		// }
		p.MoveCenterUpDown(7, 9, 3, 10);
		p.ShowMap();
		
	}
}
