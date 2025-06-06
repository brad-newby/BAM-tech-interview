import { Component, ViewEncapsulation } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ApiService } from './services/api.service';
import { PersonDto } from './Dtos/PersonDto';
import { DutyDto } from './Dtos/DutyDto';
import { NewDutyDto } from './Dtos/NewDutyDto';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {

  searchForm = this.searchFormBuilder.group({
    name: ""
  });

  updateForm = this.dutyFormBuilder.group({
    rank: "",
    dutyTitle: "",
    dutyStartDate: null,
  });

  personInfo = null;

  showPeopleList = false;

  detailColumns: string[] = ['currentRank', 'currentDutyTitle', 'careerStartDate', 'careerEndDate'];
  dutyColumns: string[] = ['id', 'rank', 'dutyTitle', 'dutyStartDate', 'dutyEndDate'];
  peopleListColumns: string[] = ['personId','name','currentRank','currentDutyTitle','careerStartDate','careerEndDate']
  detailSource: PersonDto[] = [];
  dutySource: DutyDto[] = [];
  peopleListSource: PersonDto[] = [];

  constructor(
    private searchFormBuilder: FormBuilder,
    private dutyFormBuilder: FormBuilder,
    private api: ApiService,
    private snackBar: MatSnackBar
  ){}

  onSubmitSearch(): void {
    this.showPeopleList = false;
    this.personInfo = null;
    this.detailSource = [];
    this.dutySource = [];
    let name = this.searchForm.value.name?.toString();
    name = encodeURIComponent(name!);

    this.snackBar.open(`Fetching ${this.searchForm.value.name!}'s information...`, undefined, {duration: 2000});



    this.api.GetAstronautInfo(name).subscribe(data => {
      console.log(data);
      if (data.responseCode == 200 && data.person != null)
      {
        //We can use this to dismiss it but it'll be instant do to the scale of app.
        // this.snackBar.dismiss();
        this.personInfo = data.person;
        this.dutySource = data.astronautDuties;
        this.dutySource.sort((a,b) => {return b.id - a.id});
        this.detailSource.push(data.person); 
      } else {
        this.snackBar.open(`${name} does not exist...`, 'close', {panelClass: 'error-snackbar'});
      }

    }, error => {
      this.snackBar.open(`Error when fetching information: ${error.message}`, 'close', {panelClass: 'error-snackbar'});
    });
  
    this.searchForm.reset();
  }

  onSubmitUpdate(): void {
    this.showPeopleList = false;
    this.snackBar.open(`Updating ${this.personInfo!["name"]}'s information...`, undefined, {duration: 2000});
    const newDuty: NewDutyDto = {
      dutyStartDate: this.updateForm.value.dutyStartDate!,
      dutyTitle: this.updateForm.value.dutyTitle!,
      rank: this.updateForm.value.rank!,
      name: this.personInfo!["name"],
    };
    this.api.UpdateAstronautDuty(newDuty).subscribe(data => {
      if (data.responseCode == 200)
      {
        this.snackBar.open(`${this.personInfo!["name"]}'s information has been updated!`, undefined, {duration: 2000, panelClass: 'success-snackbar'});
        this.api.GetAstronautInfo(this.personInfo!["name"]).subscribe(data => {
          this.personInfo = data.person;
          this.dutySource = data.astronautDuties;
          this.dutySource.sort((a,b) => {return b.id - a.id});
          this.detailSource = [];
          this.detailSource.push(data.person);
        }, error => {
          this.snackBar.open(`Error when fetching information: ${error.message}`, 'close', {panelClass: 'error-snackbar'});
        });
      }
    }, error => {
      this.snackBar.open(`Error when fetching information: ${error.message}`, 'close', {panelClass: 'error-snackbar'});
    });
    this.updateForm.reset();
  }

  onSubmitAddPerson(): void{
    this.showPeopleList = false;
    let name = this.searchForm.value.name?.toString();
    console.log(name);
    this.api.CreateNewPerson(name!).subscribe(data => {
      if(data.responseCode == 200)
      {
        this.snackBar.open(`New personel record created!`, undefined, {duration: 2000, panelClass: 'success-snackbar'});
      }
    }, error => {
      this.snackBar.open(`Error when creating record: ${error.message}`, 'close', {panelClass: 'error-snackbar'});
    });
    this.searchForm.reset();
  }

  onSubmitList(): void{
    this.personInfo = null;
    this.peopleListSource = [];
    this.snackBar.open(`Fetching all personnel information...`, undefined, {duration: 2000});
    this.api.GetAllPeople().subscribe(data => {
      this.showPeopleList = true;
      this.peopleListSource = data.people;
    }, error => {
      this.snackBar.open(`Error fetching personnel data: ${error.message}`, 'close', {panelClass: 'error-snackbar'});
    });
  }
}
