import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';


@Component({
  selector: 'app-get-classes',
  templateUrl: './get-classes.component.html',
  styleUrls: ['./get-classes.component.css']
})
export class GetClassesComponent {

    public classes: Class[] | null;
    public alert: AlertService;
  
    constructor(http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private alertService: AlertService,
      tokenOptionsService: TokenOptionsService) {
      this.classes = null;
      this.alert = alertService;
      this.alert.clear();
      http.get<Class[]>(baseUrl + 'api/classes')
        .subscribe(result => {
          this.classes = result;
        }, error => {
          console.error(error)
          this.alert.error(error.message);
        });
    }
  }
  
  interface Class {
    id: number;
    name: string;
    created: string;
    updated: string;
  }
  