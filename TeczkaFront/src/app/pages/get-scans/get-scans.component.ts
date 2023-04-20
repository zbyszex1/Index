import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-scans',
  templateUrl: './get-scans.component.html',
  styleUrls: ['./get-scans.component.css']
})
export class GetScansComponent {
  public scans: ScansDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private alertService: AlertService,
      tokenOptionsService: TokenOptionsService) {
    this.scans = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<ScansDetailed[]>(baseUrl + 'api/scans', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.scans = result;
        this.scans.forEach(element => {
        });
        
      }, error => {
          console.error(error);
          this.alert.error(error.message);
        });
  }
}

interface ScansDetailed {
  id: number;
  sectionId: number;
  userId: number;
  page: number;
  created: string;
  updated: string;
}
