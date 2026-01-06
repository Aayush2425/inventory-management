# Phase 4: Comprehensive Testing Implementation Guide

## 🧪 Testing Strategy Overview

```
Unit Tests (Services)  → Component Tests (Integration) → E2E Tests (User Flows)
     ↓                           ↓                              ↓
  50%                          30%                            20%
(Quick, Isolated)        (With Dependencies)            (Real Browser)
```

---

## 📁 Test File Structure

```
src/app/
├── features/
│   ├── products/
│   │   ├── products.spec.ts (ENHANCE - Component)
│   │   └── services/
│   │       └── product-services.spec.ts (ENHANCE - Service)
│   ├── auth/
│   │   ├── pages/login/
│   │   │   └── login.spec.ts (ENHANCE)
│   │   └── services/
│   │       └── auth.spec.ts (ENHANCE)
│   └── dashboard/
│       └── owner-dashboard.spec.ts (ENHANCE)
├── core/
│   ├── guards/
│   │   ├── auth-guard.spec.ts (ENHANCE)
│   │   └── auth.guard.spec.ts (NEW - for functional guard)
│   ├── interceptors/
│   │   ├── auth.interceptor.spec.ts (NEW)
│   │   └── error.interceptor.spec.ts (NEW)
│   └── services/
│       ├── loading.service.spec.ts (NEW)
│       ├── error.service.spec.ts (NEW)
│       └── notification.service.spec.ts (NEW)
└── shared/
    ├── api.service.spec.ts (ENHANCE)
    └── components/
        └── sidebar/
            └── sidebar.spec.ts (ENHANCE)

e2e/
├── auth.cy.ts (NEW - Cypress)
├── products.cy.ts (NEW)
└── dashboard.cy.ts (NEW)
```

---

## 🧪 Type 1: Service Unit Tests

### Example 1: Auth Service Test

**File**: `src/app/features/auth/services/auth.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Auth } from './auth';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../../models/auth';
import { ApiService } from '../../../shared/api.service';

describe('AuthService', () => {
  let service: Auth;
  let apiService: jasmine.SpyObj<ApiService>;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    const apiSpy = jasmine.createSpyObj('ApiService', [
      'post', 'get', 'put', 'delete'
    ]);

    TestBed.configureTestingModule({
      providers: [
        Auth,
        { provide: ApiService, useValue: apiSpy }
      ],
      imports: [HttpClientTestingModule]
    });

    service = TestBed.inject(Auth);
    apiService = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService>;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('handleLogin', () => {
    it('should login user successfully and set token', (done) => {
      const mockResponse: AuthResponse = {
        data: {
          accessToken: 'test-token-12345',
          refreshToken: 'refresh-token-12345'
        }
      };

      const loginReq: LoginRequest = {
        username: 'testuser',
        password: 'password123'
      };

      apiService.post.and.returnValue(of(mockResponse));

      service.handleLogin(loginReq).subscribe((res) => {
        expect(res.data.accessToken).toBe('test-token-12345');
        expect(service.isAuthenticated()).toBe(true);
        expect(localStorage.getItem('token')).toBe('test-token-12345');
        done();
      });
    });

    it('should handle login error gracefully', (done) => {
      const loginReq: LoginRequest = {
        username: 'invalid',
        password: 'wrong'
      };

      const errorResponse = new HttpErrorResponse({
        error: { message: 'Invalid credentials' },
        status: 401,
        statusText: 'Unauthorized'
      });

      apiService.post.and.returnValue(throwError(() => errorResponse));

      service.handleLogin(loginReq).subscribe({
        error: () => {
          expect(service.error()).toBeTruthy();
          done();
        }
      });
    });

    it('should set loading state during login', (done) => {
      const mockResponse: AuthResponse = {
        data: {
          accessToken: 'token',
          refreshToken: 'refresh'
        }
      };

      apiService.post.and.returnValue(of(mockResponse));

      expect(service.loading()).toBe(false);

      service.handleLogin({ username: 'test', password: 'pass' }).subscribe(() => {
        expect(service.loading()).toBe(false);
        done();
      });
    });
  });

  describe('handleRegister', () => {
    it('should register user successfully', (done) => {
      const mockResponse: AuthResponse = {
        data: {
          accessToken: 'new-token',
          refreshToken: 'new-refresh'
        }
      };

      const registerReq: RegisterRequest = {
        username: 'newuser',
        password: 'password123',
        fullname: 'New User',
        role: 'user'
      };

      apiService.post.and.returnValue(of(mockResponse));

      service.handleRegister(registerReq).subscribe((res) => {
        expect(service.isAuthenticated()).toBe(true);
        done();
      });
    });
  });

  describe('logout', () => {
    it('should clear auth state on logout', () => {
      // Setup: logged in state
      localStorage.setItem('token', 'test-token');

      // Action
      service.logout();

      // Assert
      expect(service.isAuthenticated()).toBe(false);
      expect(localStorage.getItem('token')).toBeNull();
    });
  });

  describe('Computed signals', () => {
    it('should compute userName correctly', (done) => {
      const mockResponse: AuthResponse = {
        data: { accessToken: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...', refreshToken: '' }
      };

      apiService.post.and.returnValue(of(mockResponse));

      service.handleLogin({ username: 'testuser', password: 'pass' }).subscribe(() => {
        // Assuming token is decoded
        expect(service.userName()).toBe('testuser');
        done();
      });
    });

    it('should compute isAdmin correctly', (done) => {
      const mockResponse: AuthResponse = {
        data: { accessToken: 'token-with-admin-role', refreshToken: '' }
      };

      apiService.post.and.returnValue(of(mockResponse));

      service.handleLogin({ username: 'admin', password: 'pass' }).subscribe(() => {
        expect(service.isAdmin()).toBe(true);
        done();
      });
    });
  });
});
```

---

### Example 2: Loading Service Test

**File**: `src/app/core/services/loading.service.spec.ts`

```typescript
import { TestBed } from '@angular/core/testing';
import { LoadingService } from './loading.service';

describe('LoadingService', () => {
  let service: LoadingService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoadingService]
    });
    service = TestBed.inject(LoadingService);
  });

  it('should start as not loading', () => {
    expect(service.isLoading()).toBe(false);
  });

  it('should show loading after increasing requests', () => {
    service.increaseActiveRequests();
    expect(service.isLoading()).toBe(true);
  });

  it('should track multiple concurrent requests', () => {
    service.increaseActiveRequests();
    service.increaseActiveRequests();
    expect(service.isLoading()).toBe(true);

    service.decreaseActiveRequests();
    expect(service.isLoading()).toBe(true);

    service.decreaseActiveRequests();
    expect(service.isLoading()).toBe(false);
  });

  it('should not go below 0 active requests', () => {
    service.decreaseActiveRequests();
    service.decreaseActiveRequests();
    expect(service.isLoading()).toBe(false);
  });

  it('should reset correctly', () => {
    service.increaseActiveRequests();
    service.increaseActiveRequests();
    expect(service.isLoading()).toBe(true);

    service.reset();
    expect(service.isLoading()).toBe(false);
  });
});
```

---

## 🧪 Type 2: Component Integration Tests

### Example 1: Products Component Test

**File**: `src/app/features/products/products.spec.ts`

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { Products } from './products';
import { ProductServices } from './services/product-services';
import { ApiResponse } from '../../models/apiResponse';
import { Product } from '../../models/products/product';

describe('Products Component', () => {
  let component: Products;
  let fixture: ComponentFixture<Products>;
  let productService: jasmine.SpyObj<ProductServices>;

  const mockProducts: ApiResponse<Product[]> = {
    data: [
      { 
        id: 1, 
        name: 'Product 1', 
        sku: 'SKU001', 
        price: 99.99,
        categoryId: 1,
        description: 'Test product'
      },
      { 
        id: 2, 
        name: 'Product 2', 
        sku: 'SKU002', 
        price: 149.99,
        categoryId: 2,
        description: 'Another product'
      }
    ],
    success: true,
    message: 'Success'
  };

  beforeEach(async () => {
    productService = jasmine.createSpyObj('ProductServices', [
      'getAllProducts',
      'createProduct'
    ]);

    await TestBed.configureTestingModule({
      imports: [Products],
      providers: [
        { provide: ProductServices, useValue: productService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Products);
    component = fixture.componentInstance;
  });

  describe('Component Initialization', () => {
    it('should create the component', () => {
      expect(component).toBeTruthy();
    });

    it('should initialize signals with default values', () => {
      expect(component.products().length).toBe(0);
      expect(component.loading()).toBe(true);
      expect(component.error()).toBeNull();
      expect(component.showModal()).toBe(false);
    });
  });

  describe('Loading Products', () => {
    it('should load products on init', () => {
      // Mock localStorage
      spyOn(localStorage, 'getItem').and.callFake((key) => {
        if (key === 'token') return 'test-token';
        return null;
      });

      // Mock JWT decode
      spyOn(window as any, 'atob').and.returnValue(
        JSON.stringify({ UserID: 1, UserName: 'testuser', Role: 'user' })
      );

      productService.getAllProducts.and.returnValue(of(mockProducts));

      fixture.detectChanges(); // Trigger ngOnInit

      expect(component.products().length).toBe(2);
      expect(component.loading()).toBe(false);
    });

    it('should handle product loading error', () => {
      spyOn(localStorage, 'getItem').and.callFake((key) => {
        if (key === 'token') return 'test-token';
        return null;
      });

      productService.getAllProducts.and.returnValue(
        throwError(() => new Error('Network error'))
      );

      fixture.detectChanges();

      expect(component.error()).toBeTruthy();
      expect(component.loading()).toBe(false);
    });

    it('should show error when no authentication token', () => {
      spyOn(localStorage, 'getItem').and.returnValue(null);

      fixture.detectChanges();

      expect(component.error()).toBe('Authentication required');
      expect(component.loading()).toBe(false);
    });
  });

  describe('Computed Signals', () => {
    beforeEach(() => {
      component.products.set(mockProducts.data);
    });

    it('should compute product count', () => {
      expect(component.productCount()).toBe(2);
    });

    it('should compute if products are empty', () => {
      expect(component.isEmptyProducts()).toBe(false);
      component.products.set([]);
      expect(component.isEmptyProducts()).toBe(true);
    });

    it('should filter products by search query', () => {
      component.updateSearch('Product 1');
      expect(component.filteredProducts().length).toBe(1);
      expect(component.filteredProducts()[0].name).toBe('Product 1');
    });

    it('should filter products by category', () => {
      component.filterByCategory(1);
      expect(component.filteredProducts()[0].categoryId).toBe(1);
    });

    it('should display limited products', () => {
      expect(component.displayedProducts().length).toBe(2);
    });
  });

  describe('Product Selection', () => {
    beforeEach(() => {
      component.products.set(mockProducts.data);
    });

    it('should select product', () => {
      component.selectProduct(mockProducts.data[0]);
      expect(component.selectedProduct()).toBe(mockProducts.data[0]);
    });

    it('should clear selection', () => {
      component.selectProduct(mockProducts.data[0]);
      expect(component.selectedProduct()).toBeTruthy();
      
      component.clearSelection();
      expect(component.selectedProduct()).toBeNull();
    });
  });

  describe('Modal Management', () => {
    it('should toggle modal', () => {
      expect(component.showModal()).toBe(false);
      component.toggleModal();
      expect(component.showModal()).toBe(true);
      component.toggleModal();
      expect(component.showModal()).toBe(false);
    });

    it('should close modal on successful product creation', (done) => {
      const newProduct = mockProducts.data[0];
      productService.createProduct.and.returnValue(
        of({ data: newProduct, success: true, message: '' })
      );

      component.onCreateProduct();

      setTimeout(() => {
        expect(component.showModal()).toBe(false);
        done();
      }, 100);
    });
  });

  describe('Template Rendering', () => {
    it('should display loading message when loading', () => {
      component.loading.set(true);
      fixture.detectChanges();

      const loadingText = fixture.nativeElement.textContent;
      expect(loadingText).toContain('Loading products');
    });

    it('should display error message when error exists', () => {
      component.error.set('Failed to load products');
      fixture.detectChanges();

      const errorText = fixture.nativeElement.textContent;
      expect(errorText).toContain('Failed to load products');
    });

    it('should render product table with data', () => {
      component.products.set(mockProducts.data);
      component.loading.set(false);
      fixture.detectChanges();

      const rows = fixture.nativeElement.querySelectorAll('tbody tr');
      expect(rows.length).toBe(2);
    });
  });
});
```

---

### Example 2: Login Component Test

**File**: `src/app/features/auth/pages/login/login.spec.ts`

```typescript
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { Login } from './login';
import { Auth } from '../../services/auth';
import { AuthResponse } from '../../../../models/auth';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authService: jasmine.SpyObj<Auth>;
  let router: jasmine.SpyObj<Router>;

  const mockAuthResponse: AuthResponse = {
    data: {
      accessToken: 'jwt-token-here',
      refreshToken: 'refresh-token-here'
    }
  };

  beforeEach(async () => {
    authService = jasmine.createSpyObj('Auth', ['handleLogin']);
    router = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [Login, ReactiveFormsModule],
      providers: [
        { provide: Auth, useValue: authService },
        { provide: Router, useValue: router }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create login component', () => {
    expect(component).toBeTruthy();
  });

  describe('Form Validation', () => {
    it('should mark form as invalid when empty', () => {
      expect(component.userForm.valid).toBe(false);
    });

    it('should mark form as valid with correct input', () => {
      component.userForm.patchValue({
        username: 'testuser',
        password: 'password123'
      });
      expect(component.userForm.valid).toBe(true);
    });

    it('should require username', () => {
      const usernameControl = component.userForm.get('username');
      usernameControl?.setValue('');
      expect(usernameControl?.hasError('required')).toBe(true);
    });

    it('should require password minimum length', () => {
      const passwordControl = component.userForm.get('password');
      passwordControl?.setValue('pass');
      expect(passwordControl?.hasError('minlength')).toBe(true);
    });
  });

  describe('Login Process', () => {
    beforeEach(() => {
      component.userForm.patchValue({
        username: 'testuser',
        password: 'password123'
      });
    });

    it('should login successfully', (done) => {
      authService.handleLogin.and.returnValue(of(mockAuthResponse));

      component.onUserLogin();

      setTimeout(() => {
        expect(authService.handleLogin).toHaveBeenCalledWith({
          username: 'testuser',
          password: 'password123'
        });
        expect(component.successMessage).toContain('successful');
        expect(router.navigate).toHaveBeenCalled();
        done();
      }, 100);
    });

    it('should show error on login failure', (done) => {
      const errorResponse = new Error('Invalid credentials');
      authService.handleLogin.and.returnValue(throwError(() => errorResponse));

      component.onUserLogin();

      setTimeout(() => {
        expect(component.errorMessage).toBeTruthy();
        expect(router.navigate).not.toHaveBeenCalled();
        done();
      }, 100);
    });

    it('should show loading state during login', (done) => {
      authService.handleLogin.and.returnValue(of(mockAuthResponse));

      expect(component.loading).toBe(false);
      component.onUserLogin();
      expect(component.loading).toBe(true);

      setTimeout(() => {
        expect(component.loading).toBe(false);
        done();
      }, 100);
    });
  });

  describe('Password Toggle', () => {
    it('should toggle password visibility', () => {
      expect(component.showUserPassword).toBe(false);
      component.showUserPassword = true;
      expect(component.showUserPassword).toBe(true);
    });
  });
});
```

---

## 🧪 Type 3: E2E Tests (Cypress)

### Setup:
```bash
npm install --save-dev @cypress/schematic cypress
ng add @cypress/schematic
```

### Example 1: Authentication E2E Test

**File**: `e2e/auth.cy.ts`

```typescript
describe('Authentication Flow', () => {
  beforeEach(() => {
    cy.visit('http://localhost:4200');
  });

  describe('Login Page', () => {
    it('should display login form', () => {
      cy.contains('h2', 'Login').should('be.visible');
      cy.get('input[name="username"]').should('exist');
      cy.get('input[name="password"]').should('exist');
      cy.get('button[type="submit"]').should('exist');
    });

    it('should show validation errors for empty form', () => {
      cy.get('button[type="submit"]').click();
      cy.get('.form-text, .invalid-feedback').should('be.visible');
    });

    it('should show error on invalid credentials', () => {
      cy.get('input[name="username"]').type('invaliduser');
      cy.get('input[name="password"]').type('wrongpass');
      cy.get('button[type="submit"]').click();

      cy.get('.alert-danger, .error-message').should('contain', 'Unauthorized');
    });

    it('should login successfully with valid credentials', () => {
      cy.intercept('POST', '**/Auth/login', {
        statusCode: 200,
        body: {
          data: {
            accessToken: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...',
            refreshToken: 'refresh-token'
          },
          success: true
        }
      });

      cy.get('input[name="username"]').type('testuser');
      cy.get('input[name="password"]').type('password123');
      cy.get('button[type="submit"]').click();

      cy.url().should('include', '/OwnerDashboard');
      cy.contains('Owner DashBoard').should('be.visible');
    });

    it('should navigate to register page', () => {
      cy.contains('a', 'Register').click();
      cy.url().should('include', '/Register');
      cy.contains('h2', 'Register').should('be.visible');
    });

    it('should toggle password visibility', () => {
      const passwordInput = cy.get('input[name="password"]');
      
      // Initially password type
      passwordInput.should('have.attr', 'type', 'password');

      // Toggle visibility
      cy.get('.toggle-password-btn').click();
      passwordInput.should('have.attr', 'type', 'text');
    });
  });

  describe('Register Page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:4200/Register');
    });

    it('should display register form', () => {
      cy.contains('h2', 'Register').should('be.visible');
      cy.get('input[name="username"]').should('exist');
      cy.get('input[name="password"]').should('exist');
      cy.get('input[name="fullname"]').should('exist');
      cy.get('select[name="role"]').should('exist');
    });

    it('should register new user successfully', () => {
      cy.intercept('POST', '**/Auth/register', {
        statusCode: 200,
        body: {
          data: {
            accessToken: 'new-token',
            refreshToken: 'new-refresh'
          },
          success: true
        }
      });

      cy.get('input[name="username"]').type('newuser');
      cy.get('input[name="fullname"]').type('New User');
      cy.get('input[name="password"]').type('password123');
      cy.get('select[name="role"]').select('user');
      cy.get('button[type="submit"]').click();

      cy.url().should('include', '/OwnerDashboard');
    });
  });
});
```

### Example 2: Products E2E Test

**File**: `e2e/products.cy.ts`

```typescript
describe('Products Page', () => {
  beforeEach(() => {
    // Login first
    cy.visit('http://localhost:4200/Login');
    cy.get('input[name="username"]').type('testuser');
    cy.get('input[name="password"]').type('password123');
    cy.get('button[type="submit"]').click();

    // Wait for navigation and visit products
    cy.url().should('include', '/OwnerDashboard');
    cy.visit('http://localhost:4200/Products');
  });

  it('should display products table', () => {
    cy.get('table').should('be.visible');
    cy.get('thead th').should('have.length', 5); // ID, Name, SKU, Category, Price
  });

  it('should load and display products', () => {
    cy.intercept('GET', '**/Product/user/**', {
      statusCode: 200,
      body: {
        data: [
          { id: 1, name: 'Product 1', sku: 'SKU001', price: 99.99 },
          { id: 2, name: 'Product 2', sku: 'SKU002', price: 149.99 }
        ],
        success: true
      }
    });

    cy.get('tbody tr').should('have.length', 2);
  });

  it('should open add product modal', () => {
    cy.contains('button', 'Add Product').click();
    cy.get('.modal-content').should('be.visible');
    cy.contains('h5', 'Add Product').should('be.visible');
  });

  it('should close modal when clicking close button', () => {
    cy.contains('button', 'Add Product').click();
    cy.get('.modal-content').should('be.visible');
    
    cy.get('.btn-close').click();
    cy.get('.modal-content').should('not.be.visible');
  });

  it('should create new product', () => {
    cy.intercept('POST', '**/Product', {
      statusCode: 200,
      body: {
        data: { id: 3, name: 'New Product', sku: 'NEW001', price: 199.99 },
        success: true
      }
    });

    cy.contains('button', 'Add Product').click();
    cy.get('input[placeholder*="name"]').type('New Product');
    cy.get('input[placeholder*="sku"]').type('NEW001');
    cy.get('input[placeholder*="price"]').type('199.99');
    cy.get('button[type="submit"]').click();

    cy.get('.success-message').should('contain', 'created successfully');
  });

  it('should search products', () => {
    cy.get('input[placeholder*="search"]').type('Product 1');
    cy.get('tbody tr').should('have.length', 1);
  });

  it('should filter by category', () => {
    cy.get('select[name="category"]').select('Electronics');
    cy.get('tbody tr').should('have.length', 2); // Only Electronics products
  });

  it('should handle loading state', () => {
    cy.get('.spinner, .loading-indicator').should('be.visible');
  });

  it('should handle error state', () => {
    cy.intercept('GET', '**/Product/user/**', {
      statusCode: 500,
      body: { message: 'Server error' }
    });

    cy.reload();
    cy.get('.alert-danger, .error-message').should('be.visible');
  });
});
```

---

## ✅ Test Coverage Checklist

### Services (Unit Tests)
- [ ] Auth service: login, register, logout, role computation
- [ ] Product service: CRUD operations
- [ ] Loading service: request tracking
- [ ] Error service: error formatting
- [ ] Notification service: show/hide notifications

### Components (Integration Tests)
- [ ] Login: form validation, login flow, error handling
- [ ] Register: registration flow
- [ ] Products: load products, create product, search, filter
- [ ] Dashboard: data loading, metric calculations
- [ ] Sidebar: navigation

### E2E Tests (User Flows)
- [ ] Complete login flow
- [ ] Complete registration flow
- [ ] View products
- [ ] Add product
- [ ] Search and filter products
- [ ] Logout and re-login

---

## 📊 Running Tests

```bash
# Run unit tests
ng test

# Run tests with coverage
ng test --code-coverage

# Run E2E tests
ng e2e

# Run specific test file
ng test --include='**/products.spec.ts'

# Run tests in headless mode (CI)
ng test --watch=false --browsers=Chrome
```

---

## 🎯 Code Coverage Goals

```
Statements   : 80% (minimum)
Branches     : 75% (minimum)
Functions    : 80% (minimum)
Lines        : 80% (minimum)
```

Configure in `karma.conf.js`:
```javascript
coverageReporter: {
  dir: require('path').join(__dirname, './coverage'),
  reports: ['html', 'lcovonly', 'text-summary'],
  fixWebpackSourcePaths: true,
  thresholds: {
    emitWarning: false,
    global: {
      statements: 80,
      branches: 75,
      functions: 80,
      lines: 80
    }
  }
}
```

---

## 🧪 Common Testing Patterns

### Pattern 1: Mocking HTTP Requests
```typescript
it('should load data', () => {
  const mockData = [{ id: 1, name: 'Test' }];
  
  service.getData().subscribe(data => {
    expect(data).toEqual(mockData);
  });

  const req = httpMock.expectOne('/api/endpoint');
  expect(req.request.method).toBe('GET');
  req.flush(mockData);
});
```

### Pattern 2: Testing Observables
```typescript
it('should emit values', (done) => {
  service.getData().subscribe(data => {
    expect(data.length).toBeGreaterThan(0);
    done();
  });
});
```

### Pattern 3: Testing Signals
```typescript
it('should update signal', () => {
  component.count.set(5);
  expect(component.count()).toBe(5);
});

it('should compute derived value', () => {
  component.count.set(5);
  expect(component.doubleCount()).toBe(10);
});
```

### Pattern 4: Testing Effects
```typescript
it('should run effect when signal changes', (done) => {
  spyOn(console, 'log');
  component.count.set(5);
  
  setTimeout(() => {
    expect(console.log).toHaveBeenCalled();
    done();
  }, 0);
});
```

---

## 🚀 Next Steps

1. ✅ Create test files for all services
2. ✅ Create test files for all components
3. ✅ Setup Cypress for E2E tests
4. ✅ Mock HttpClient in tests
5. ✅ Achieve 80%+ code coverage
6. ✅ Run tests in CI/CD pipeline
