import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';

@Component({
  selector: 'app-fetch-persons',
  templateUrl: './fetch-persons.component.html',
  styleUrls: ['./fetch-persons.component.css']
})
export class FetchPersonsComponent {
  public personas: PersonasDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string,
              private alertService: AlertService) {
    this.personas = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<PersonasDetailed[]>(baseUrl + 'api/test/persons')
      .subscribe(result => {
        this.personas = result;
        // this.personas.forEach(element => {
        // });
        
      }, error => {
        console.error(error);
        this.alert.error(error.message);
        });
  }
}

interface PersonasDetailed {
  id: number;
  last: string;
  first: string;
  created: string;
  updated: string;
}
