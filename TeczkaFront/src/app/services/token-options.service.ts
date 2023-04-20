import { Injectable } from '@angular/core';
import { LocalStorageService } from '@app/services/local-storage.service';
import { HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { Token, UserToken } from '@app/classes/user-token';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class TokenOptionsService {

  private jwtService: JwtHelperService = new JwtHelperService();
  private subject = new Subject<Token>();
  private defaultName = '';

  public name: string | undefined | null;
  public userId: number;
  public role: string | undefined | null;
  public token: string | undefined | null;

  constructor(
    private localStorageService: LocalStorageService
  ) {
    this.name = 'unknown';
    this.userId = 0;
    this.role = 'unknown';
    this.token = 'empty';
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
    this.token = this.localStorageService.get('token');
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

  getRole() {
    this.role = this.localStorageService.get('role');
    return this.role != null ? this.role : '';
  }

  isLoggedIn(): boolean {
    return this.getToken().length > 0;
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

  isExpired(): boolean {
    let expires: number = this.localStorageService.getNum('expires');
    if (expires <= 0) {
      console.log('expired');
      return true;
    }
    expires *= 1000;
    let expiresN = new Date(expires)
    const now: number = Date.now();
    const delta = expires - now;
    console.log('delta: ' + delta)
    return delta < 5000;
  }

  setAll(token: Token) {
    this.subject.next(token);
    this.decodeToken(token);
  }

  decodeToken(token: Token) {
    this.localStorageService.set('token', token.serverToken);
    var userInfo = this.jwtService.decodeToken(token.serverToken);
    const userToken = new UserToken();
    userToken.token = token.serverToken;
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
        this.localStorageService.setNum('expires', val);
      }
    }
    // console.log(this.localStorageService.getNum('userId'));
    // console.log(this.localStorageService.get('name'));
    // console.log(this.localStorageService.get('email'));
    // console.log(this.localStorageService.get('phone'));
    // console.log(this.localStorageService.get('role'));
    console.log(this.localStorageService.getNum('expires'));
    this.localStorageService.set('userToken', JSON.stringify(userToken));
    this.isExpired();
  }

  clear() {
    this.localStorageService.remove('token');
    this.localStorageService.remove('name');
    this.localStorageService.remove('userId');
    this.localStorageService.remove('role');
    this.localStorageService.remove('email');
    this.localStorageService.remove('phone');
    this.localStorageService.remove('expires');
    // console.log('clear()');
  }
}
