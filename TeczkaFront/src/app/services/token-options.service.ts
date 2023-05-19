import { Injectable } from '@angular/core';
import { LocalStorageService } from '@app/services/local-storage.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject, map, of, range, tap } from 'rxjs';
import { Token, UserToken } from '@app/classes/user-token';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})

export class TokenOptionsService {

  private jwtService: JwtHelperService = new JwtHelperService();
  private subject = new Subject<Token>();
  public name: string | undefined | null;
  public userId: number;
  public role: string | undefined | null;
  public token: string | undefined | null;
  public refreshToken: string | undefined | null;
  constructor(
    private localStorageService: LocalStorageService) {
    this.name = 'unknown';
    this.userId = 0;
    this.role = 'unknown';
    this.token = 'empty';
    this.refreshToken = 'empty';
    this.getAll();
  }

  onToken(): Observable<Token> {
    return this.subject.asObservable();
  }

  getOptions() {
    this.getToken();
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.token}`
    });
    const requestOptions = { headers: headers };
    return requestOptions;
  }

  getToken(): string {
    this.token = this.localStorageService.get('jwtToken');
    return this.token != null ? this.token : '';
  }

  getUser(): string {
    this.name = this.localStorageService.get('name');
    return this.name != null ? this.name : '';
  }

  getUserId(): number {
    this.userId = this.localStorageService.getNum('userId');
    return this.userId;
  }

  getRole(): string {
    this.role = this.localStorageService.get('role');
    return this.role != null ? this.role : '';
  }

  getRefreshToken(): string {
    this.refreshToken = this.localStorageService.get('refreshToken');
    return this.refreshToken != null ? this.refreshToken : '';
  }

  isLoggedIn(): boolean {
    if (this.getToken().length == 0 || this.getRefreshToken().length == 0) {
      return false;
    }
    return !this.isExpiredRefresh();
  }

  isAdmin(): boolean {
    return this.getRole().toLowerCase() == 'admin';
  }

  isSuperviser(): boolean {
    return this.getRole().toLowerCase() == 'superviser';
  }

  isWriter(): boolean {
    return this.getRole().toLowerCase() == 'writer';
  }

  isReader(): boolean {
    return this.getRole().toLowerCase() == 'reader';
  }

  isExpiredJwt(): boolean {
    let expires: number = this.localStorageService.getNum('expiresJwt');
    if (expires <= 0) {
      return true;
    }
    expires *= 1000;
    const delta = expires - Date.now();
    console.log(delta);
    return delta < 0;
  }

  isExpiredRefresh(): boolean {
    let expires: number = this.localStorageService.getNum('expiresRefresh');
    if (expires <= 0) {
      return true;
    }
    expires *= 1000;
    const delta = expires - Date.now();
    return delta < 0;
  }

  getAll() {
    this.getToken();
    this.getRefreshToken();
    this.getUser();
    this.getUserId();
    this.getRole();
    this.subject.next(new Token());
  }

  setAll(token: Token) {
    this.decodeToken(token);
    this.subject.next(token);
  }

  decodeToken(token: Token) {
    this.localStorageService.set('jwtToken', token.jwtToken);
    this.localStorageService.set('refreshToken', token.refreshToken);
    var userInfo = this.jwtService.decodeToken(token.jwtToken);
    const userToken = new UserToken();
    userToken.token = token.jwtToken;
    for (let key of Object.keys(userInfo)) {
      const val = userInfo[key];
      if (key.endsWith('claims/nameidentifier')) {
        this.localStorageService.setNum('userId', val);
        userToken.userId = val;
      }
      if (key.endsWith('claims/emailaddress')) {
        this.localStorageService.set('email', val);
      }
      if (key.endsWith('claims/name')) {
        this.localStorageService.set('name', val);
        userToken.name = val;
      }
      if (key.endsWith('claims/mobilephone')) {
        this.localStorageService.set('phone', val);
      }
      if (key.endsWith('claims/role')) {
        this.localStorageService.set('role', val);
        userToken.role = val;
      }
      if (key.endsWith('exp')) {
        this.localStorageService.setNum('expiresJwt', val);
      }
    }
    var refreshInfo = this.jwtService.decodeToken(token.refreshToken);
    for (let key of Object.keys(refreshInfo)) {
      const val = refreshInfo[key];
      if (key.endsWith('exp')) {
        this.localStorageService.setNum('expiresRefresh', val);
      }
    }
    // this.isExpiredJwt();
    console.log(this.localStorageService.get('name'));
  }

  clear() {
    this.localStorageService.remove('jwtToken');
    this.localStorageService.remove('refreshToken');
    this.localStorageService.remove('name');
    this.localStorageService.remove('userId');
    this.localStorageService.remove('role');
    this.localStorageService.remove('email');
    this.localStorageService.remove('phone');
    this.localStorageService.remove('expiresJwr');
    this.localStorageService.remove('expiresRefresh');
    this.subject.next(new Token());
  }

  doRefresh(http: HttpClient, baseUrl: string) {
    if (!this.isExpiredJwt()) {
      console.log('not expired')
      return of({});
    }
    const refreshToken = this.getRefreshToken();
    const userId = this.getUserId();
    const token = {refreshToken: refreshToken, userId: userId };
    return http.post<Token>(baseUrl + 'api/account/refresh', token)
      .pipe(
        map(result => {
          this.setAll(result);
          console.log('tokenOptionsService.doneRefresh()')
          return result;
        })
      )
  }

}
