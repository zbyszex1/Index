import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { LocalStorageService } from '@app/services/local-storage.service';

import { environment } from '@env/environment';
import { Token } from '@app/classes/user-token';
import { Login, Password } from '@app/classes/login';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userSubject: BehaviorSubject<Token | null>;
  private userTokenSubject: BehaviorSubject<Token | null>;
  public userToken: Observable<Token | null>;
  public baseUrl: string;
  public name: string | undefined | null;
  public userId: number;
  public email: string | undefined | null;
  public role: string | undefined | null;
  public token: string | undefined | null;
  public expires: number;

  constructor(
      private router: Router,
      private http: HttpClient,
      private alertService: AlertService,
      private localStorageService: LocalStorageService,
      private tokenOptionsService: TokenOptionsService,
      @Inject('BASE_URL') _baseUrl: string,
  ) {
      this.userSubject = new BehaviorSubject(JSON.parse(this.localStorageService.get('user')!));
      this.userTokenSubject = new BehaviorSubject(JSON.parse(this.localStorageService.get('userToken')!));
      this.userToken = this.userTokenSubject.asObservable();
      console.log(this.userToken);
      this.baseUrl = _baseUrl;
      this.name = localStorageService.get('name');
      this.userId = localStorageService.getNum('userId');
      this.email = localStorageService.get('email');
      this.role = localStorageService.get('role');
      this.token = localStorageService.get('token');
      this.expires = localStorageService.getNum('expires');
  }

  // public get userValue() {
  //     return this.userSubject.value;
  // }

  logIn(login: Login) {
    return this.http.post<Token>(this.baseUrl + 'api/account/login', login)
      .pipe(map(result => {
        this.tokenOptionsService.setAll(result);
        return result;
      }));
  }

  logout() {
    this.tokenOptionsService.clear();
    this.userSubject.next(null);
  }

  register(user: Token) {
    return this.http.post<Token>(this.baseUrl + 'api/account/register', user)
    .pipe(map(result => {
      this.tokenOptionsService.setAll(result);
      return result;
    }));
  }

  personal(params: any) {
    const userId = this.tokenOptionsService.getUserId();
    return this.http.put<Token>(this.baseUrl + 'api/account/' + userId, params, this.tokenOptionsService.getOptions())
      .pipe(map(result => {
        this.tokenOptionsService.setAll(result);
          return result;
      }));
  }

  password(params: any) {
    const userId = this.tokenOptionsService.getUserId();
    return this.http.post<Token>(this.baseUrl + 'api/account/' + userId, params, this.tokenOptionsService.getOptions())
      .pipe(map(result => {
        console.log(result);
        return result;
      }));
  }

  getAll() {
    return this.http.get<Token[]>(this.baseUrl + 'api/users', this.tokenOptionsService.getOptions())
    .pipe(map(result => {
      // console.log(result);
      return result;
    }))
  }

  getById(id: string) {
      // return this.http.get<User>(`${environment.apiUrl}/users/${id}`);
  }

  delete(id: string) {
      // return this.http.delete(`${environment.apiUrl}/users/${id}`)
      //     .pipe(map(x => {
      //         // auto logout if the logged in user deleted their own record
      //         if (id == this.userValue?.id) {
      //             this.logout();
      //         }
      //         return x;
      //     }));
  }

}
