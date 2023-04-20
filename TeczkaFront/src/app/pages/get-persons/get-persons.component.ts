import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-persons',
  templateUrl: './get-persons.component.html',
  styleUrls: ['./get-persons.component.css']
})
export class GetPersonsComponent {
  public persons: PersonsDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string,
              private alertService: AlertService,
              tokenOptionsService: TokenOptionsService) {
    this.persons = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<PersonsDetailed[]>(baseUrl + 'api/persons', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.persons = result;
        this.persons.forEach(element => {
        });
        
      }, error => {
        console.error(error);
        this.alert.error(error.message);
        });
  }
}

interface PersonsDetailed {
  id: number;
  last: string;
  first: string;
  created: string;
  updated: string;
}
