import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { ApiResponse } from '../../../models/apiResponse';
import { Supplier } from '../../../models/suppilers/supplier';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  constructor(private apiService: ApiService) {}

  getSuppliers(searchTerm?: string, categoryFilter?: string): Observable<ApiResponse<Supplier[]>> {
    const queryParams = new URLSearchParams();
    if (searchTerm) {
      queryParams.append('query', searchTerm);
    }
    if (categoryFilter) {
      queryParams.append('category', categoryFilter);
    }
    const queryString = queryParams.toString();
    return this.apiService.get<ApiResponse<Supplier[]>>(`Suppliers/search?${queryString}`);
  }

  createSupplier(supplier: Supplier) {
    return this.apiService.post<ApiResponse<Supplier>>('Suppliers', supplier);
  }

  updateSupplier(supplier: Supplier) {
    return this.apiService.put<ApiResponse<Supplier>>(`Suppliers`, supplier.id, supplier);
  }
  deleteSupplier(supplierId: number) {
    return this.apiService.delete<ApiResponse<null>>(`Suppliers`, supplierId);
  }
}
