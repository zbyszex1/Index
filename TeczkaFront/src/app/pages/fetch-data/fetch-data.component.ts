import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] | null;
  public bleble: any = null;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    const words: string[] = ['appearance','color','condition','personality','quantity','shapes',
                             'size','sounds','taste','time','touch','animals','business','education',
                             'family','food','health','media','people','place','profession','religion',
                             'science','sports','technology','thing','time','transportation'];
    const number: number = Math.floor(Math.random() * words.length);
    const word: string = words[number];
    console.log(word);
    http.get<any[]>(baseUrl + 'api/test/'+word)
      .subscribe(result => {
        let blebles = result;
        blebles.forEach(b => {
          this.bleble = b;
        });
        
      }, error => {
        console.error(error);
        // this.alert.error(error.message);
        });

    this.forecasts = null;
    http.get<WeatherForecast[]>(baseUrl + 'api/weatherforecast')
      .subscribe(result => {
        this.forecasts = result;
      }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
