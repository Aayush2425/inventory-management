import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Salesorders } from './salesorders';

describe('Salesorders', () => {
  let component: Salesorders;
  let fixture: ComponentFixture<Salesorders>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Salesorders]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Salesorders);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
