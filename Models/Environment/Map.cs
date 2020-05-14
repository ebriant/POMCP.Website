﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
 using POMCP.Website.Models.Environment.Cells;

 namespace POMCP.Website.Models.Environment
{
    public class Map
    {
	    
	    public int Dx { get; private set; }

		public int Dy { get; private set; }

		public Cell[,] Cells { get; private set; }
	
		public Map(int tx, int ty) {
			Dx = tx;
			Dy = ty;
			Cells = new Cell[Dx,Dy];
			// AddOutsideWalls();
		}

		public void AddObstacle(Obstacle obstacle) {
			if (IsInMap(obstacle.X,obstacle.Y))
				Cells[obstacle.X,obstacle.Y] = obstacle;
		}
		
		
		public Cell GetCell(int x, int y)
		{
			return Cells[x, y];
		}

		/// <summary>
		/// Check if a cell is free (i.e not occupied by an obstacle)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsCellFree(int x, int y) {
			if (IsInMap(x, y)) {
				return !(Cells[x,y] is Obstacle);
			}
			return false;
		}

		/// <summary>
		/// Check if the coordinates are in the map
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsInMap(int x, int y) {
			return x >= 0 && x < Dx && y >= 0 && y < Dy;
		}

		public string[][] GetCellsArray(string defaultString = "")
		{
			string[][] result = new string[Dx][];
			for (int i = 0; i < Dx; i++)
			{
				result[i] = new string[Dy];
				
				for (int j = 0; j < Dy; j++)
				{
					if (Cells[i, j] == null)
					{
						result[i][j] = defaultString;
					}
					else
					{
						result[i][j] = Cells[i,j].CellType;
					}
				}
			}
			return result;
		}


		public void ResizeMap(int dx, int dy)
		{
			Cell[,] result = new Cell[dx,dy];
			for (int i = 0; i < dx; i++)
			{
				for (int j = 0; j < dy; j++)
				{
					if (i < Dx && j < Dy)
						result[i, j] = Cells[i, j];
				}
			}
			Cells = result;
			Dx = dx;
			Dy = dy;
		}
	}
}