import {Component, OnInit, Inject, Input} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import { Cell } from '../map';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})
export class BoardComponent implements OnInit {

  public cells: Cell[][];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Cell[][]>(baseUrl + 'world/cells').subscribe(result => {
      this.cells = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}
