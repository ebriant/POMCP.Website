using System;
using System.Collections.Generic;
using System.Drawing;
using POMCP.Website.Models.Pomcp;

namespace POMCP.Website.Models.Cameras
{
    public class AngularCamera : Camera
    {
        public double FOV = Math.PI / 8;

		private const int PositionsCount = 8;
	
	
	public AngularCamera(int x, int y, int number, CameraVision vision) :base(x,y,number, vision){
		
	}

	/**
	 * retourne le champ de vision de la camera utilise l'angle pour determiner
	 * le champs
	 * 
	 * @return vision avec angle
	 */
	public override bool[,] GetVision(double angle) {
		
		// CasesVisibles vuAngle = new CasesVisibles(visible);

		bool[,] result = new bool[VisibleCells.GetLength(0),VisibleCells.GetLength(1)];
		// Vector representing the direction of the camera
		double[] cameraVewVector = { Math.Cos(angle),
				Math.Sin(angle) };
		// ecart autorisé sur le cosinus
		double ecartCosinus = Math.Cos(FOV);

		// for each cell
		for (int i = 0; i < result.GetLength(0); i++)
			for (int j = 0; j < result.GetLength(1); j++) {
				// get the vector camera -> cell
				double dx = i - X;
				double dy = j - Y;
				double norm = Math.Sqrt(dx * dx + dy * dy);

				if (norm != 0) {
					// get the cosinus
					double cos = (dx * cameraVewVector[0] + dy
							* cameraVewVector[1])
							/ norm;
					// si le cosinus reel est plus petit, non visible
					if (cos < ecartCosinus)
						result[i,j] = false;

				}
			}
		return result;
	}

	/**
	 * permet de changer l'angle de la camera
	 */
	/*
	public void changerAngle(double deltaAngle) {
		angle += deltaAngle;
	}
	
	public void setAngleDirect(double angle) {
		angle = angle;
	}

	public double getAngle() {
		return angle;
	}

	public void setAngle(double angle) {
		angle = angle;
	}
	*/
	
	
	public override Distribution<Observation> GetObservation(State s){
		Distribution<Observation> o = new Distribution<Observation>();
		if (GetVision(s.CamerasOrientations[Num])[s.X,s.Y])
			o.setProba(new Observation(true, s.X, s.Y),1);
		else 
			o.setProba(new Observation(false),0);
		return o;
	}

	
	public override List<double> GetActions() {
		List<double> L = new List<double>();
		for (int i = -2 ; i<3; i++) {
			L.Add(Math.PI*i/4);
		}
		return L;
	}
    }
}