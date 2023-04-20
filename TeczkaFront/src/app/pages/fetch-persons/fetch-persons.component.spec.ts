import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FetchPersonsComponent } from './fetch-persons.component';

describe('FetchPersonsComponent', () => {
  let component: FetchPersonsComponent;
  let fixture: ComponentFixture<FetchPersonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FetchPersonsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FetchPersonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
