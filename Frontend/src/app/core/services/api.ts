import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Injectable } from '@angular/core';
import { JsonConvert } from 'json2typescript';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
// import { UserLogged } from '../utils/userLogged';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

  protected jsonConvert: JsonConvert;
  protected changeDetectorRef: ChangeDetectorRef | undefined;

  constructor(protected http: HttpClient) {
    this.jsonConvert = new JsonConvert();
  }
  private formatErrors(error: any) {
    return throwError(() => error);
  }
  //Post
  protected post(
    url: string,
    body: any,
    contentType: string = 'application/json'
  ) {
    const options: any = {};
    // options['headers'] = this.getHeaders();
    options['headers']['Content-Type'] = contentType;
    return this.http.post(environment.apiUrl + url, body, options);
  }
  //Post

  //Get
  protected get(url: string, params: any = null): Observable<Object> {
    params = params || {};
    const options = {
      params,
    };
    // @ts-ignore
    // options['headers'] = this.getHeaders();
    return this.http.get(environment.apiUrl + url, options);
  }
  protected getCustom(url: string, params: any = null): Observable<Object> {
    params = params || {};
    const options = {
      params,
    };
    // @ts-ignore
    // options['headers'] = this.getHeaders();
    return this.http.get(environment.provinceUrl + url, options);
  }
  //post
  protected postEntity(entitySet: string, body: Object = {}): Observable<any> {
    const options: any = {};
    //@ts-ignore
     options['headers'] = this.getHeaders();
    return this.http
      .post(`${environment.apiUrl}${entitySet}`, body, options)
      .pipe(catchError(this.formatErrors));
  }

  protected getHeaders(): { [header: string]: string } {
    const headers: any = {
      'Accept': 'application/json',
    };
  
    // const userLogged = new UserLogged();
    // if (userLogged.isLogged()) {
    //   headers['Authorization'] = 'Bearer ' + userLogged.getToken();
    // }
    return headers;
  }
  
  protected putEntity(
    entitySet: string,
    id: number,
    body: Object = {}
  ): Observable<any> {
    const options: any = {};;
    const headers: any = {
      Accept: 'application/json',
      'Content-Type': 'application/x-www-form-urlencoded',
    };
     options['headers'] = this.getHeaders()
    return this.http
      .put(`${environment.apiUrl}${entitySet}/${id}`, body, options)
      .pipe(catchError(this.formatErrors));
  }
  protected patchEntity(
    entitySet: string,
    id: number,
    body: Object = {}
  ): Observable<any> {
    const options: any = {};
    options['headers'] = this.getHeaders();
    let header: any = {
      Accept: 'application/json',
      'Content-Type': 'application/x-www-form-urlencoded',
    };
    return this.http
      .patch(`${environment.apiUrl}${entitySet}/${id}`, body, options)
      .pipe(catchError(this.formatErrors));
  }
  protected deleteEntity(entitySet: string): Observable<any> {
    const options: any = {};
    // options['headers'] = this.getHeaders();
    const headers: any = {
      Accept: 'application/json',
      'Content-Type': 'application/x-www-form-urlencoded',
    };
     options['headers'] = this.getHeaders()
    return this.http
      .delete(`${environment.apiUrl}${entitySet}`, options)
      .pipe(catchError(this.formatErrors));
  }
  //   let datapost =
  //     'refresh_token=' +
  //     encodeURIComponent(refreshtoken) +
  //     '&grant_type=' +
  //     encodeURIComponent('refresh_token');
  //   const options: any = {};
  //   options['headers'] = {
  //     'Content-Type': 'application/x-www-form-urlencoded',
  //   };
  //   return this.http.post(
  //     `${environment.refresh_token}/token?key=AIzaSyBV-2Mwe5pQ--fE2mlAzLcE7107a_QOc54`,
  //     datapost,
  //     options
  //   );
  // }
}
