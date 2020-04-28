﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;

namespace POMCP.Website.Models.Environment
{
    public class Map
    {
	    
	    public int Dx { get; }

		public int Dy { get; }

		public Cell[,] Cells { get; }
	
		public Map(int tx, int ty) {
			Dx = tx;
			Dy = ty;
			Cells = new Cell[Dx,Dy];
			AddOutsideWalls();
		}
		
		private void AddOutsideWalls()
		{
			for (int i = 0; i < Dx; i++)
			{
				AddObstacle(new Wall(i,0));
				AddObstacle(new Wall(i,Dy-1));
			}
			for (int j = 1; j < Dy-1; j++)
			{
				AddObstacle(new Wall(0,j));
				AddObstacle(new Wall(Dx-1,j));
			}
		}
		
		public void AddObstacle(Obstacle obstacle) {
			Cells[obstacle.X,obstacle.Y] = obstacle;
		}

		public IEnumerable<IEnumerable<Cell>> GetWallsList()
		{
			List<Cell> result = new List<Cell>();
			for (int i = 0; i < Dx; i++)
			{
				for (int j = 0; j < Dy; j++)
				{
					if (Cells[i, j] is Wall)
					{
						result.Add(Cells[i,j]);
					}
				}
			}

			Cell[,] test = new Cell[1,2];
			Cell[][] test2 = new Cell[][] { };
			return test2;
		}

		public Cell[][] GetCellsArray()
		{
			Cell[][] result = new Cell[Dx][];
			for (int i = 0; i < Dx; i++)
			{
				result[i] = new Cell[Dy];
				
				for (int j = 0; j < Dy; j++)
				{
					if (Cells[i, j] == null)
					{
						result[i][j] = new Cell(i,j);
					}
					else
					{
						result[i][j] = Cells[i,j];
					}
				}
			}

			return result;
		}
	}
}