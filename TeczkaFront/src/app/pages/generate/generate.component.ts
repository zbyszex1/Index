import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { AlertService } from '@app/services/alert.service';
import { IndexesService } from '@app/services/indexes.service';
import { TokenOptionsService } from '@app/services/token-options.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-generate',
  templateUrl: './generate.component.html',
  styleUrls: ['./generate.component.css']
})
export class GenerateComponent {

  public tokenOptionsService: TokenOptionsService;
  public submittedWork: boolean;
  public url: string;
  public html: string;
  public json: string;
  public dataReady: boolean;
  public indexes: Index[];
  public http: HttpClient;
  public baseUrl: string;

  constructor(_http: HttpClient, 
    @Inject('BASE_URL') _baseUrl: string,
    private alertService: AlertService,
      private indexesService: IndexesService,
      _tokenOptionsService: TokenOptionsService) {
    this.tokenOptionsService = _tokenOptionsService;
    this.http = _http;
    this.baseUrl = _baseUrl;
    this.submittedWork = false;
    this.dataReady = false;
    this.url = 'https://teczka.sarata.pl/';
    this.html = '';
    this.json = '';
    this.indexes = [];
  }
  
  generate() {
    this.tokenOptionsService.doRefresh(this.http, this.baseUrl)
    .pipe(first())
    .subscribe({
      next: (result: any) => {
        this.doGenerate();
      },
      error: (error: any) => {
        this.alertService.error(error.message);
      }
    });
  };

  doGenerate() {
    this.dataReady = false;
    this.submittedWork = true;
    this.alertService.clear();
    this.indexesService.generate() 
      .pipe(first())
      .subscribe({
        next: (result: Index[]) => {
          this.indexes = result;
          this.dataReady = true;
          this.json = JSON.stringify(result);
          this.json = this.json.replace('[{"name"', '[\n  {"name"').replace('}]}]', '}]}\n]');
          this.json = this.json.replace(/,\{"name"/g, ',\n  {"name"').replace(/,"units"/g, ',\n   "units"');
          this.json = this.json.replace(/\{"display"/g, '\n      {"display"').replace(/\}\]\}/g, '}\n   ]\n  }');
        },
        error: (error: any) => {
          console.log(error);
          this.alertService.error(error.message);
        }
      });
  }

  convert() {
    this.indexes.forEach(index => {
      this.html +='<div>\n<b>' + index.name + '&nbsp;&nbsp;</b>\n';
      index.units.forEach(unit => {
        this.html += '&nbsp;&nbsp;' + unit.display;
        unit.pages.forEach((page, i) => {
          this.html += (i==0) ? ': ' : ',\n&nbsp;&nbsp;&nbsp;&nbsp;';
          this.html +='<a href="' + this.url + unit.path +'/' +this.fillZeros(page) + '" target="_blank"><b>' + page + '</b></a>';
        });
        this.html += '\n';
      });
      this.html += '</div>';
    })
  }

  fillZeros(val: number) {
    let i_str = val.toString();
    if (val <= 99)
      i_str = '0' + i_str;
    if (val <= 9)
      i_str = '0' + i_str;
    return i_str;
  }
}

interface Index {
  name: string;
  units: Unit[];
}

interface Unit {
  display: string;
  path: string;
  pages: number[];
}
