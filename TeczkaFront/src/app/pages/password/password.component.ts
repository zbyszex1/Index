import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AccountService } from '@app/services/account.service';
import { AlertService } from '@app/services/alert.service';


@Component({
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.css']
})
export class PasswordComponent implements OnInit, OnDestroy {
    form!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        this.form = this.formBuilder.group({
            old:      ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(8)]],
            confirm:  ['', [Validators.required]]
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
        this.accountService.password(this.form.value)
            .pipe(first())
            .subscribe({
                next: (result: any) => {
                    console.log(result);
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
                    this.router.navigateByUrl(returnUrl);
                          },
                error: error => {
                    console.log(error);
                    this.alertService.error(error.error);
                    this.loading = false;
                }
            });
    }
}