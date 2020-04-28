import {Component, ViewChild, ElementRef, OnInit, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";


@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  private cellSize = 20;
  private xSize = 20;
  private ySize = 20;
  public cells: Cell[][];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    // http.get<number[]>(baseUrl + 'world/dimensions').subscribe(result => {
    //   this.xSize = result[0];
    //   this.ySize = result[1];
    // }, error => console.error(error));

    http.get<Cell[][]>(baseUrl + 'world/cells').subscribe(result => {
      this.cells = result;
      this.xSize = result.length;
      this.ySize = result[0].length;
    }, error => console.error(error));

    // http.get<Cell>(baseUrl + 'world/target').subscribe(result => {
    //   this.ctx.fillStyle = 'green';
    //   this.drawCell(result);
    //
    // }, error => console.error(error));


  }

  ngOnInit(): void {

  }

  abscissaArray(): number[] {
    return Array.from(Array(this.xSize).keys())
  }

  ordinateArray(): number[] {
    return Array.from(Array(this.ySize).keys())
  }

  getCellColor(x,y) {
    if (typeof this.cells === "undefined") {
      return "#F2F2F2";
    }

    switch(this.cells[x][y].cellType) {
      case "wall": {
        return "#737373";
      }
      case "glass": {
        return "#05F2F2";
      }
      case "target": {
        return "#4B7E12";
      }
      default: {
        return "#F2F2F2";
      }
    }
  }
}

interface Cell {
  x:number;
  y:number;
  cellType :string;
}

interface Map {
  dx: number;
  dy: number;
}


