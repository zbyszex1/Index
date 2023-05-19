import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { PaginationService } from './../../services/pagination.service';

@Component({
  selector: 'app-get-persons',
  templateUrl: './get-persons.component.html',
  styleUrls: ['./get-persons.component.css']
})
export class GetPersonsComponent implements OnInit {
  public persons: PersonsDetailed[] | null;
  public alert: AlertService;
  public page: number;
  public pageSize: number;
  public count: number;
  public last: string;
  public pageSizes: number[];
  public curretIndex = -1;

  // public responsive;

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string,
              private alertService: AlertService,
              private paginationService: PaginationService) {
    this.persons = null;
    this.alert = alertService;
    this.page = 1;
    this.count = 0;
    this.last = '';
    this.pageSize = 20;
    this.pageSizes = [20, 50, 100];
    // this.responsive = true;
    this.alert.clear();
    this.paginationService.getPersonsCount()
    .subscribe(
      response => {
        this.count = response;
      },
      error => {
        console.error(error);
      });
  }

  ngOnInit(): void {
    this.retrievePersons();
  }

  retrievePersons(): void {
    const params = this.paginationService.getRequestParams(this.last, this.page, this.pageSize);
    this.paginationService.getPersons(params)
    .subscribe(
      response => {
        this.persons = response;
        this.persons?.forEach(person => {
          person.first = person.first.replace('_', ' ');
        })
        console.log(response);
      },
      error => {
        console.error(error);
      });
  }

  handlePageChange(event: number): void {
    this.page = event;
    this.retrievePersons();
  }

  handlePageSizeChange(event: any): void {
    this.pageSize = event.target.value;
    this.page = 1;
    this.retrievePersons();
  }


  searchLast(): void {
    this.page = 1;
    this.retrievePersons();
  }
}

interface PersonsDetailed {
  id: number;
  last: string;
  first: string;
  userId: number;
  class: string;
  created: string;
  updated: string;
}
