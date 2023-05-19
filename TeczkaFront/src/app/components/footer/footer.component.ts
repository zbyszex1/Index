import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { AccountService } from '@app/services/account.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { Subscription } from 'rxjs';
import { MessageService } from '@app/services/message.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit, OnDestroy {

  name: string;
  role: string;
  loggedIn: boolean;
  tokenSubscription!: Subscription;

  constructor(
    private tokenOptionsService: TokenOptionsService

  ) {
    this.name = '';
    this.role = '';
    this.loggedIn = false;
  }

  ngOnInit(): void {
    this.userState();
    this.tokenSubscription = this.tokenOptionsService.onToken()
    .subscribe(token => {
      setTimeout( () => {
        this.userState();
      }, 250 );
    })
  }

  ngOnDestroy(): void {
    this.tokenSubscription.unsubscribe();
  }

  userState() {
    this.name = this.tokenOptionsService.getUser();
    this.role = this.tokenOptionsService.getRole();
    this.loggedIn = this.tokenOptionsService.isLoggedIn();
    console.log(this.name)
    console.log(this.loggedIn)
  }
}
