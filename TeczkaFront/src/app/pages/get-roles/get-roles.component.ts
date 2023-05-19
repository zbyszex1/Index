import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-roles',
  templateUrl: './get-roles.component.html',
  styleUrls: ['./get-roles.component.css']
})
export class GetRolesComponent {
  public roles: Roles[] | null;
  public alert: AlertService;

  constructor(http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private alertService: AlertService,
    tokenOptionsService: TokenOptionsService) {
    this.roles = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<Roles[]>(baseUrl + 'api/roles')
      .subscribe(result => {
        this.roles = result;
      }, error => {
        console.error(error)
        this.alert.error(error.message);
      });
  }
}

interface Roles {
  id: number;
  name: string;
  level: number;
  created: string;
  updated: string;
}
