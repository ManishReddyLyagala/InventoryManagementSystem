import { Component } from '@angular/core';
import { ApiService } from '../../../services/apiservice/apiservice';

@Component({
  selector: 'app-test',
  standalone: true, 
  imports: [],
  templateUrl: './test.html',
  styleUrls: ['./test.scss']
})
export class Test 
{
  constructor(private api: ApiService) {}
  endpoints: any;
  ngOnInit(): void 
  {
    // load endpoints then fetch products
    this.api.loadEndpoints().subscribe(e => {
      this.endpoints = e;
      this.api.get(this.endpoints.products.list).subscribe((res: any) => {
        console.log(res);
      });
    });
  }
}
