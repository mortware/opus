import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';

import { CoreModule } from './core/core.module';

import { AppComponent } from './app.component';
import { JobModule } from './job/job.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CoreModule,


    JobModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
