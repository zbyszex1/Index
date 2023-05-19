import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';

@Component({
  selector: 'app-server',
  templateUrl: './server.component.html',
  styleUrls: ['./server.component.css']
})
export class ServerComponent {
  public sections: ServerDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string,
        private alertService: AlertService) {
    this.sections = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<ServerDetailed[]>(baseUrl + 'api/server')
      .subscribe(result => {
        this.sections = result;
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
  }
}

interface ServerDetailed {
  name: string;
  value: string;
}
