import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-sections',
  templateUrl: './get-sections.component.html',
  styleUrls: ['./get-sections.component.css']
})
export class GetSectionsComponent {
  public sections: SectionsDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string,
        private alertService: AlertService,
        tokenOptionsService: TokenOptionsService) {
    this.sections = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<SectionsDetailed[]>(baseUrl + 'api/sections', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.sections = result;
        this.sections.forEach(element => {
          const i = element.pdf.lastIndexOf('/');
          if (i>0) {
            element.pdf = element.pdf.substring(i);
          }
        });
        
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
}
}

interface SectionsDetailed {
  id: number;
  routeId: number;
  name: string;
  thumbs: string;
  pages: string;
  pdf: string;
  header: string;
  description: string;
  offset: number;
  min: number;
  max: number;
  created: string;
  updated: string;
}
