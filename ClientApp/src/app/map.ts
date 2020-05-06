export interface Camera {
  x:number; // Abscissa of the camera
  y:number; // Ordinate of the camera
  orientation:number; // Orientation of the camera in radiant (between Pi and -Pi)
  fov:number; // fov of the camera in radiant
}

export interface System {
  trueState: string[][]
  distributionView:  string[][]
  cameraView: string[][]
  cameras: Camera[]
  movingOptions: boolean[][]
}
