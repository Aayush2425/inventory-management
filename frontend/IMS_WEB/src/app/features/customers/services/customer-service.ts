import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Customer } from '../../../models/customers/customer';
import { ApiResponse } from '../../../models/apiResponse';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  constructor(private apiService: ApiService) {}

  getCustomers(searchTerm: string, wareHouseFilter: string): Observable<ApiResponse<Customer[]>> {
    const queryParams = new URLSearchParams();
    if (searchTerm) {
      queryParams.append('query', searchTerm);
    }
    if (wareHouseFilter) {
      queryParams.append('warehouse', wareHouseFilter);
    }
    const queryString = queryParams.toString();
    return this.apiService.get<ApiResponse<Customer[]>>(`Customers/search?${queryString}`);
  }

  createCustomer(customer: Customer) {
    return this.apiService.post<ApiResponse<Customer>>('Customers', customer);
  }

  updateCustomer(customer: Customer) {
    return this.apiService.put<ApiResponse<Customer>>(`Customers`, customer.id, customer);
  }
  deleteCustomer(customerId: number) {
    return this.apiService.delete<ApiResponse<null>>(`Customers`, customerId);
  }
}
