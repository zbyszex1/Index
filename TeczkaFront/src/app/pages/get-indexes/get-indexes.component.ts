import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-indexes',
  templateUrl: './get-indexes.component.html',
  styleUrls: ['./get-indexes.component.css']
})
export class GetIndexesComponent {
  public indexes: IndexesDetailed[] | null;
  public alert: AlertService;

  constructor(http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private alertService: AlertService,
      tokenOptionsService: TokenOptionsService) {
    this.indexes = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<IndexesDetailed[]>(baseUrl + 'api/indexes', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.indexes = result;
        this.indexes.forEach(element => {
        });
      }, error => {
          console.error(error);
          this.alert.error(error.message);
        });
  }
}

interface IndexesDetailed {
  id: number;
  personId: number;
  scanId: number;
  created: string;
  updated: string;
}
