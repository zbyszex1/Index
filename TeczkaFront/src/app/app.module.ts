import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
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

import { CounterComponent } from './pages/counter/counter.component';
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
import { HashLocationStrategy, LocationStrategy, PathLocationStrategy } from '@angular/common';
import { ServerComponent } from './pages/server/server.component';
import { GenerateComponent } from './pages/generate/generate.component';

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
    GenerateComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    // RouterModule.forRoot([
    //   { path: '', component: HomeComponent, pathMatch: 'full' },
    //   { path: 'login', component: LoginComponent, pathMatch: 'full' },
    //   { path: 'register', component: RegisterComponent },
    //   { path: 'personal', component: PersonalComponent, canActivate:[LoginActivate] },
    //   { path: 'password', component: PasswordComponent, canActivate:[LoginActivate] },
    //   { path: 'help', component: HelpComponent },
    //   { path: 'contact', component: ContactComponent },
    //   { path: 'get-available', component: AvailableComponent, canActivate:[LoginActivate] },
    //   { path: 'get-roles', component: GetRolesComponent, canActivate:[AdminActivate] },
    //   { path: 'get-users', component: GetUsersComponent, canActivate:[AdminActivate] },
    //   { path: 'get-articles', component: GetArticlesComponent, canActivate:[AdminActivate] },
    //   { path: 'get-sections', component: GetSectionsComponent, canActivate:[AdminActivate] },
    //   { path: 'get-persons', component: GetPersonsComponent, canActivate:[AdminActivate] },
    //   { path: 'get-scans', component: GetScansComponent, canActivate:[AdminActivate] },
    //   { path: 'get-indexes', component: GetIndexesComponent, canActivate:[AdminActivate] },
    //   { path: 'fetch-data', component: FetchDataComponent, canActivate:[AdminActivate] },
    //   { path: 'fetch-persons', component: FetchPersonsComponent, canActivate:[AdminActivate] },
    //   { path: 'counter', component: CounterComponent },
    //   { path: 'page-not-found', component: PageNotFoundComponent },
    //   { path: '**', component: PageNotFoundComponent }
    // ])
  ],
  providers: [
    LocalStorageService,
    TokenOptionsService,
    AlertService,
    AccountService,
    IndexesService,
    LoginActivate,
    AdminActivate,
    // { provide: LocationStrategy, 
    //   // useClass: HashLocationStrategy
    //   useClass: PathLocationStrategy
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
}
