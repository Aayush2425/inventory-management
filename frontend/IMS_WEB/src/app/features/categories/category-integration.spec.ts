import { TestBed } from '@angular/core/testing';
import { Categories } from './categories';
import { CategoryService } from './services/category-service';
import { ApiService } from '../../shared/api.service';
import { provideHttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { Auth } from '../auth/services/auth';

describe('Categories Integration Test', () => {
  let component: Categories;
  let fixture: any;
  let service: CategoryService;
  let authService: Auth;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [Categories],
      providers: [CategoryService, ApiService, provideHttpClient(), Auth],
    });

    fixture = TestBed.createComponent(Categories);
    component = fixture.componentInstance;
    service = TestBed.inject(CategoryService);
    authService = TestBed.inject(Auth);
  });

  it('should fetch categories from backend', async () => {
    // First, login to get auth tokens
    await firstValueFrom(authService.handleLogin({ username: 'OWNER', password: 'OWNER123' }));
    const categoriesResponse = await firstValueFrom(service.getCategories());
    expect(categoriesResponse.data).toBeDefined();
    expect(Array.isArray(categoriesResponse.data)).toBe(true);
  });

  it('should open modal & save via service integration', async () => {
    component.openModal();
    component.categoryForm.setValue({ id: 0, name: 'IntegrationTest', description: 'Testing' });
    component.saveCategory();
    expect(component.showModal()).toBe(false); // should close modal after save
  });
});
