import { Component, OnInit } from '@angular/core';
import { CreateJobRequest, WheelPosition, IJobItem, TyreReplacement } from '../models';
import { FormGroup, FormControl, Validators, FormBuilder, FormArray } from '@angular/forms';
import { ApiService } from 'src/app/core';

@Component({
  selector: 'app-create-job',
  templateUrl: './create-job.component.html',
  styleUrls: ['./create-job.component.css']
})
export class CreateJobComponent implements OnInit {

  newSheet: CreateJobRequest;
  jobSheetForm: FormGroup;
  itemForm: FormGroup;

  wheelPositions: string[];



  constructor(private formBuilder: FormBuilder, private apiService: ApiService) {
    this.jobSheetForm = this.formBuilder.group({
      referenceLabourInMinutes: new FormControl('', Validators.required),
      referencePrice: new FormControl('', Validators.required),
      items: this.formBuilder.array([this.createItem()])
    });

    let wheelPositions = Object.keys(WheelPosition);
    this.wheelPositions = wheelPositions.slice(wheelPositions.length / 2);
  }

  ngOnInit(): void {
    this.clearForm();
  }

  private createForm(referenceLabourInMinutes?: number, referencePrice?: number): void {

    this.jobSheetForm = this.formBuilder.group({
      referenceLabourInMinutes: new FormControl(referenceLabourInMinutes || 0, Validators.required),
      referencePrice: new FormControl(referencePrice || 0, Validators.required),
      items: this.formBuilder.array([])
    });

    this.itemForm = this.formBuilder.group({
      $type: new FormControl('', Validators.required),
      position: new FormControl('', Validators.required)
    });
  }

  clearForm(): void {
    this.createForm();
  }

  addItem(item: any): void {
    this.items.push(this.createItem(item));
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  onSubmit(form: any): void {
    console.log(this.jobSheetForm.value);

    this.apiService.post('/job/create', this.jobSheetForm.value).subscribe(result => {
      console.log(result);
    },
    error => {

    });
  }

  get items(): FormArray {
    return this.jobSheetForm.get("items") as FormArray;
  }

  private createItem(item?: TyreReplacement): FormGroup {
    let formItem = this.formBuilder.group({
      $type: new FormControl('TyreReplacement'),
      position: new FormControl('NearsideFront')
    });

    if (!item)
      return formItem;

    formItem.setValue({
      $type: item.$type,
      position: WheelPosition.NearsideFront
    });

    return formItem;
  }

  generateRandomSheet(): void {
    this.createForm(60,10)
  }

  addRandomItem(): void {
    this.items.push(this.createItem());
  }
}
