import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetSectionsComponent } from './get-sections.component';

describe('GetSectionsComponent', () => {
  let component: GetSectionsComponent;
  let fixture: ComponentFixture<GetSectionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetSectionsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetSectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
