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
  loggedIn: boolean;
  tokenSubscription!: Subscription;

  constructor(
    private tokenOptionsService: TokenOptionsService

  ) {
    this.name = '';
    this.loggedIn = false;
  }

  ngOnInit(): void {
    this.tokenSubscription = this.tokenOptionsService.onToken()
    .subscribe(token => {
      this.name = this.tokenOptionsService.getUser();
      this.loggedIn = this.tokenOptionsService.isLoggedIn();
      console.log(this.tokenOptionsService.isLoggedIn())
      console.log(this.tokenOptionsService.getUser())
// this.name = (token == null || typeof token.name == "undefined") ? '' : token.name;
      // this.loggedIn = this.name?.length > 0;
  })
  }

  ngOnDestroy(): void {
    this.tokenSubscription.unsubscribe();
  }

}
