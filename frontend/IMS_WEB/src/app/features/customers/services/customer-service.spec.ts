import { TestBed } from '@angular/core/testing';
import { CustomerService } from './customer-service';
import { ApiService } from '../../../shared/api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../../../environments/environment';
describe('CustomerService', () => {
  let service: CustomerService;
  let httpMock: HttpTestingController;
  const baseUrl = environment.baseUrl;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomerService, ApiService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(CustomerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should call GET /Customers', () => {
    // arrange
    const mockResponse = { data: [] };
    const searchTerm = '';
    const wareHouseFilter = '';

    service.getCustomers(searchTerm, wareHouseFilter).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });
    // assign
    const req = httpMock.expectOne(`${baseUrl}/Customers/search?`);
    // assert
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should call POST /Customers', () => {
    const customer = {
      id: 0,
      name: 'aayush',
      email: 'aayush@gmail.com',
      phoneNumber: '8866628807',
      address: 'morbi',
      warehouseId: 1,
    };
    const mockResponse = { success: true, data: customer };

    service.createCustomer(customer).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Customers`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(customer);
    req.flush(mockResponse);
  });

  it('should call PUT /Customers/:id', () => {
    const customer = {
      id: 1,
      name: 'aayush',
      email: 'aayush@gmail.com',
      phoneNumber: '8866628807',
      address: 'morbi',
      warehouseId: 1,
    };
    const mockResponse = { updated: true };

    service.updateCustomer(customer).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Customers/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(customer);
    req.flush(mockResponse);
  });

  it('should call DELETE /Customers/:id', () => {
    const id = 5;
    const mockResponse = { deleted: true };

    service.deleteCustomer(id).subscribe((res) => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(`${baseUrl}/Customers/5`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });
});
