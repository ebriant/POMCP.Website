import {Component, OnInit, Inject, Input, Output, EventEmitter} from '@angular/core';
import {Camera} from "../map";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {


  @Input() map: string[][];
  @Input() camerasList: Camera[] = [];
  @Input() trueState: number[];
  @Input() probabilities: number[][];
  @Input() cameraVision: number[][];

  @Input() cellChangeActive: boolean;
  @Output() cellChanged = new EventEmitter<number[]>();

  isClicking = false;

  constructor() {
  }

  ngOnInit(): void {
  }

  cellClick(x,y) {
    this.cellChanged.emit([x,y]);
    this.isClicking = true;
  }

  cellOver(x, y){
    if (this.isClicking) {
      this.cellChanged.emit([x,y]);
    }
  }

  abscissaArray(): number[] {
    if (!this.map) {
      return []
    }
    return Array.from(Array(this.map.length).keys())
  }

  ordinateArray(): number[] {
    if (!this.map) {
      return []
    }
    return Array.from(Array(this.map[0].length).keys())
  }

  getCellSize() {
    if (!this.map) {
      return 0
    }
    return Math.round(340 / this.map.length);
  }

  Round(x: number,n: number): number{
    return Number(x.toFixed(n))
  }

  getCellClass(x:number, y:number) {
    if (!this.map) {
      return "";
    }

    switch(this.map[x][y]) {
      case "wall": {
        return "wall";
      }
      case "glass": {
        return "glass";
      }
    }

    for (let camera of this.camerasList) {
      if (x == camera.x && y == camera.y) {
        return "camera";
      }
    }

    if (this.cameraVision) {
      if (this.cameraVision[x][y] == 0) {
          return "unobservable"
      }
      if (this.cameraVision[x][y] == 1) {
          return "observable"
      }
    }

    if (this.trueState && x == this.trueState[0] && y == this.trueState[1]) {
      return "target";
    }


    return "";
  }

  getCellColor(x,y) {
    if (!this.probabilities || !this.probabilities[x][y]) {
      return ""
    }
    return this.getColorProba(this.probabilities[x][y]);

  }

  /**
   * Return the string corresponding to the color of the probability in css rbg style (example "rgb(110,217,61)")
   * @param p: probability
   */
  public getColorProba(p) {
    let color1 = [242,242,242];
    let color2 = [110,217,61];
    let newColor = [];
    for (let i = 0; i<3; i++) {
      newColor[i] = color2[i] + (color1[i] - color2[i]) * (1-p)**2;
    }
    return "rgb(" + newColor[0] + ","  + newColor[1] + "," + newColor[2] + ")"

  }

  getVectorX(angle: number) {
    return Math.round((1000) * Math.cos(angle));
  }

  getVectorY(angle: number) {
    return Math.round((1000) * Math.sin(angle));
  }


  hasProbability(x,y): boolean {
    if (!this.probabilities) {
      return false;
    }
    return (x > 0 && y > 0 && x < this.probabilities.length - 1 && y < this.probabilities[0].length - 1);
  }

}


