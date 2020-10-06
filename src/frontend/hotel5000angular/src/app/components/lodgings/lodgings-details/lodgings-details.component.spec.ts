import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LodgingsDetailsComponent } from './lodgings-details.component';

describe('LodgingsDetailsComponent', () => {
  let component: LodgingsDetailsComponent;
  let fixture: ComponentFixture<LodgingsDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LodgingsDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LodgingsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
