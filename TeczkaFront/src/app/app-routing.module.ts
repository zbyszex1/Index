import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginActivate } from './classes/login-activate'
import { AdminActivate } from './classes/admin-activate'
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { PersonalComponent } from './pages/personal/personal.component';
import { PasswordComponent } from './pages/password/password.component';
import { AvailableComponent } from './pages/available/available.component';
import { ClassesComponent } from './pages/classes/classes.component';
import { GenerateComponent } from './pages/generate/generate.component';
import { ContactComponent } from './pages/contact/contact.component';
import { HelpComponent } from './pages/help/help.component';
import { GetRolesComponent } from './pages/get-roles/get-roles.component';
import { GetUsersComponent } from './pages/get-users/get-users.component';
import { GetArticlesComponent } from './pages/get-articles/get-articles.component';
import { GetSectionsComponent } from './pages/get-sections/get-sections.component';
import { GetClassesComponent } from './pages/get-classes/get-classes.component';
import { GetPersonsComponent } from './pages/get-persons/get-persons.component';
import { GetIndexesComponent } from './pages/get-indexes/get-indexes.component';
import { GetScansComponent } from './pages/get-scans/get-scans.component';
import { CounterComponent } from './pages/counter/counter.component';
import { FetchDataComponent } from './pages/fetch-data/fetch-data.component';
import { FetchPersonsComponent } from './pages/fetch-persons/fetch-persons.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { ServerComponent } from './pages/server/server.component';

const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'personal', component: PersonalComponent, canActivate:[LoginActivate] },
    { path: 'password', component: PasswordComponent, canActivate:[LoginActivate] },
    { path: 'help', component: HelpComponent },
    { path: 'contact', component: ContactComponent },
    { path: 'server', component: ServerComponent },
    { path: 'generate', component: GenerateComponent, canActivate:[AdminActivate] },
    { path: 'classificate', component: ClassesComponent, canActivate:[LoginActivate] },
    { path: 'available', component: AvailableComponent, canActivate:[LoginActivate] },
    { path: 'get-roles', component: GetRolesComponent },
    { path: 'get-users', component: GetUsersComponent, canActivate:[AdminActivate] },
    { path: 'get-articles', component: GetArticlesComponent },
    { path: 'get-sections', component: GetSectionsComponent },
    { path: 'get-classes', component: GetClassesComponent },
    { path: 'get-persons', component: GetPersonsComponent },
    { path: 'get-scans', component: GetScansComponent },
    { path: 'get-indexes', component: GetIndexesComponent },
    { path: 'fetch-persons', component: FetchPersonsComponent, canActivate:[AdminActivate] },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent },
    { path: 'page-not-found', component: PageNotFoundComponent },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'enabled',
    // useHash: true,
    anchorScrolling: 'enabled',
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
