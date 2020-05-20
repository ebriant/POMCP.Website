import {Component, Inject, Input} from '@angular/core';
import {GettingStartedComponent} from "../getting-started/getting-started.component";
import {BoardComponent} from "../board/board.component";
import {HttpClient, HttpParams} from "@angular/common/http";
import {System} from "../map";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  @Input() gettingStarted: GettingStartedComponent;
  @Input() board: BoardComponent;
  isExpanded = false;
  mapPanel = false;
  parametersPanel = false;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  toggleMap() {
    this.mapPanel = !this.mapPanel;
    this.parametersPanel = false;
  }

  toggleParams() {
    this.parametersPanel = !this.parametersPanel;
    this.mapPanel = false;
  }

  changeWorld(index: number) {
    let params = new HttpParams();
    params = params.append('index', String(index));
    this.http.get<System>(this.baseUrl + "pomcp", {params: params}).subscribe(result => {
      this.board.map = result.map;
      this.board.trueState = result.trueState;
      this.board.probabilities = result.probabilities;
      this.board.camerasVision = result.camerasVision;
      this.board.cameras = result.cameras;
      this.board.movingOptions = result.movingOptions;
      this.board.isMoving = false;
      this.board.mapSizeX = result.map.length;
      this.board.mapSizeY = result.map[0].length;
    }, error => console.error(error));
  }

}
