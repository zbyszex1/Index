import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AlertService } from '@app/services/alert.service';
import { TokenOptionsService } from '@app/services/token-options.service';

@Component({
  selector: 'app-get-articles',
  templateUrl: './get-articles.component.html',
  styleUrls: ['./get-articles.component.css']
})
export class GetArticlesComponent {
  public articles: Articles[] | null;
  public alert: AlertService;

  constructor(http: HttpClient, 
    @Inject('BASE_URL') baseUrl: string,
    private alertService: AlertService,
    tokenOptionsService: TokenOptionsService) {
    this.articles = null;
    this.alert = alertService;
    this.alert.clear();
    http.get<Articles[]>(baseUrl + 'api/articles', tokenOptionsService.getOptions())
      .subscribe(result => {
        console.log(result);
        this.articles = result;
      }, error => {
        console.error(error);
        this.alert.error(error.message);
      });
}
}

interface Articles {
  id: number;
  name: string;
  created: string;
  updated: string;
}
