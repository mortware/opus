import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateJobComponent } from './create-job/create-job.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [CreateJobComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [CreateJobComponent]
})
export class JobModule { }
