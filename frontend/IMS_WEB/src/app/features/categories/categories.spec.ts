import { TestBed } from '@angular/core/testing';
import { Categories } from './categories';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { of } from 'rxjs';
import { vi } from 'vitest';
import { CategoryService } from './services/category-service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('Categories Component', () => {
  let component: Categories;
  let fixture: any;

  const mockCategoryService = {
    getCategories: vi.fn().mockReturnValue(
      of({
        data: [
          { id: 1, name: 'Electronics', description: 'Devices' },
          { id: 2, name: 'Furniture', description: 'Chairs & Tables' },
        ],
      })
    ),
    createCategory: vi.fn().mockReturnValue(of({})),
    updateCategory: vi.fn().mockReturnValue(of({})),
    deleteCategory: vi.fn().mockReturnValue(of({})),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [Categories, ReactiveFormsModule, CommonModule],
      providers: [
        { provide: CategoryService, useValue: mockCategoryService },
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    fixture = TestBed.createComponent(Categories);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load categories on init', () => {
    expect(mockCategoryService.getCategories).toHaveBeenCalled();
    expect(component.categories().length).toBe(2);
  });

  it('should open modal for adding category', () => {
    component.openModal();
    expect(component.showModal()).toBe(true);
    expect(component.categoryForm.value).toEqual({ id: 0, name: '', description: '' });
  });

  it('should open modal with category data when editing', () => {
    const row = {
      id: 1,
      name: 'Electronics',
      description: 'Devices',
    };

    component.openModal(row);
    expect(component.categoryForm.value).toEqual(row);
    expect(component.showModal()).toBe(true);
  });

  it('should call createCategory when id = 0', () => {
    component.openModal();
    component.categoryForm.setValue({ id: 0, name: 'Test', description: 'Sample' });

    component.saveCategory();
    expect(mockCategoryService.createCategory).toHaveBeenCalled();
  });

  it('should call updateCategory when id != 0', () => {
    component.openModal({ id: 2, name: 'Edit', description: 'Updated', createdAt: new Date() });

    component.saveCategory();
    expect(mockCategoryService.updateCategory).toHaveBeenCalled();
  });

  it('should call deleteCategory', () => {
    component.deleteCategory(1);
    expect(mockCategoryService.deleteCategory).toHaveBeenCalledWith(1);
  });

  it('should set error if form invalid', () => {
    component.openModal();
    component.categoryForm.controls['name'].setValue('');
    component.saveCategory();
    expect(component.error()).toBe('Please fill in all required fields.');
  });
});
