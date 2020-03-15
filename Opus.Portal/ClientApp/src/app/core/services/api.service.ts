import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';

const BASE_URL = '';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
    
  private options = {
    headers: this.getHeader(),
    withCredentials: false
  };

  constructor(private httpClient: HttpClient) { }

  public get(path: string, params: HttpParams = new HttpParams()): Observable<any> {
    return this.httpClient
      .get(BASE_URL + path, { headers: null, params })
      .pipe(catchError(this.formatErrors));
  }

  public post(path: string, body: any = {}): Observable<any> {
    return this.httpClient
      .post(BASE_URL + path, JSON.stringify(body), this.options)
      .pipe(catchError(this.formatErrors));
  }

  private getHeader(): any {
    const token = localStorage.getItem('auth-token');
    if (token !== null) {
      return new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      });
    }

    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  public formatErrors(error: any): Observable<any> {
    return throwError(error.error);
  }
}