import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {Person } from './../classes/person'

@Injectable({providedIn: 'root'})

export class PaginationService {

  private baseUrl: string;
  constructor(private http: HttpClient,
    @Inject('BASE_URL') _baseUrl: string) {
      this.baseUrl = _baseUrl;
  }

  getIndexesCount(): Observable<any> {
    let params: any = {};
    params['any'] = true;
    return this.http.get<any>(this.baseUrl + 'api/indexes/cnt', { params });
  }

  getIndexes(params: any): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'api/indexes/pg', { params });
  }

  getPersonsCount(): Observable<any> {
    let params: any = {};
    params['any'] = true;
    return this.http.get<any>(this.baseUrl + 'api/persons/cnt', { params });
  }

  getPersons(params: any): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'api/persons/pg', { params });
  }

  getScansCount(): Observable<any> {
    let params: any = {};
    params['any'] = true;
    return this.http.get<any>(this.baseUrl + 'api/scans/cnt', { params });
  }

  getScans(params: any): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'api/scans/pg', { params });
  }

  getRequestParams(searchLast: string, page: number, pageSize: number): any {
    let params: any = {};
    if (searchLast) {
      params[`Filter`] = searchLast;
    }
    if (page) {
      params[`Page`] = page - 1;
    }
    if (pageSize) {
      params[`PageSize`] = pageSize;
    }
    return params;
  }

}
