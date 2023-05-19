import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { PaginationService } from '@app/services/pagination.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-scans',
  templateUrl: './get-scans.component.html',
  styleUrls: ['./get-scans.component.css']
})
export class GetScansComponent {
  public scans: ScansDetailed[] | null;
  public alert: AlertService;
  public page: number;
  public pageSize: number;
  public count: number;
  public last: string;
  public pageSizes: number[];
  public curretIndex = -1;

  constructor(http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private alertService: AlertService,
      tokenOptionsService: TokenOptionsService,
      private paginationService: PaginationService) {
    this.scans = null;
    this.alert = alertService;
    this.page = 1;
    this.count = 0;
    this.last = '';
    this.pageSize = 50;
    this.pageSizes = [50, 100, 200, 500];
    // this.responsive = true;
    this.alert.clear();
    this.paginationService.getScansCount()
    .subscribe(
      response => {
        this.count = response;
        console.log(response)
      },
      error => {
        this.alert.error(error.message);
        console.error(error);
      });
  }

  ngOnInit(): void {
    this.retrieveScans();
  }

  retrieveScans(): void {
    const params = this.paginationService.getRequestParams(this.last, this.page, this.pageSize);
    this.paginationService.getScans(params)
    .subscribe(
      response => {
        this.scans = response;
        console.log(response);
      },
      error => {
        this.alert.error(error.message);
        console.error(error);
      });
  }

  handlePageChange(event: number): void {
    this.page = event;
    this.retrieveScans();
  }

  handlePageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.page = 1;
    this.retrieveScans();
  }

}

interface ScansDetailed {
  id: number;
  sectionId: number;
  section: string;
  userId: number;
  page: number;
  created: string;
  updated: string;
}
