import {Component, OnInit, Inject, Input, Output, EventEmitter} from '@angular/core';
import {Camera} from "../map";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @Input() cells: string[][];
  @Input() camera: boolean;
  @Input() CamerasList: Camera[] = [];


  @Output() cellChanged = new EventEmitter<number[]>();

  CameraVector: number[][] = [];

  constructor() {
  }

  ngOnInit(): void {
  }

  abscissaArray(): number[] {
    if (typeof(this.cells) == "undefined") {
      return []
    }
    return Array.from(Array(this.cells.length).keys())
  }

  ordinateArray(): number[] {
    if (typeof(this.cells) == "undefined") {
      return []
    }
    return Array.from(Array(this.cells[0].length).keys())
  }

  getCellSize() {
    if (typeof(this.cells) == "undefined") {
      return 0
    }
    return Math.round(320 / this.cells.length);
  }

  IsCellProba(x,y) :boolean {
     return !isNaN(Number(this.cells[x][y]))
  }

  getCellColor(x,y) {
    if (!this.cells) {
      return "#F2F2F2"
    }
    if (!this.IsCellProba(x,y)){
      switch(this.cells[x][y]) {
        case "wall": {
          return "#262626";
        }
        case "glass": {
          return "#ACC8D9";
        }
        case "target": {
          return "#6ED93D";
        }
        case "invisible": {
          return "#727372"
        }
        case "visible": {
          return "#BFBFBF"
        }
        case "camera": {
          return "#F28705"
        }
        default: {
          return "#F2F2F2";
        }
      }
    }
    else {
      return this.getColorProba(Number(this.cells[x][y]));
    }
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


}


