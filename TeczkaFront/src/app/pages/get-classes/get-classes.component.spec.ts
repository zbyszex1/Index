import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetClassesComponent } from './get-classes.component';

describe('GetClassesComponent', () => {
  let component: GetClassesComponent;
  let fixture: ComponentFixture<GetClassesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetClassesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetClassesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
