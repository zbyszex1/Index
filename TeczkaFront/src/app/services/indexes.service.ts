import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '@app/classes/person';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserToken } from '@app/classes/user-token';
import { map } from 'rxjs';
import { AlertService } from './alert.service';
import { TokenOptionsService } from './token-options.service';
import { Scan } from '@app/classes/scan';

@Injectable({
  providedIn: 'root'
})
export class IndexesService {

  public baseUrl: string;
  jwtService: JwtHelperService = new JwtHelperService();

  constructor(
    private http: HttpClient,
    private alertService: AlertService,
    private tokenOptionsService: TokenOptionsService,
    @Inject('BASE_URL') _baseUrl: string,
    ) { 
      this.baseUrl = _baseUrl;
  }

  addPerson(person: Person)  {
    return this.http.post<Person>(this.baseUrl + 'api/persons', person, this.tokenOptionsService.getOptions())
      .pipe(map(result => {
        return result;
      }));
  }

  doneIndexed(outData: Scan)  {
    // const data = JSON.stringify({'Done': true});
    console.log(outData);
    return this.http.put<any>(this.baseUrl + 'api/available/'+outData.id, outData, this.tokenOptionsService.getOptions())
      .pipe(map(result => {
        return result;
      }));
  }

  generate() {
    return this.http.post<any>(this.baseUrl + 'api/generate', null, this.tokenOptionsService.getOptions())
      .pipe(map(result => {
        return result;
      }));
  }
}

