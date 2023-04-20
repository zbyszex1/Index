import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetScansComponent } from './get-scans.component';

describe('GetScansComponent', () => {
  let component: GetScansComponent;
  let fixture: ComponentFixture<GetScansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetScansComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetScansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
