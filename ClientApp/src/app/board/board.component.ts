import {Component, OnInit, Inject, HostListener} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Camera, System} from "../map";

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {


  public map: string[][];
  public trueState: number[];
  public probabilities: number[][];
  public camerasVision: number[][];
  public cameras: Camera[] = [];
  public movingOptions: boolean[][] = [[false, false, false], [false, false, false], [false, false, false]];

  public isMoving = false;

  public cellChangeType;
  public cellChangeActive = false;

  public mapSizeX =9;
  public mapSizeY =9;

  treeDepth = 3;
  iterations = 500;
  gama = 0.3;
  C = 0.5;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    http.get<System>(baseUrl + 'pomcp',).subscribe(result => {
      this.map = result.map;
      this.trueState = result.trueState;
      this.probabilities = result.probabilities;
      this.camerasVision = result.camerasVision;
      this.cameras = result.cameras;
      this.movingOptions = result.movingOptions;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  public updateSystem(dx, dy) {
    this.isMoving = true;
    this.cellChangeActive = false;
    let params = new HttpParams();
    params = params.append('dx', dx);
    params = params.append('dy', dy);
    params = params.append('treeSamplesCount', String(this.iterations));
    params = params.append('treeDepth', String(this.treeDepth));
    params = params.append('gama', String(this.gama));
    params = params.append('c', String(this.C));
    this.http.get<System>(this.baseUrl + "pomcp/move", {params: params}).subscribe(result => {
      this.map = result.map;
      this.trueState = result.trueState;
      this.probabilities = result.probabilities;
      this.camerasVision = result.camerasVision;
      this.cameras = result.cameras;
      this.movingOptions = result.movingOptions;
      this.isMoving = false;
    }, error => console.error(error));
  }

  isInMap(x, y): boolean {
    return (x > 0 && y > 0 && x < this.map.length - 1 && y < this.map[0].length - 1
      && !(x == this.trueState[0] && y == this.trueState[1]))
  }

  /**
   * Checks if changing the cell type at the given position is allowed
   * @param x
   * @param y
   */
  isChangeAllowed(x, y): boolean {
    if (!this.cellChangeActive || !this.isInMap(x, y))
      return false;
    if (this.cellChangeType == "camera") {
      for (let camera of this.cameras) {
        if (x == camera.x && y == camera.y) {
          console.log("potato");
          return true;
        }
      }
      return this.map[x][y] == "";
    } else if (this.cellChangeType == "target") {
      return this.map[x][y] == "";
    } else {
      if (x == this.trueState[0] && y == this.trueState[1])
        return false;
      for (let camera of this.cameras) {
        if (x == camera.x && y == camera.y)
          return false;
      }
      return true;
    }
  }

  /**
   * Change the type of a cell at a given point
   * @param coord
   */
  changeCell(coord: number[]) {

    console.log(this.cellChangeActive);

    if (this.cellChangeActive && this.isChangeAllowed(coord[0], coord[1])) {
      if (this.cellChangeType == "target") {
        this.cellChangeActive = false;
      }
      let params = new HttpParams();
      params = params.append('x', String(coord[0]));
      params = params.append('y', String(coord[1]));
      params = params.append('cellType', this.cellChangeType);


      this.http.get<System>(this.baseUrl + "pomcp/modify-cell", {params: params}).subscribe(result => {
        this.map = result.map;
        this.trueState = result.trueState;
        this.probabilities = result.probabilities;
        this.camerasVision = result.camerasVision;
        this.cameras = result.cameras;
        this.movingOptions = result.movingOptions;
      }, error => console.error(error));
    }
  }

  setCellChangeType(typeString: string) {
    if (this.cellChangeActive && this.cellChangeType == typeString) {
      this.cellChangeActive = false;
    } else {
      this.cellChangeType = typeString;
      this.cellChangeActive = true;
    }
  }

  setMapSize() {
    let params = new HttpParams();
    params = params.append('dx', String(this.mapSizeX ? this.mapSizeX : this.map.length));
    params = params.append('dy', String(this.mapSizeY ? this.mapSizeY : this.map[0].length));

    this.http.get<System>(this.baseUrl + "pomcp/map-size", {params: params}).subscribe(result => {
      this.map = result.map;
      this.trueState = result.trueState;
      this.probabilities = result.probabilities;
      this.camerasVision = result.camerasVision;
      this.cameras = result.cameras;
      this.movingOptions = result.movingOptions;
    }, error => console.error(error));
  }


  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    console.log(event.key);
    switch (event.key) {
      case "ArrowLeft":
        if (this.movingOptions[0][1]){
          this.updateSystem(-1,0);
        }
        break;
      case "ArrowRight":
        if (this.movingOptions[2][1]){
          this.updateSystem(1,0);
        }
        break;
      case "ArrowUp":
        if (this.movingOptions[1][2]){
          this.updateSystem(0,-1);
        }
        break;
      case "ArrowDown":
        if (this.movingOptions[1][2]){
          this.updateSystem(0,1);
        }
        break;
    }
  }



}
