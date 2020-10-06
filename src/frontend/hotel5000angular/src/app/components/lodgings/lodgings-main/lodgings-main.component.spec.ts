import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LodgingsMainComponent } from './lodgings-main.component';

describe('LodgingsMainComponent', () => {
  let component: LodgingsMainComponent;
  let fixture: ComponentFixture<LodgingsMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LodgingsMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LodgingsMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
