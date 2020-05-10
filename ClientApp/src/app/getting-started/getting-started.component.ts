import {Component, OnInit, ViewEncapsulation} from '@angular/core';
import {MESSAGE} from "./message";

@Component({
  selector: 'app-getting-started',
  templateUrl: './getting-started.component.html',
  styleUrls: ['./getting-started.component.css'],
  encapsulation: ViewEncapsulation.None,
})
export class GettingStartedComponent implements OnInit {

  pageCounter = 0;
  message = MESSAGE;

  constructor() {
  }

  ngOnInit() {
  }

  increaseCount(){
    this.pageCounter ++;
  }

  decreaseCount(){
    this.pageCounter --;
  }
}
