import { Component, signal } from '@angular/core';
import { Category } from '../../models/categories/category';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../../shared/components/sidebar/sidebar';
import { CategoryService } from './services/category-service';

@Component({
  selector: 'app-categories',
  imports: [ReactiveFormsModule, CommonModule, Sidebar],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class Categories {
  categories = signal<Category[]>([]);
  categoryForm: FormGroup;
  error = signal<string | null>(null);
  showModal = signal(false);
  errorMessage = '';
  successMessage = '';

  constructor(private categoryService: CategoryService, private fb: FormBuilder) {
    this.categoryForm = this.fb.group({
      id: [0],
      name: ['', Validators.required],
      description: [''],
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }
  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories.set(response?.data ?? []);
      },
    });
  }
  openModal(category?: Category) {
    if (category) {
      this.categoryForm.setValue({
        id: category.id,
        name: category.name,
        description: category.description,
      });
    } else {
      this.categoryForm.reset({ id: 0, name: '', description: '' });
    }
    this.showModal.set(true);
  }
  closeModal() {
    this.showModal.set(false);
    this.error.set(null);
  }
  saveCategory() {
    if (this.categoryForm.invalid) {
      this.error.set('Please fill in all required fields.');
      return;
    }
    const categoryData = this.categoryForm.value;
    if (categoryData.id) {
      this.categoryService.updateCategory(categoryData).subscribe({
        next: () => {
          this.successMessage = 'Category updated successfully.';
          this.loadCategories();
          this.closeModal();
        },
      });
    } else {
      this.categoryService.createCategory(categoryData).subscribe({
        next: () => {
          this.successMessage = 'Category created successfully.';
          this.loadCategories();
          this.closeModal();
        },
      });
    }
  }
  deleteCategory(categoryId: number) {
    this.categoryService.deleteCategory(categoryId).subscribe({
      next: () => {
        this.successMessage = 'Category deleted successfully.';
        this.loadCategories();
      },
    });
  }
}
