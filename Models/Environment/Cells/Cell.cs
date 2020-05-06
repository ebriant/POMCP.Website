namespace POMCP.Website.Models.Environment.Cells
{
    public class Cell
    {
	    public int X { get;}
	    public int Y { get;}

        public string CellType { get; set; } = "empty";

        public Cell(int x, int y){		
            X = x;
            Y = y;
        }
    }
}