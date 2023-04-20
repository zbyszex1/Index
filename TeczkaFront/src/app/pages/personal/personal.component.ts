import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '@app/services/account.service';
import { AlertService } from '@app/services/alert.service';
import { first } from 'rxjs';
import { LocalStorageService } from '@app/services/local-storage.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-edit-user',
  templateUrl: './personal.component.html',
  styleUrls: ['./personal.component.css']
})
export class PersonalComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  loading = false;
  submitted = false;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private accountService: AccountService,
      private localStoraService: LocalStorageService,
      private tokenOptionsService: TokenOptionsService,
      private alertService: AlertService
  ) { }

  ngOnInit() {
      this.form = this.formBuilder.group({
          name:     [this.localStoraService.get('name'), [Validators.required, Validators.minLength(6)]],
          email:    [this.localStoraService.get('email'), [Validators.required, Validators.email]],
          phone:    [this.localStoraService.get('phone'), [Validators.required, Validators.minLength(9)]]
      });
  }

  ngOnDestroy() {

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
      console.log(this.form.value);
      this.accountService.personal(this.form.value) 
      .pipe(first())
      .subscribe({
        next: (result: any) => {
          console.log(result);
          this.tokenOptionsService.setAll(result);
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigateByUrl(returnUrl);
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.error(error.message);
          this.loading = false;
        }
      });
  }
}