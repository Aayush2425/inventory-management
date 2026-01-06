import { Injectable } from '@angular/core';
import { ApiService } from '../../../shared/api.service';
import { ApiResponse } from '../../../models/apiResponse';
import { Category } from '../../../models/categories/category';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  constructor(private apiService: ApiService) {}
  getCategories() {
    return this.apiService.get<ApiResponse<Category[]>>('Category');
  }

  createCategory(category: any) {
    return this.apiService.post('Category', category);
  }
  updateCategory(category: any) {
    return this.apiService.put(`Category`, category.id, category);
  }
  deleteCategory(categoryId: number) {
    return this.apiService.delete(`Category`, categoryId);
  }
}
