import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { PaginationService } from '@app/services/pagination.service';

@Component({
  selector: 'app-get-indexes',
  templateUrl: './get-indexes.component.html',
  styleUrls: ['./get-indexes.component.css']
})
export class GetIndexesComponent implements OnInit {
  public indexes: IndexesDetailed[] | null;
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
      private paginationService: PaginationService) {
    this.indexes = null;
    this.alert = alertService;
    this.page = 1;
    this.count = 0;
    this.last = '';
    this.pageSize = 50;
    this.pageSizes = [50, 100, 200, 500];
    // this.responsive = true;
    this.alert.clear();
    this.paginationService.getIndexesCount()
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
    this.retrieveIndexes();
  }

  retrieveIndexes(): void {
    const params = this.paginationService.getRequestParams(this.last, this.page, this.pageSize);
    this.paginationService.getIndexes(params)
    .subscribe(
      response => {
        this.indexes = response;
        console.log(response);
      },
      error => {
        this.alert.error(error.message);
        console.error(error);
      });
  }

  handlePageChange(event: number): void {
    this.page = event;
    this.retrieveIndexes();
  }

  handlePageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.page = 1;
    this.retrieveIndexes();
  }

}

interface IndexesDetailed {
  id: number;
  personId: number;
  scanId: number;
  created: string;
  updated: string;
}
