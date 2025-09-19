import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private endpoints: any = null;

  constructor(private http: HttpClient) {}

  // load endpoints from assets/api-endpoints.json (returns a Promise-like Observable)
  loadEndpoints(): Observable<any> {
    if (this.endpoints) return from([this.endpoints]);
    // src\assets\api-endpoints.json
    return this.http.get('../assets/api-endpoints.json').pipe(
      map(e => {
        this.endpoints = e;
        return this.endpoints;
      })
    );
  }

  // helper to resolve a templated endpoint like /api/products/{id}
  private resolve(endpoint: string, params?: { [k: string]: any }) {
    if (!params) return endpoint;
    let out = endpoint;
    Object.keys(params).forEach(k => out = out.replace(`{${k}}`, encodeURIComponent(params[k])));
    return out;
  }

  // generic GET
  get(path: string, params?: any): Observable<any> {
    return this.loadEndpoints().pipe(
      map(endpoints => this.resolve(path, params)),
      switchMap(resolved => this.http.get(resolved))
    );
  }

  post(path: string, body: any, params?: any) {
    return this.loadEndpoints().pipe(
      map(endpoints => this.resolve(path, params)),
      switchMap(resolved => this.http.post(resolved, body))
    );
  }

  put(path: string, body: any, params?: any) {
    return this.loadEndpoints().pipe(
      map(endpoints => this.resolve(path, params)),
      switchMap(resolved => this.http.put(resolved, body))
    );
  }

  delete(path: string, params?: any) {
    return this.loadEndpoints().pipe(
      map(endpoints => this.resolve(path, params)),
      switchMap(resolved => this.http.delete(resolved))
    );
  }
}

// Add this to your components
// import { ApiService } from '../../core/services/api.service';
// constructor(private api: ApiService) {}

//   ngOnInit(): void {
//     // load endpoints then fetch products
//     this.api.loadEndpoints().subscribe(e => {
//       this.endpoints = e;
//       this.api.get(this.endpoints.products.list).subscribe((res: any) => {
//         this.products = res || [];
//       });
//     });