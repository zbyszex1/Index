import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { TokenOptionsService } from "@app/services/token-options.service";
import { Observable } from "rxjs";

@Injectable()
export class LoginActivate implements CanActivate {
  constructor(private tokenOptionsService: TokenOptionsService, 
              private router: Router) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean>|Promise<boolean>|boolean {
    if (!this.tokenOptionsService.isLoggedIn()) {
        console.log(this.router)
        this.router.navigate(['login']);
    }
    return true;
  }

  adminActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean>|Promise<boolean>|boolean {
    if (!this.tokenOptionsService.isAdmin()) {
        this.router.navigate(['login']);
    }
    return true;
  }
}