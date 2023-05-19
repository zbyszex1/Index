import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { IndexesService } from '@app/services/indexes.service';
import { AccountService } from '../../services/account.service';
import { PersonUpdate } from '@app/classes/person';
import { first } from 'rxjs';


@Component({
  selector: 'app-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.css']
})
export class ClassesComponent implements OnInit, OnDestroy {

  private alert: AlertService;
  public  personsReady: boolean;
  // public  personSelect: PersonSelect;
  public persons: PersonDetailed[];
  public personIx: number;
  public personId: number;
  // public personName: string | undefined;
  public classes: Class[];
  public loading = false;
  public http: HttpClient;
  public baseUrl: string;
  public selectedValue:any;
  public option: HTMLOptionElement | undefined;

  constructor(http: HttpClient, 
    @Inject('BASE_URL') baseUrl: string,
    private alertService: AlertService,
    private localStorageService: LocalStorageService,
    private indexesService: IndexesService,
    private accountService: AccountService,
    private tokenOptionsService: TokenOptionsService) {
  this.alert = alertService;
  this.http = http;
  this.baseUrl = baseUrl;
  this.personsReady = false;
  this.personIx = 0;
  this.personId = 0;
  // this.personName = undefined;
  this.persons = [];
  this.classes = [];
  // this.personId = 0;
  this.persons = [];
  this.alert.clear();
  this.tokenOptionsService.doRefresh(this.http, this.baseUrl)
  .pipe(first())
  .subscribe({
    next: (result: any) => {
      this.getClasses();
      this.getPersons();
    },
    error: (error: any) => {
      this.alertService.error(error.message);
    }
  });

  }
  ngOnInit(): void {
  // this.openSkan();
  }

  ngOnDestroy(): void {
  }

  getClasses() {
    this.http.get<Class[]>(this.baseUrl + 'api/available/classes', this.tokenOptionsService.getOptions())
    .subscribe(result => {
      this.classes = result;
    }, error => {
      console.error(error);
      this.alert.error(error.message);
    });
  }

  getPersons() {
    this.http.get<PersonDetailed[]>(this.baseUrl + 'api/available/persons', this.tokenOptionsService.getOptions())
    .subscribe(result => {
      this.persons = result;
      // this.processP();
    }, error => {
      console.error(error);
      this.alert.error(error.message);
    });
  }

  getIndex(i: number) {
    return String(i).padStart(4, '0') + '\xa0\xa0\xa0'
  }
  getPerson(person: PersonDetailed) {
    let result: string = '';
    const classId = person.classId;
    this.classes.forEach(c => {
      if (c.id == person.classId) {
        result = this.formatClass(c);
      }
    });
    let last = person.last?.replace('_', ' ');
    let first = person.first?.replace('_', ' ');
    return result + last + ' ' + first;
  }

  onIndeks(event: any) {
    this.personId = event;
  }

  setClass(classId: number) {
    this.tokenOptionsService.doRefresh(this.http, this.baseUrl)
    .pipe(first())
    .subscribe({
      next: (result: any) => {
        this.doSetClass(classId);
      },
      error: (error: any) => {
        this.alertService.error(error.message);
      }
    });
  };

  doSetClass(classId: number) {
    const personId = this.personId;
    if (personId==0)
      return;
    const outData: PersonUpdate = new PersonUpdate();
    outData.classId =  classId;
    this.http.put<any>(this.baseUrl + 'api/persons/' + personId, outData)
    .subscribe(result => {
      var person  = this.persons.filter(function(p) {
        return p.id == personId;
      })[0];
      person.classId = classId;
    }, error => {
      console.error(error);
      this.alert.error(error.message);
    });
  }

  formatClass(c: Class) {
    let result = c.name;
    switch(c.id) {
      case 5:
        result += '\xa0\xa0\xa0\xa0';
        break;
      case 4:
        result += '\xa0';
        break;
      case 3:
        result += '\xa0\xa0\xa0';
        break;
      case 2:
        result += '\xa0\xa0';
        break;
      default:
        result += '\xa0\xa0\xa0';
        break;
    }
    return result;
  }
}

interface Class {
  id: number;
  name: string;
}

interface PersonDetailed {
  id: number;
  last?: string;
  first?: string;
  classId: number;
}

// interface SectionSelect {
//   id: number;
//   name: string;
// }

// interface PersonSelect {
//   id: number;
//   name: string;
// }

