import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HostListener } from '@angular/core';
import { Component, Inject, ViewChild } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { NgForm } from '@angular/forms';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})


export class ContactComponent {

  http: HttpClient | null;
  baseUrl: string;
  form!: FormGroup;
  processing: boolean;
  loading = false;
  submitted = false;

  contactForm = {
    sender: '',
    email: '',
    subject: '',
    message: ''
  }

  constructor( 
    @Inject('BASE_URL') _baseUrl: string,
    private formBuilder: FormBuilder,
    private alertService: AlertService,
    private _http: HttpClient
  ) {
    this.http = _http;
    this.baseUrl = _baseUrl;
    this.processing = false;
  }
  contactFormRef: NgForm | undefined

  ngOnInit() {
    this.form = this.formBuilder.group({
      sender:    ['', [Validators.required, Validators.minLength(6)]],
      email:   ['', [Validators.required, Validators.email]],
      subject: ['wiadomość z formularz TECZKA.MEN', [Validators.required]],
      message: ['', [Validators.required, Validators.minLength(12)]]
    });
  }

  ngOnDestroy() {

  }

  get f() { return this.form.controls; }

  onSubmit() {
    this.submitted = true;
    this.alertService.clear();
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    console.log(this.form.value);
    this.processing = true;
      console.log(this.baseUrl + 'api/available/email')
      this.http?.post<any>( this.baseUrl + 'api/available/email',  this.form.value)
        .subscribe({
          next: (project) => {
            this.alertService.success('Wiadomość została wysłana');
            this.processing = false;
          },
          error: (error) => {
            this.alertService.error(error.error);
            this.processing = false;
          }
        });
    }

  resetForm() {
    this.submitted = false;
    this.contactFormRef?.reset(this.contactForm)
  }

}
