import { TestBed } from '@angular/core/testing';
import { CategoryService } from './category-service';
import { ApiService } from '../../../shared/api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../../../environments/environment';
describe('CategoryService', () => {
  let service: CategoryService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.baseUrl;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CategoryService, ApiService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(CategoryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should call GET /Category', () => {
    // arrange
    const mockResponse = { data: [] };

    service.getCategories().subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });
    // assign
    const req = httpMock.expectOne(`${baseUrl}/Category`);
    // assert
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should call POST /Category', () => {
    const category = { name: 'Electronics' };
    const mockResponse = { success: true };

    service.createCategory(category).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Category`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(category);
    req.flush(mockResponse);
  });

  it('should call PUT /Category/:id', () => {
    const category = { id: 10, name: 'Mobiles' };
    const mockResponse = { updated: true };

    service.updateCategory(category).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Category/10`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(category);
    req.flush(mockResponse);
  });

  it('should call DELETE /Category/:id', () => {
    const id = 5;
    const mockResponse = { deleted: true };

    service.deleteCategory(id).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Category/5`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });
});
