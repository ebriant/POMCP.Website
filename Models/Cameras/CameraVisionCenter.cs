using System;
using POMCP.Website.Models.Environment;


namespace POMCP.Website.Models.Cameras
{
    public class CameraVisionCenter : CameraVision
    {
        
        public CameraVisionCenter(Map map) : base(map)
        {
        }

        /// <summary>
        /// Method that return the visible cells from the position of the camera
        /// </summary>
        /// <param name="xCam"></param>
        /// <param name="yCam"></param>
        /// <returns> 2D array of booleans: true if the cell is visible, false either</returns>
        public override bool[,] GetVisible(int xCam, int yCam)
        {
            bool[,] result = new bool[map.Dx, map.Dy];
            // For all cases in the map
            for (int i = 0; i < map.Dx; i++)
            {
                for (int j = 0; j < map.Dy; j++)
                {
                    // If the cell is the camera, it is visible
                    if (xCam == i && yCam == j)
                        result[i, j] = true;
                    else
                        // Else check if it is visible
                        result[i, j] = IsVisible(xCam, yCam, i, j);
                }
            }
            return result;
        }
        
        //
        /// <summary>
        ///  Check if a cell (i,j) would be visible from the position of the camera (xCam, yCam).
        /// Creates a ray that goes from the camera to the target cell, and check for each cell in between
        /// if they are opaque
        /// </summary>
        /// <param name="xCam"></param>
        /// <param name="yCam"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>True if the cell is visible</returns>
        private bool IsVisible(int xCam, int yCam, int i, int j)
        {
            // amplitude of one step
            double maxi = Math.Max(Math.Abs(xCam - i), Math.Abs(yCam - j));
            
            // Get step size
            double dx = (i - xCam) / maxi;
            double dy = (j - yCam) / maxi;

            // Ray starts from the center of the camera
            double tmpX = xCam + 0.5;
            double tmpY = yCam + 0.5;

            // Follow the ray
            while (true)
            {
                // Check if the view is blocked on (tmpX,tmpY)
                if (IsViewBlocked(tmpX, tmpY))
                    return false;
                
                // Go forward in the ray
                tmpX += dx;
                tmpY += dy;
                
                // Stop when the ray reach the target cell
                if ((int) tmpX == i & (int) tmpY == j)
                    return true;
            }
        }
        
        
        /// <summary>
        /// Check if the cell (tmpX, tmpY) is blocked
        /// </summary>
        /// <param name="tmpX"></param>
        /// <param name="tmpY"></param>
        /// <returns></returns>
        private bool IsViewBlocked(double tmpX, double tmpY)
        {
            float TOLERANCE = 0.01f;
            // Get the cell corresponding to (tmpX,tmpY)
            int x = (int) tmpX;
            int y = (int) tmpY;

            // Problem when the point is near the border of two cells (X line)
            if (Math.Abs(x - tmpX) < TOLERANCE)
                // test if the two adjacent cells are both opaque
                return map.GetCell(x, y) is Opaque & map.GetCell(x-1, y) is Opaque;
            
            // Problem when the point is near the border of two cells (Y line)
            if (Math.Abs(y - tmpY) < TOLERANCE)
                // test if the two adjacent cells are both opaque
                return map.GetCell(x, y) is Opaque & map.GetCell(x, y-1) is Opaque;
            // general case
            return map.GetCell(x, y) is Opaque;
        }
    }
}