import {Component, Input} from '@angular/core';
import {GettingStartedComponent} from "../getting-started/getting-started.component";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  @Input() gettingStarted: GettingStartedComponent;
  isExpanded = false;
  mapPanel = false;
  parametersPanel = false;

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
}
