export interface Camera {
  x:number; // Abscissa of the camera
  y:number; // Ordinate of the camera
  orientation:number; // Orientation of the camera in radiant (between Pi and -Pi)
  fov:number; // fov of the camera in radiant
}

// export interface System {
//   trueState: string[][]
//   distributionView:  string[][]
//   cameraView: string[][]
//   cameras: Camera[]
//   movingOptions: boolean[][]
// }

export interface System {
  // Represent the base map with the walls, glass and empty spaces
  //TODO make it a number code for each element
  map: string[][]

  // Coordinates of the target in the true state of the system
  trueState: number[]

  // Position, orientation and fov of the cameras
  cameras: Camera[]

  // Contains the matrix representing if an empty space is visible or not from the cameras
  // the first boolean is if the cell is potentially visible, and the second is if it is actually observed
  camerasVision: number[][]

  probabilities: number[][]

  movingOptions: boolean[][]
}


