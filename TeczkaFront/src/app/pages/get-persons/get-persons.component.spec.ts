import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetPersonsComponent } from './get-persons.component';

describe('GetPersonsComponent', () => {
  let component: GetPersonsComponent;
  let fixture: ComponentFixture<GetPersonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetPersonsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetPersonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
