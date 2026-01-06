import { TestBed } from '@angular/core/testing';
import { SupplierService } from './supplier-service';
import { ApiService } from '../../../shared/api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../../../environments/environment';
describe('SupplierService', () => {
  let service: SupplierService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.baseUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SupplierService, ApiService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(SupplierService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should call GET /Suppliers', () => {
    // arrange
    const mockResponse = { data: [] };
    const searchTerm = '';
    const wareHouseFilter = '';

    service.getSuppliers(searchTerm, wareHouseFilter).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });
    // assign
    const req = httpMock.expectOne(`${baseUrl}/Suppliers/search?`);
    // assert
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should call POST /Suppliers', () => {
    const supplier = {
      id: 0,
      name: 'aayush',
      email: 'aayush@gmail.com',
      phoneNumber: '8866628807',
      address: 'morbi',
      warehouseId: 1,
    };
    const mockResponse = { success: true, data: supplier };

    service.createSupplier(supplier).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Suppliers`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(supplier);
    req.flush(mockResponse);
  });

  it('should call PUT /Suppliers/:id', () => {
    const supplier = {
      id: 1,
      name: 'aayush',
      email: 'aayush@gmail.com',
      phoneNumber: '8866628807',
      address: 'morbi',
      warehouseId: 1,
    };
    const mockResponse = { updated: true };

    service.updateSupplier(supplier).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Suppliers/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(supplier);
    req.flush(mockResponse);
  });

  it('should call DELETE /Suppliers/:id', () => {
    const id = 5;
    const mockResponse = { deleted: true };

    service.deleteSupplier(id).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Suppliers/5`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });
});
