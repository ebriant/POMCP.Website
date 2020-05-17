import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import {GridComponent} from "./grid/grid.component";
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BoardComponent } from './board/board.component';
import { TooltipDirective } from './tooltip.directive';
import { GettingStartedComponent } from './getting-started/getting-started.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    GridComponent,
    BoardComponent,
    TooltipDirective,
    GettingStartedComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NoopAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
