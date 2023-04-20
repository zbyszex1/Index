import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AlertService } from '@app/services/alert.service';
import { AccountService } from '@app/services/account.service';
import { pipe } from 'rxjs';
import { Token } from '../../classes/user-token';
import { TokenOptionsService } from '../../services/token-options.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  form!: FormGroup;
  loading = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private accountService: AccountService,
      private alertService: AlertService,
      private tokenOptionsService: TokenOptionsService,
      // private jwtTokenService JWTTokenService,
  ) { }

  ngOnInit() {
      this.form = this.formBuilder.group({
          email: ['', [Validators.required, Validators.email]],
          password: ['', [Validators.required, Validators.minLength(8)]]
      });
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
      this.submitted = true;

      // reset alerts on submit
      this.alertService.clear();

      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }

      this.loading = true;
      this.accountService.logIn(this.form.value) 
        .pipe(first())
        .subscribe({
          next: (result: Token) => {
            console.log(result);
            const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
            this.router.navigateByUrl(returnUrl);
            this.loading = false;
          },
          error: (error: any) => {
            console.log(error);
            this.alertService.error(error.error);
            this.loading = false;
          }
        });
  }
}