import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-users',
  templateUrl: './get-users.component.html',
  styleUrls: ['./get-users.component.css']
})
export class GetUsersComponent {
  public users: UsersDetailed[] | null;
  public alert: AlertService;

  constructor(
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private alertService: AlertService,
    tokenOptionsService: TokenOptionsService) {
    this.users = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<UsersDetailed[]>(baseUrl + 'api/users', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.users = result;
        this.users.forEach(element => {
        });
        
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
}
}

interface UsersDetailed {
  id: number;
  roleId: number;
  name: string;
  email: string;
  phone: string;
  tempPassword: string;
  passwordHash: string;
  created: string;
  updated: string;
}
