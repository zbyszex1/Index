import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { Token } from '@app/classes/user-token';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { first } from 'rxjs/operators';
import { concatMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-get-users',
  templateUrl: './get-users.component.html',
  styleUrls: ['./get-users.component.css']
})

export class GetUsersComponent implements OnInit, OnDestroy {
  public tokenOptionsService: TokenOptionsService;
  public users: UsersDetailed[] | null;
  public alert: AlertService;
  public http: HttpClient;
  public baseUrl: string;

  constructor(
      _http: HttpClient,
      @Inject('BASE_URL') _baseUrl: string,
      private alertService: AlertService,
      _tokenOptionsService: TokenOptionsService) {
    this.tokenOptionsService = _tokenOptionsService;
    this.users = null;
    this.http = _http;
    this.baseUrl = _baseUrl;
    this.alert = alertService;
    this.alert.clear();
  }

  ngOnInit() {
      this.tokenOptionsService.doRefresh(this.http, this.baseUrl)
      .pipe(first())
      .subscribe({
        next: (result: any) => {
          this.getData();
        },
        error: (error: any) => {
          this.alertService.error(error.message);
        }
      });
  };

  ngOnDestroy() {
    this.users = [];
  }
  
  getData() {
    this.http.get<any[]>(this.baseUrl + 'api/users')
    .subscribe(result => {
      this.users = result;
      console.log('getData()');
    },
    error => {
      console.error(error);
      this.alert.error(error.message);
    })
  }
}

interface UsersDetailed {
  id: number;
  name: string;
  email: string;
  phone: string;
  level: number;
  role: string;
  // tempPassword: string;
  // passwordHash: string;
  // created: string;
  // updated: string;
}
