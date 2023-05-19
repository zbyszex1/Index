import { BrowserModule } from '@angular/platform-browser';
import { Inject, Injectable, NgModule, isDevMode } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module'; 
import { LoginActivate } from './classes/login-activate'
import { AdminActivate } from './classes/admin-activate'

import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { AlertComponent } from './components/alert/alert.component';

import { GetRolesComponent } from './pages/get-roles/get-roles.component';
import { GetUsersComponent } from './pages/get-users/get-users.component';
import { GetArticlesComponent } from './pages/get-articles/get-articles.component';
import { GetSectionsComponent } from './pages/get-sections/get-sections.component';
import { GetPersonsComponent } from './pages/get-persons/get-persons.component';
import { GetIndexesComponent } from './pages/get-indexes/get-indexes.component';
import { GetScansComponent } from './pages/get-scans/get-scans.component';
import { AvailableComponent } from './pages/available/available.component';

import { FetchDataComponent } from './pages/fetch-data/fetch-data.component';
import { FetchPersonsComponent } from './pages/fetch-persons/fetch-persons.component';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { PasswordComponent } from './pages/password/password.component';
import { AccountService } from './services/account.service';
import { LocalStorageService } from './services/local-storage.service';
import { TokenOptionsService } from './services/token-options.service';
import { AlertService } from './services/alert.service';
import { IndexesService } from './services/indexes.service';
import { PersonalComponent } from './pages/personal/personal.component';
import { ContactComponent } from './pages/contact/contact.component';
import { HelpComponent } from './pages/help/help.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ServerComponent } from './pages/server/server.component';
import { GenerateComponent } from './pages/generate/generate.component';
import { GetClassesComponent } from './pages/get-classes/get-classes.component';
import { ClassesComponent } from './pages/classes/classes.component';

import { JwtModule, JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxPaginationModule } from 'ngx-pagination';


export function jwtOptionsFactory(http: HttpClient) {
  const domain = isDevMode() ? 'localhost:44440' : 'teczka.men';
  return {
    tokenGetter: () => {
      return localStorage.getItem('jwtToken');
    },
    allowedDomains: [domain],
    disallowedRoutes: [
      domain + '/api/account/login',
      domain + '/api/account/register',
      domain + '/api/account/refresh',
      domain + '/api/roles',
      domain + '/api/articles',
      domain + '/api/sections',
      domain + '/api/classes',
      // domain + '/api/persons',
      domain + '/api/scans',
      domain + '/api/indexes',
      domain + '/api/weatherforecast',
    ]
  }
}

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  baseUrl: string;
  constructor(private jwtHelper: JwtHelperService,
    private http: HttpClient,
    private alertService: AlertService,
    private localStorageService: LocalStorageService,
    private tokenOptionsService: TokenOptionsService,
    @Inject('BASE_URL') _baseUrl: string,
  ) {
    this.baseUrl = _baseUrl;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('jwtToken');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      console.log('token OK.')
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    // } else {
    //   console.log('** Token Expired **')
    }

    return next.handle(request);
  }
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    PasswordComponent,
    ContactComponent,
    HelpComponent,
    PersonalComponent,
    AlertComponent,

    GetRolesComponent,
    GetUsersComponent,
    GetArticlesComponent,
    GetSectionsComponent,
    GetPersonsComponent,
    GetIndexesComponent,
    GetScansComponent,
    AvailableComponent,
    FetchDataComponent,
    FetchPersonsComponent,
    PageNotFoundComponent,
    ServerComponent,
    GenerateComponent,
    GetClassesComponent,
    ClassesComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    ModalModule.forRoot(),
    JwtModule.forRoot({
      jwtOptionsProvider: {
        provide: JWT_OPTIONS,
        useFactory: jwtOptionsFactory,
        deps: [HttpClient]
      }
    }),
    NgxPaginationModule
  ],
  providers: [
    LocalStorageService,
    TokenOptionsService,
    AlertService,
    AccountService,
    IndexesService,
    LoginActivate,
    AdminActivate,
    { provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor, 
      multi: true
    }
    // { provide: LocationStrategy, 
    //   // useClass: HashLocationStrategy
    //   useClass: PathLocationStrategy
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
}

