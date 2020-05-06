import {Component, OnInit, Inject, Input} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Camera, System} from "../map";

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  public trueState: string[][];
  public proba:  string[][];
  public cameraView:  string[][];
  public cameras: Camera[] = [];
  public isMoving = false;
  public movePossibilities: boolean[][] = [[false, false, false],[false, false, false],[false, false, false]];


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    let params = new HttpParams();
    params = params.append('init', "true");
    http.get<System>(baseUrl + 'pomcp', { params: params }).subscribe(result => {
      this.trueState = result.trueState;
      this.proba = result.distributionView;
      this.cameraView = result.cameraView;
      this.cameras = result.cameras;
      this.movePossibilities = result.movingOptions;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  public updateSystem(dx,dy) {
    this.isMoving = true;
    console.log(dx ,dy);
    let params = new HttpParams();
    params = params.append('dx', dx);
    params = params.append('dy', dy);
    this.http.get<System>(this.baseUrl + "pomcp", { params: params }).subscribe(result => {
      this.trueState = result.trueState;
      this.proba = result.distributionView;
      this.cameraView = result.cameraView;
      this.cameras = result.cameras;
      this.movePossibilities = result.movingOptions;
      this.isMoving = false;
    }, error => console.error(error));
  }



}
