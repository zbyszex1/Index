import { Component, OnInit, OnDestroy, Input, Inject } from '@angular/core';
import { AccountService } from '@app/services/account.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { Subscription } from 'rxjs';
import { MessageService } from '@app/services/message.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit, OnDestroy {

  constructor(http: HttpClient, 
    @Inject('BASE_URL') baseUrl: string,
    private tokenOptionsService: TokenOptionsService,
    private accountService: AccountService,
    private messageService: MessageService,
    ) {
    this.open = false;    
    this.ariaA = false;
    this.ariaU = false;
    this.ariaI = false;
    this.loggedIn = false;
    this.isAdmin = false;
    this.variable = "abs";
    this.emptys = [];
    const expired = tokenOptionsService.isExpired();
    if (expired) {
      this.accountService.logout(); 
    } else {
      http.get<any[]>(baseUrl + 'api/available/test', tokenOptionsService.getOptions())
        .subscribe(result => {
          tokenOptionsService.getUser();
          tokenOptionsService.getRole();
        }, error => {
          this.accountService.logout(); 
        });
    }
  }

  @Input() name = this.tokenOptionsService.getUser();

  tokenSubscription!: Subscription;
  open: boolean;
  ariaA: boolean;
  ariaU: boolean;
  ariaI: boolean;
  loggedIn: boolean;
  isAdmin: boolean;
  variable: string;
  winRef: any;
  emptys: any[];

  ngOnInit(): void {
    this.tokenSubscription = this.tokenOptionsService.onToken()
    .subscribe(token => {
      setTimeout( () => {
        this.loginState();
      },250)
    })
  }

  ngOnDestroy(): void {
    this.tokenSubscription.unsubscribe();
    this.tokenOptionsService.clear();
  }

  onDropdownA(): void {
    this.ariaA = !this.ariaA;
    this.ariaU = false;
    this.ariaI = false;
  }

  onDropdownU(): void {
    this.ariaU = !this.ariaU;
    this.ariaA = false;
    this.ariaI = false;
  }

  onDropdownI(): void {
    this.ariaI = !this.ariaI;
    this.ariaA = false;
    this.ariaU = false;
  }

  logout() {
    this.accountService.logout();
    this.refresh(true);
    setTimeout( () => {
      this.loginState();
    },250)
}

  refresh(delay=false) {
    this.open = false;
    this.ariaA = false;
    this.ariaU = false;
    this.ariaI = false;
  }

  loginState() {
    this.tokenOptionsService.isExpired();
    this.name = this.tokenOptionsService.getUser();
    this.loggedIn = this.tokenOptionsService.isLoggedIn();
    this.isAdmin = this.tokenOptionsService.isAdmin();
  }
}
