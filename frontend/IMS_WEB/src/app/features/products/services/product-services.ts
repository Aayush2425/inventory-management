import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { Observable } from 'rxjs';
import { Product, ProductWithInventoryCreate } from '../../../models/products/product';
import { ApiResponse } from '../../../models/apiResponse';

@Injectable({
  providedIn: 'root',
})
export class ProductServices {
  constructor(private apiService: ApiService) {}

  getProducts(searchTerm?: string, categoryFilter?: string): Observable<ApiResponse<Product[]>> {
    const queryParams = new URLSearchParams();
    if (searchTerm) {
      queryParams.append('query', searchTerm);
    }
    if (categoryFilter) {
      queryParams.append('category', categoryFilter);
    }
    const queryString = queryParams.toString();
    return this.apiService.get<ApiResponse<Product[]>>(`Product/search?${queryString}`);
  }

  createProduct(
    req: ProductWithInventoryCreate
  ): Observable<ApiResponse<ProductWithInventoryCreate>> {
    return this.apiService.post<ApiResponse<ProductWithInventoryCreate>>('Product', req);
  }

  updateProduct(
    req: ProductWithInventoryCreate
  ): Observable<ApiResponse<ProductWithInventoryCreate>> {
    console.log(req);

    return this.apiService.put<ApiResponse<ProductWithInventoryCreate>>(
      'Product',
      req.product.id,
      req
    );
  }

  onDeleteProduct(productId: number): Observable<ApiResponse<null>> {
    return this.apiService.delete<ApiResponse<null>>('Product', productId);
  }
}
