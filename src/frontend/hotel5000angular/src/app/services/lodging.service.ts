import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Lodging } from '../models/Lodging';
import { Result } from '../models/Result';

@Injectable({
  providedIn: 'root'
})
export class LodgingService {
  lodgingsApi: string = environment.apiUrl + '/api/lodgings';

  constructor(private http: HttpClient) { }

  getLodgings(id?: number): Observable<Result<Lodging[]>>{
    let queryParams: string = '?';
    if(id !== undefined)
      queryParams = queryParams + 'id=' + id;
    return this.http.get<Result<Lodging[]>>(`${this.lodgingsApi}${queryParams}`);
  }
}
