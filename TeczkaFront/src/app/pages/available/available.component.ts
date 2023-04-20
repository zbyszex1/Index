import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { LocalStorageService } from '../../services/local-storage.service';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { IndexesService } from '@app/services/indexes.service';
import { AccountService } from '../../services/account.service';
import { Person } from '@app/classes/person';
import { Scan } from '@app/classes/scan';
import { pipe } from 'rxjs';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-available',
  templateUrl: './available.component.html',
  styleUrls: ['./available.component.css']
})
export class AvailableComponent implements OnInit, OnDestroy {
  private X: number;
  private Y: number;
  private W: number;
  private H: number;
  // private sectionList: string[];
  private idList: number[];
  private section: string;
  private winRef: any;
  private alert: AlertService;
  private sectionId: number;
  private scanId: number;
  public  personId: number;
  public  last: string;
  public  first: string;
  public  persons: PersonDetailed[];
  public  sectionsReady: boolean;
  public  pagesReady: boolean;
  public  personsReady: boolean;
  public  sections: SectionDetailed[];
  public  scans: ScanDetailed[];
  public  scansSelected: ScanDetailed[];
  public  sectionSelect: SectionSelect[];
  public  personSelect: PersonSelect[];
  public  indeksSelect: PersonSelect[];
  public  pageSelect: number[];
  public  page: string;
  public  formPerson!: FormGroup;
  public  loading = false;
  public  submittedPerson: boolean;
  public  addingPerson: boolean;


  constructor(http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string,
        private alertService: AlertService,
        private localStorageService: LocalStorageService,
        private indexesService: IndexesService,
        private accountService: AccountService,
        private tokenOptionsService: TokenOptionsService) {
    this.X = 0;
    this.Y = 0;
    this.W = 1024;
    this.H = 512;
    this.sectionId = 0;
    this.scanId = 0;
    this.personId = 0;
    this.last = '';
    this.first = '';
    this.sectionsReady = false;
    this.pagesReady = false;
    this.personsReady = false;
    this.sections = [];
    this.scansSelected = [];
    this.scans = [];
    this.persons = [];
    this.sectionSelect = [];
    this.personSelect = [];
    this.indeksSelect = [];
    this.pageSelect = [];
    // this.sectionList = [];
    this.idList = [];
    this.page = '';
    this.section = '';
    this.winRef = null;
    this.alert = alertService;
    this.submittedPerson = false;
    this.addingPerson = false;
    this.formPerson = new FormGroup({
      first: new FormControl(),
      last: new FormControl()
    })
    this.alert.clear();
    http.get<SectionDetailed[]>(baseUrl + 'api/available/sections', this.tokenOptionsService.getOptions())
      .subscribe(result => {
        this.sections = result;
        if (this.scans != null)
        this.processSS();
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
      http.get<ScanDetailed[]>(baseUrl + 'api/available/scans', this.tokenOptionsService.getOptions())
      .subscribe(result => {
        this.scans = result;
        if (this.sections != null)
        this.processSS();
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
      http.get<PersonDetailed[]>(baseUrl + 'api/available/persons', this.tokenOptionsService.getOptions())
      .subscribe(result => {
        this.persons = result;
        this.processP();
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
  }
  ngOnInit(): void {
    this.X = this.localStorageService.getNum('winRef_X', 0);
    this.Y = this.localStorageService.getNum('winRef_Y', 0);
    this.W = this.localStorageService.getNum('winRef_W', screen.width/2);
    this.H = this.localStorageService.getNum('winRef_H', screen.height);
    this.openSkan();
  }

  ngOnDestroy(): void {
    if (this.winRef != null && !this.winRef.closed) {
      this.localStorageService.setNum('winRef_X', this.X);
      this.localStorageService.setNum('winRef_Y', this.Y);
      this.localStorageService.setNum('winRef_W', screen.width/2);
      this.localStorageService.setNum('winRef_H', this.H);
      this.winRef.close();
    }  
  }

  processSS() {
    this.scans?.forEach(scan => {
      const sectionId = scan.sectionId;
      const id = this.idList.find(i => i == sectionId);
      if (id == null || typeof id == 'undefined')
      {
        const section = this.sections?.find(s => s.id == sectionId);
        this.idList.push(sectionId)
        // this.sectionList.push(section?.name ? section?.name : '');
        this.sectionSelect.push( { 'id': sectionId, 'name': (section?.name ? section?.name : '')} );
      }
    });
    this.sectionsReady = true;
    this.pagesReady = true;
  }

  processP() {
    this.persons?.forEach(person => {
        this.personSelect.push( { 'id': person.id, 'name': person.last + ' ' +person.first.replace('_', ' ')} );
    });
    this.personsReady = true;
  }
  
  onSection(event: any) {
    let i = this.scans?.findIndex(s => s.sectionId == event);
    this.sectionId = i;
    const section = this.sections?.find(s => s.id == event);
    this.section = section?.name? section?.name : '';
    this.scansSelected = [];
    while (this.scans[i].sectionId == event) {
      this.scansSelected.push(this.scans[i]);
      i++;
    }
  }
  onPage(event: any) {
    const scan = this.scansSelected?.find(s => s.id == event);
    this.scanId = scan ? scan?.id : 0;
    this.page = String(scan?.page).padStart(3, '0');
    this.openSkan()
  }

  onPerson(event: any) {
    this.personId = event;
    console.log(event)
  }

  setPerson() {
    const person = this.persons?.find(s => s.id == this.personId);
    console.log(person)
    if (person != null)
      this.indeksSelect.push( { 'id': person?.id, 'name': person?.last + ' ' +person?.first} );
  }

  onIndeks(event: any) {
    const person = this.indeksSelect?.find(s => s.id == event);
    this.page = '';
    if (person!=null) {
      const idxs = this.indeksSelect?.indexOf(person);
      this.indeksSelect.splice(idxs,1);
    }
  }

  openSkan() {
    let URL = "https://teczka.sarata.pl/";
    if (typeof this.section != 'undefined' && this.section.length > 0 && 
        typeof this.page != 'undefined' && this.page?.length > 0)
    {
      URL += this.section + "/" + this.page;
    } else {
      URL += "blank";
    }
    if (this.winRef != null && !this.winRef.closed) {
      this.winRef.location.href = URL;
    }  else {
      let strWindowFeatures = 
        "location=yes,height=" + this.H + 
        ",width=" + this.W + 
        ",left=" + this.X + 
        ",top=" + this.Y + 
        ",scrollbars=yes,status=no";
      this.winRef = window.open(URL, "blank", strWindowFeatures);
    }
  }

  // convenience getter for easy access to formPerson fields
  get f() {
    return this.formPerson.controls; 
  }

  addPerson(): any {
    this.addingPerson = true;
  }

  cancelPerson(): any {
    this.first = '';
    this.last = '';
    setTimeout(() => {
      this.addingPerson = false;
    },500)
  }

  onSubmitPerson() {
    this.submittedPerson = true;
    this.alertService.clear();
    if (this.formPerson.invalid) {
        return;
    }
    this.indexesService.addPerson(this.formPerson.value) 
      .pipe(first())
      .subscribe({
        next: (result: Person) => {
          this.first = '';
          this.last = '';
          this.addingPerson = false;
          console.log(result)
          this.insertPerson(result);
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.error(error.message);
        }
      });
    }

    insertPerson(person: Person) {
      let _not = true;
      while (person.first != null && person.first?.indexOf('_') > 0) {
        person.first = person.first?.replace('_', ' ');
      }
      const full = person.last + ' ' + person.first;
      const object: any = {'id': person?.id, 'name': full};
      for (let i=0; _not && i<this.personSelect.length; i++) {
        if (this.personSelect[i].name.localeCompare(full) > 0) {
          this.personSelect.splice(i, 0, object)
          _not = false;
        }
      }
      this.indeksSelect.push( object );
    }

    writeIndex(): any {
      const outData: Scan = new Scan();
      outData.done = true;
      outData.id = this.scanId;
      outData.userId =  this.tokenOptionsService.getUserId();
      outData.persons = [];
      this.indeksSelect.forEach((person) => {
        outData.persons?.push(person.id);
      });
      this.indexesService.doneIndexed(outData) 
        .pipe(first())
        .subscribe({
          next: (result: Person) => {
            this.removeFromCombo();
            this.indeksSelect = [];
            this.addingPerson = false;
          },
          error: (error: any) => {
            console.log(error);
            this.alertService.error(error.message);
          }
        });
        this.page = '';
        this.openSkan();
      }
  
    removeFromCombo() {
      const scan = this.scansSelected?.find(s => s.id == this.scanId);
      this.page = '';
      if (scan!=null) {
        const idx = this.scans?.indexOf(scan);
        this.scans.splice(idx,1);
        const idxs = this.scansSelected?.indexOf(scan);
        this.scansSelected.splice(idxs,1);
      }
    }

    removeFromList() {
    }

}


interface SectionDetailed {
  id: number;
  routeId: number;
  name: string;
  thumbs: string;
  pages: string;
  pdf: string;
  header: string;
  description: string;
  offset: number;
  min: number;
  max: number;
  created: string;
  updated: string;
}

interface ScanDetailed {
  id: number;
  sectionId: number;
  userId: number;
  page: number;
  done: boolean;
  created: string;
  updated: string;
}

interface PersonDetailed {
  id: number;
  last: string;
  first: string;
}

interface SectionSelect {
  id: number;
  name: string;
}

interface PersonSelect {
  id: number;
  name: string;
}

interface OutData {
  
}
