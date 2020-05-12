import {Component, OnInit, Inject, Input} from '@angular/core';
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

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    let params = new HttpParams();
    params = params.append('init', "true");
    http.get<System>(baseUrl + 'pomcp', {params: params}).subscribe(result => {
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
    let params = new HttpParams();
    params = params.append('dx', dx);
    params = params.append('dy', dy);
    this.http.get<System>(this.baseUrl + "pomcp", {params: params}).subscribe(result => {
      this.map = result.map;
      this.trueState = result.trueState;
      this.probabilities = result.probabilities;
      this.camerasVision = result.camerasVision;
      this.cameras = result.cameras;
      this.movingOptions = result.movingOptions;
      this.isMoving = false;
    }, error => console.error(error));
  }

  isInMap(x,y): boolean{
    return (x > 0 && y > 0 && x<this.map.length-1 && y<this.map[0].length-1
      && !(x == this.trueState[0] && y == this.trueState[1]))
  }

  isCellFree(x,y): boolean {
    let result = this.isInMap(x,y);

    result = result && !(x == this.trueState[0] && y == this.trueState[1]);
    for (let camera of this.cameras) {
      result = result && !(x == camera.x && y == camera.y);
    }

    return result;
  }

  changeCell(coord: number[]) {
    if (this.cellChangeActive && this.isCellFree(coord[0],coord[1])) {
      // this.map[coord[0]][coord[1]] = this.cellChangeType;
      let params = new HttpParams();
      params = params.append('x', String(coord[0]));
      params = params.append('y', String(coord[1]));
      params = params.append('cellType', this.cellChangeType);


      this.http.get<System>(this.baseUrl + "pomcp/modifyCell", {params: params}).subscribe(result => {
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
    }
    else {
      this.cellChangeType = typeString;
      this.cellChangeActive = true;
    }

  }
}
