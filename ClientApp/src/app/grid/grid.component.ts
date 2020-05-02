import {Component, OnInit, Inject, Input} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Cell} from "../map";

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @Input() cells: Cell[][];

  constructor() {
  }

  ngOnInit(): void {

  }

  abscissaArray(): number[] {
    return Array.from(Array(this.cells.length).keys())
  }

  ordinateArray(): number[] {
    return Array.from(Array(this.cells[0].length).keys())
  }

  getCellColor(x,y) {
    if (typeof this.cells === "undefined") {
      return "#F2F2F2";
    }
    switch(this.cells[x][y].cellType) {
      case "wall": {
        return "#262626";
      }
      case "glass": {
        return "#ACC8D9";
      }
      case "target": {
        return "#6ED93D";
      }
      default: {
        return "#F2F2F2";
      }
    }

  }

  /**
   * Return the string corresponding to the color of the probability in css rbg style (example "rgb(110,217,61)")
   * @param p: probability
   */
  public getColorProba(p) {
    let color1 = [110,217,61];
    let color2 = [242,242,242];
    let newColor = [];
    for (let i = 0; i<3; i++) {
      newColor[i] = color1[i] + (color2[i] - color1[i]) * p
    }
    return "rgb(" + newColor[0] + ","  + newColor[1] + "," + newColor[2] + ")"

  }

  hexToRGB(h) {
    let r = 0, g = 0, b = 0;

    // 3 digits
    if (h.length == 4) {
      let r = "0x" + h[1] + h[1];
      let g = "0x" + h[2] + h[2];
      let b = "0x" + h[3] + h[3];

      // 6 digits
    } else if (h.length == 7) {
      let r = "0x" + h[1] + h[2];
      let g = "0x" + h[3] + h[4];
      let b = "0x" + h[5] + h[6];
    }

    return "rgb("+ +r + "," + +g + "," + +b + ")";
  }
}


