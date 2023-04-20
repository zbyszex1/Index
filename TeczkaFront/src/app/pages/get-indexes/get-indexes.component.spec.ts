import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetIndexesComponent } from './get-indexes.component';

describe('GetIndexesComponent', () => {
  let component: GetIndexesComponent;
  let fixture: ComponentFixture<GetIndexesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetIndexesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetIndexesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
