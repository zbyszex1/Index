import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetArticlesComponent } from './get-articles.component';

describe('GetArticlesComponent', () => {
  let component: GetArticlesComponent;
  let fixture: ComponentFixture<GetArticlesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetArticlesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetArticlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
