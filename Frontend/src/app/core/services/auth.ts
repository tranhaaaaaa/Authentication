import { Injectable } from '@angular/core';
import { ApiService } from './api';
import { JsonConvert } from 'json2typescript';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Auth extends ApiService{
  constructor(protected override http: HttpClient) {
    super(http);
    const jsonConvert = new JsonConvert();
   }
  login(formData: any) {
     let url = `/api/auth/login`;
    return super.postEntity(url, formData).pipe(
      map((res) => {
        if (res === undefined) {
          throw new Error('Invalid response from server');
        }
        return res;
      })
    );
  }
}
